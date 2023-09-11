# IMPORTANT: This is just for using the local whisper dir as the package directly. Delete until next comment when just installing whisper normally.
import sys
from pathlib import Path

import fuzzywuzzy.fuzz

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
# end of dev import
import whisper

import colorsys
from typing import List
from whisper.tokenizer import get_tokenizer
from colorama import init, Style

import re
import sounddevice as sd
import wavio as wv

print('Loading model')
model = whisper.load_model("small")

# # Sampling frequency
# freq = 44100
#
# # Recording duration
# duration = 10
#
# # Start recorder with the given values
# # of duration and sample frequency
# print("Started Recording...")
# recording = sd.rec(int(duration * freq),
#                    samplerate=freq, channels=2)
#
# # Record audio for the given number of seconds
# sd.wait()
#
# # Convert the NumPy array to audio file
# wv.write("audio.wav", recording, freq, sampwidth=2)
# print("Recording Created.")


print('Loading audio')  # load audio and pad/trim it to fit 30 seconds
audio = whisper.load_audio("../audio.mp3")
audio = whisper.pad_or_trim(audio)

mel = whisper.log_mel_spectrogram(audio).to(
    model.device)  # make log-Mel spectrogram and move to the same device as the model

detect_lang = False
print('Decoding audio')  # decode the audio
options = whisper.DecodingOptions()
result = whisper.decode(model, mel, options)


def return_confidence(tokens: List[int], token_probs: List[float], tokenizer):
    init(autoreset=True)  # Initialize colorama
    text_tokens = [tokenizer.decode([t]) for t in tokens]

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

        # Average the sum of elements between start_index and end_index
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


def seek_answer(target_word):
    highest_sim = 0
    for index, word in enumerate(result_array):
        similarity = fuzzywuzzy.fuzz.ratio(target_word, word)
        if similarity > highest_sim:
            highest_sim = similarity
            sim_index = index

    print(f"Word: {result_array[sim_index]}, Target Word: {target_word}, Index: {sim_index}, Similarity: {highest_sim}, Confidence: {conf_array[sim_index]}")



tokenizer = get_tokenizer(multilingual=model.is_multilingual, language="dutch", task=options.task)
conf_array = return_confidence(result.tokens, result.token_probs,tokenizer)  # Print the text with colours, and return the confidence of each word.

# Use regular expression to split the text into words and punctuation marks
result_array = re.findall(r'\w+|[.,]', result.text)

seek_answer("Nederlands")
