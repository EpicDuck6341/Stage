import whisper
import sounddevice as sd
import wavio as wv

# Sampling frequency
freq = 44100

# Recording duration
duration = 10

# Start recorder with the given values
# of duration and sample frequency
print("Started Recording.")
recording = sd.rec(int(duration * freq),
                   samplerate=freq, channels=2)

# Record audio for the given number of seconds
sd.wait()

# Convert the NumPy array to audio file
wv.write("audio.wav", recording, freq, sampwidth=2)
print("Recording Created.")

# Select model for use
model = whisper.load_model("small")
# Transcribe chosen audio file
result = model.transcribe("audio.wav", language="dutch", fp16=False, verbose=True)
print(result["text"])
