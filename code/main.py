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

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))

print('Loading model')
model = whisper.load_model("small")

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
wv.write("../audio/audio.wav", recording, freq, sampwidth=2)
print("Recording Created.")

print('Loading audio')  # load audio and pad/trim it to fit 30 seconds
audio = whisper.load_audio("../audio/audio.mp3")
audio = whisper.pad_or_trim(audio)

mel = whisper.log_mel_spectrogram(audio).to(model.device)  # make log-Mel spectrogram and move to the same device as the model

detectLang = False
print('Decoding audio')  # decode the audio
options = whisper.DecodingOptions()
result = whisper.decode(model, mel, options)


def calculateConfidence(tokens: List[int], tokenProbs: List[float], tokenizerTemp):
    init(autoreset=True)  # Initialize colorama
    textTokens = [tokenizerTemp.decode([t]) for t in tokens]

    # Get the indices of elements that start with a space
    indices = [index for index, element in enumerate(textTokens) if element.startswith((' ', '.', ','))]

    # Initialize a variable to store the result
    confidence = []

    # Iterate through the indices and add elements between them
    for i in range(len(indices)):
        startIndex = indices[i]
        if i < len(indices) - 1:
            endIndex = indices[i + 1]
        else:
            endIndex = len(tokenProbs)

        # Average of the elements between startIndex and endIndex
        confidence.append((sum(tokenProbs[startIndex:endIndex]) / (endIndex - startIndex)))

    for token, prob in zip(textTokens, tokenProbs):
        # Interpolate between red and green in the HSV color space
        r, g, b = colorsys.hsv_to_rgb(prob * (1 / 3), 1, 1)
        r, g, b = int(r * 255), int(g * 255), int(b * 255)
        colorCode = f"\033[38;2;{r};{g};{b}m"

        coloredToken = f"{colorCode}{Style.BRIGHT}{token}{Style.RESET_ALL}"
        print(coloredToken, end="")

    print()
    return confidence


def findMostSimilarWord(targetWord):
    # Use regular expression to split the text into words and punctuation marks
    resultArray = re.findall(r'\w+|[.,]', result.text)
    simIndex = 0
    highestSim = 0
    for index, word in enumerate(resultArray):
        similarity = fuzzywuzzy.fuzz.ratio(targetWord, word)  # Using Levenshtein Distance to calculate the similarity between two sequences
        if similarity > highestSim: # Search for the highest similarity in the array
            highestSim = similarity
            simIndex = index

    print(f"Similar Word: {resultArray[simIndex]}, Target Word: {targetWord}, Index: {simIndex}, Similarity: {highestSim}, Confidence: {confArray[simIndex]}")

tokenizer = get_tokenizer(multilingual=model.is_multilingual, language="dutch", task=options.task)
confArray = calculateConfidence(result.tokens, result.token_probs, tokenizer)  # Print the text with colors, and return the confidence of each word.

findMostSimilarWord("Nederlands")
