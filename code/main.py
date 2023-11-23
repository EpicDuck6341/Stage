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

repeat = False;

# Define the upload folder for audio files
UPLOAD_FOLDER = "/UploadFolder"
Path(UPLOAD_FOLDER).mkdir(parents=True, exist_ok=True)


def load_model():
    print('Loading model')
    model = whisper.load_model("small")
    return model


def load_audio(file_path):
    # Load the uploaded audio file
    audio = whisper.load_audio(file_path)
    audio = whisper.pad_or_trim(audio)

    mel = whisper.log_mel_spectrogram(audio).to(model.device)
    return mel


# Modify the route to accept POST requests for audio upload
@app.route('/', methods=['POST'])
def upload_audio():
    global repeat
    try:
        # Check if the 'audio' and 'objectName' fields are in the request
        if 'audio' not in request.files or 'objectName' not in request.form:
            return "Missing file part or objectName", 400

        file = request.files['audio']
        object_name = request.form['objectName']

        # Check if the file has a valid filename
        if file.filename == '':
            return "No selected file", 400

        if file:
            # Save the uploaded file to the UPLOAD_FOLDER
            file_path = Path(UPLOAD_FOLDER) / "givenAnswer.wav"
            file.save(file_path)

            # Load the uploaded audio for processing
            mel = load_audio(file_path)
            options = whisper.DecodingOptions(fp16=False)
            result = whisper.decode(model, mel, options)

            tokenizer = get_tokenizer(multilingual=model.is_multilingual, language="dutch", task=options.task)
            conf_array = calculate_confidence(result.tokens, result.token_probs, tokenizer)

            similar_words = []

            # Find similar words for each target word
            target_word = object_name
            similar_word, _, similarity, confidence = find_most_similar_word(target_word, result.text, conf_array)
            similar_words.append({
                'Target Word': target_word,
                'Similar Word': similar_word,
                'similarity': similarity,
                'confidence': confidence
            })

            if similar_words[0]['confidence'] >= 0.6:
                if similar_words[0]['similarity'] == 100:
                    repeat = False
                    data = "Correct"
                    return json.dumps(data)
                else:
                    repeat = False
                    data = "Incorrect"
                    return json.dumps(data)
            else:
                if not repeat:
                    repeat = True
                    data = "Repeat"
                    return json.dumps(data)
                else:
                    repeat = False
                    data = "Incorrect"
                    return json.dumps(data)

    except Exception as e:
        repeat = True
        data = "Repeat"
        return json.dumps(data)


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
        similarity = fuzzywuzzy.fuzz.token_set_ratio(target_word,
                                                     word)  # Using Levenshtein Distance to calculate the similarity between two sequences
        if similarity > highest_sim:  # Search for the highest similarity in the array
            highest_sim = similarity
            sim_index = index
        else:
            if similarity == highest_sim and conf_array[sim_index] < conf_array[index]:
                highest_sim = similarity
                sim_index = index

    print(
        f"Similar Word: {result_array[sim_index]}, Target Word: {target_word}, Index: {sim_index}, Similarity: {highest_sim}, Confidence: {conf_array[sim_index]}")
    return result_array[sim_index], target_word, highest_sim, conf_array[sim_index]


@app.route('/', methods=['GET'])
def main():
    upload_audio()


if __name__ == "__main__":
    model = load_model()
    app.run()
