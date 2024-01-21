
Certainly! Here are some corrections for grammar and spelling errors:

# 1. Project Requirements

Download the latest version of the project from [GitHub](https://github.com/EpicDuck6341/Stage).

Download and install [Unity Hub](https://unity.com/download).

Download and install the latest version (2021.3.16) of [Unity](https://unity.com/releases/editor/archive).

An NVIDIA GPU is required for Whisper.

## 1.1 Whisper Requirements

Download and install [Python version 3.9.9](https://www.python.org/downloads/release/python-399/). During installation, make sure to check the box that adds Python to PATH.

Download and install [CUDA](https://developer.nvidia.com/cuda-11-6-0-download-archive?target_os=Windows&target_arch=x86_64&target_version=11&target_type=exe_local).

Copy the command from [PyTorch](https://pytorch.org/get-started/locally/) into the Command Prompt to download PyTorch.

Download [FFmpeg](https://github.com/BtbN/FFmpeg-Builds/releases/download/autobuild-2024-01-17-12-52/ffmpeg-N-113344-gbe4fcf027b-win64-gpl.zip) and add the locations of the exes, located in the bin folder of the downloaded zip, to PATH.

Now open the main script located in the GitHub project in the code folder and import any packages that are required by Python. You can now run the Whisper Flask Web Server.

# 2. Contributing

Explanations of the most important classes and functions will be listed below.

## Python
The Python backend is a web server that functions by receiving audio from the Unity application, after which it transcribes the audio and returns the results.

### main.py

#### upload_audio()
The `upload_audio` function is used whenever audio is sent from the Unity application. It then makes use of `calculate_confidence` and `tokenizer`, which was made using [this](https://github.com/openai/whisper/discussions/284) discussion, to return the confidence. After this, it makes use of the `find_most_similar_word` function to return the required results. It then grades the answer based on confidence and similarity.

#### calculate_confidence()
This function takes the confidence of each token. Since these tokens do not always contain complete words, it strings them together where needed and takes the average confidence.

#### find_most_similar_word()
This function loops over the transcribed text and uses the Levenshtein Distance to calculate the similarity between words. Whenever a word's similarity is higher than the current highest, it overwrites it, and whenever the similarity is the same but the confidence is higher, it also overwrites the previous answer. After looping over the complete array, it returns the word with the highest similarity and confidence.

## C#

### AssignmentChange.cs
This script is used to set the 'state' of the game to the third assignment by setting the `answerGraded` variable to true. This starts the third assignment. This is only done whenever the user is not holding an object and whenever the shell array, used to pick shells to decorate the castle with, is empty. After which, it sets the `switched` variable to true so that it only switches once per playthrough.

### BlackScreen.cs
The function in this script is called during assignment two, whenever the player is teleported to another location. It slowly turns the image of a canvas, that covers the player's entire screen, to black. It then teleports the player and after a short delay fades the color back to transparent.

### BucketDetect.cs
This script is used in assignment one and two whenever something is dropped inside the bucket.

#### OnTriggerEnter(Collider collision)
This function checks if the name of the collided object matches. After which, it spawns the prefab that indicates how far the bucket is filled or raises its height whenever it is already present. It does this for both the shells in assignment two and the sand, called "Cube(Clone)," from assignment one.

### BucketUI.cs
This script contains functions that get called to increase or decrease the fill of the bucket indicator shown in the UI.

### CircularProgressBar.cs
Used to slowly drain the progress bar that is used during assignment three.

### CountdownManager.cs
This script starts and ends the timer used for the progress bar and keeps track if it is currently running.

### FirstPersonController.cs
First-person class taken from the [asset store](https://assetstore.unity.com/packages/3d/characters/modular-first-person-controller-189884) and adjusted to fit the game. Most notably, the option for movement has been taken away.

### GrabObject.cs
The `GrabObject` class is the main class of the program and handles all the objects that are held by the player. It also limits the objects that the player can and cannot use and how they can use them by keeping track of the game's current state through variables.

### GroundDetection.cs
This script is used to check if an object falls through the floor. It also contains a different class called `TransformData` which is used to store the position and rotation of the shell present in the scene.

#### OnCollisionEnter(Collision collision)
This function is used to check if any object has fallen through; this only happens whenever sand or shells miss the bucket. Whenever the object is a shell, it should be returned back to its original position instead of being destroyed. This is done by making use of the values stored in the dictionary that uses the `TransformData` class.

### HighlightObject.cs
This script is attached to an object that needs to highlight itself by glowing green. It also makes use of a public timer which is set to zero by default; this timer is in place for exceptions like the shovel used in assignment one which should only start glowing if the player has a hard time finding it.

### HttpClientUnity.cs
Used to communicate with the Whisper program through the web server. It is only used during assignment three.

#### sendAudio()
The function first sends the latest created audio file by using a counter from the `Recording` class that keeps track of the total number of files. It sends this along with the name of the current object in question (assignment three). It then awaits a response from the web server; this response will be Correct, Incorrect, or Repeat. After this, it increases the `answerIndex` to progress assignment three to the next object.

### ImageChanger.cs
The functions of this class are used in other classes to swap the current image that is shown during assignment three. The images are meant for when the player needs to talk, the grading process, and whenever the grade is given.

### NPCController.cs
This class is used during assignment three to move the NPC around in the scene and focus the player's camera on it while it does. Whenever the NPC arrives at the set destination, it triggers the start of the timer.

### NPCVoiceLines.cs
Class that contains all the voice lines said by the NPC and functions to play them.

### ObjectNaming.cs
This class is used to cycle through the questions of assignment three. It also switches the game 'state' to the next assignment whenever all the questions have been completed by using an index that gets increased with each question.

### ObjectNoises.cs
Class that contains all the noises made by the objects and a function to play them.

### Place

Shells.cs
This class is used during assignment two whenever the user is grabbing shells to place on the castle. Whenever the function is called, it returns a random shell from the array; this shell will then be put as the held object in the `GrabObject` class.

### RecordAudio.cs
This class records the audio from the player's microphone during assignment three. It starts recording whenever the timer is started and records for a total of eight seconds.

#### GenerateUniqueFileName()
This function keeps track of the total number of files in the recordings folder; it then adjusts the name of the new recording to be unique. So if there are five files, the count will be set to 6, and this will also be added to the name of the file. This count is also used in the http class to find the newest audio file to send.

### SpawnBarricade.cs
This class is used whenever the fourth assignment is started. It spawns transparent objects for where the player should place the barricade. It knows when to spawn due to assignment four variables which are set to true whenever the last question from the `ObjectNaming` class has been asked.

### SpawnCastle.cs
This class is used for building the sandcastle from assignment two. It has functions to spawn certain points of the castle; this function is called in the `GrabObject` class. When it is called, it also requires a name, which in this case is the name of the clicked object; the name indicates which object should be built on top. The castle also replaces the individual parts with the complete prefab whenever it is built.

### SpawnShells.cs
This class is used whenever the second assignment is started. It spawns transparent shells on the castle for where the player should place the shells near the end of the second assignment.

### StartUp.cs
This script simply starts the web server whenever the Unity Application is started and kills it whenever the application is closed.

### Waves.cs
Contains a function that is used during assignment four to raise and lower the wave height.

# 3. Recommendations

It is recommended that the `GrabObject` class is cleaned up and divided into different classes, so that all the different variables and conditions are more organized.

Additionally, the spawn classes should be combined to minimize repeated code.
