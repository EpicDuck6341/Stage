import sys
from pathlib import Path
import fuzzywuzzy.fuzz
import whisper
import colorsys
from typing import List
from whisper.tokenizer import get_tokenizer
from colorama import init, Style
import re
import sounddevice as sd
import wavio as wv
import json
from flask import Flask, request, jsonify


app = Flask(__name__)

def load_model():
    print('Loading model')
    model = whisper.load_model("small")
    return model

def load_audio():

    # Sampling frequency
    freq = 44100

    # Recording duration
    duration = 10

    # Start recorder with the given values
    # of duration and sample frequency
    print("Started Recording...")
    recording = sd.rec(int(duration * freq),
                    samplerate=freq, channels=2)

    # Record audio for the given number of seconds
    sd.wait()

    # Convert the NumPy array to audio file
    wv.write("../audio/test.wav", recording, freq, sampwidth=2)
    print("Recording Created.")

    print('Loading audio')  # load audio and pad/trim it to fit 30 seconds
    audio = whisper.load_audio("../audio/test.wav")
    audio = whisper.pad_or_trim(audio)

    mel = whisper.log_mel_spectrogram(audio).to(model.device)  # make log-Mel spectrogram and move to the same device as the model

    return mel

def calculate_confidence(tokens: List[int], token_probs: List[float], tokenizer_temp):
    init(autoreset=True)  # Initialize colorama
    text_tokens = [tokenizer_temp.decode([t]) for t in tokens]

    # Get the indices of elements that start with a space
    indices = [index for index, element in enumerate(text_tokens) if element.startswith((' ', '.', ','))]

    # Initialize a variable to store the result
    confidence = []

    # Iterate through the indices and add elements between them
    for i in range(len(indices)):
        start_index = indices[i]
        if i < len(indices) - 1:
            end_index = indices[i + 1]
        else:
            end_index = len(token_probs)

        # Average of the elements between start_index and end_index
        confidence.append((sum(token_probs[start_index:end_index]) / (end_index - start_index)))

    for token, prob in zip(text_tokens, token_probs):
        # Interpolate between red and green in the HSV color space
        r, g, b = colorsys.hsv_to_rgb(prob * (1 / 3), 1, 1)
        r, g, b = int(r * 255), int(g * 255), int(b * 255)
        color_code = f"\033[38;2;{r};{g};{b}m"

        colored_token = f"{color_code}{Style.BRIGHT}{token}{Style.RESET_ALL}"
        print(colored_token, end="")

    print()
    return confidence

def find_most_similar_word(target_word, result_text, conf_array):
    # Use regular expression to split the text into words and punctuation marks
    result_array = re.findall(r'\w+|[.,]', result_text)
    sim_index = 0
    highest_sim = 0
    for index, word in enumerate(result_array):
        similarity = fuzzywuzzy.fuzz.ratio(target_word, word)  # Using Levenshtein Distance to calculate the similarity between two sequences
        if similarity > highest_sim:  # Search for the highest similarity in the array
            highest_sim = similarity
            sim_index = index

    print(f"Similar Word: {result_array[sim_index]}, Target Word: {target_word}, Index: {sim_index}, Similarity: {highest_sim}, Confidence: {conf_array[sim_index]}")
    return result_array[sim_index],target_word,highest_sim,conf_array[sim_index]

@app.route('/', methods=['GET'])
def main():
    mel = load_audio()
    options = whisper.DecodingOptions(fp16=False)
    result = whisper.decode(model, mel, options)

    tokenizer = get_tokenizer(multilingual=model.is_multilingual, language="dutch", task=options.task)
    conf_array = calculate_confidence(result.tokens, result.token_probs, tokenizer)  # Print the text with colors, and return the confidence of each word.

    similar_word,target_word,similarity, confidence = find_most_similar_word("Nederlands", result.text, conf_array)
    return json.dumps({'Target Word': target_word,
                       'Similar Word':similar_word,
                       'similarity': similarity,
                       'confidence': confidence})

if __name__ == "__main__":
    model = load_model()
    app.run()
