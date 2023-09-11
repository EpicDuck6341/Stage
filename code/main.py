# IMPORTANT: This is just for using the local whisper dir as the package directly. Delete until next comment when just installing whisper normally.
import sys
from pathlib import Path
sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
# end of dev import
import whisper

import colorsys
from typing import List
from whisper.tokenizer import get_tokenizer
from colorama import init, Style


print('Loading model')
model = whisper.load_model("small")


print('Loading audio') # load audio and pad/trim it to fit 30 seconds
audio = whisper.load_audio("../audio.mp3")
audio = whisper.pad_or_trim(audio)


mel = whisper.log_mel_spectrogram(audio).to(model.device) # make log-Mel spectrogram and move to the same device as the model


detect_lang = False
print('Decoding audio') # decode the audio
options = whisper.DecodingOptions()
result = whisper.decode(model, mel, options)


def print_colored_text(tokens: List[int], token_probs: List[float], tokenizer):
    init(autoreset=True)  # Initialize colorama
    text_tokens = [tokenizer.decode([t]) for t in tokens]

    # Get the indices of elements that start with a space
    indices = [index for index, element in enumerate(text_tokens) if element.startswith((' ', '.', ','))]


    # Initialize a variable to store the result
    result = []

    # Iterate through the indices and add elements between them
    for i in range(len(indices)):
        start_index = indices[i]
        if i < len(indices) - 1:
            end_index = indices[i + 1]
        else:
            end_index = len(token_probs)

        # Concatenate elements between start_index and end_index
        summed_value = sum(token_probs[start_index:end_index])
        average_value = summed_value/(end_index-start_index)
        result.append(average_value)

    # Print the result
    print(result)

    for token, prob in zip(text_tokens, token_probs):
        # Interpolate between red and green in the HSV color space
        r, g, b = colorsys.hsv_to_rgb(prob * (1/3), 1, 1)
        r, g, b = int(r * 255), int(g * 255), int(b * 255)
        color_code = f"\033[38;2;{r};{g};{b}m"

        colored_token = f"{color_code}{Style.BRIGHT}{token}{Style.RESET_ALL}"
        print(colored_token, end="")



    print()

tokenizer = get_tokenizer(multilingual=model.is_multilingual, language="dutch", task=options.task)
print_colored_text(result.tokens, result.token_probs, tokenizer)  # print text with fancy confidence colors




