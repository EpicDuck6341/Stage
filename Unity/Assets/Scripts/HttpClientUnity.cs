using System.IO;
using System.Net.Http;
using System.Collections;
using UnityEngine;

public class HttpClientUnity : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();
    private string serverUrl = "http://127.0.0.1:5000";
    private RecordAudio RA;
    private ObjectNaming ON;
    private ImageChanger IC;
    private NPCcontroller NPC;
    private CountdownManager CM;
    private NPCVoiceLines NPCV;
    //Keeps track of the currents expected answer for playing the audio file
    private int answerIndex;

    private void Start()
    {
        NPCV = GameObject.Find("NPCVoiceLines").GetComponent<NPCVoiceLines>();
        RA = GameObject.Find("AudioManager").GetComponent<RecordAudio>();
        ON = GameObject.Find("FirstPersonController").GetComponent<ObjectNaming>();
        IC = GameObject.Find("Image").GetComponent<ImageChanger>();
        NPC = GameObject.Find("NPC").GetComponent<NPCcontroller>();
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
    }


    public async void sendAudio()
    {
        try
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            string fileName = "givenAnswer";
            if (RA.count > 0)
            {
                fileName = fileName + (RA.count - 1) + ".wav";
            }

            // Load the audio file as bytes
            string audioFilePath = Path.Combine(Application.dataPath, "Audio/Recordings/" + fileName);
            byte[] audioBytes = File.ReadAllBytes(audioFilePath);


            // Create a ByteArrayContent with the audio data
            ByteArrayContent audioContent = new ByteArrayContent(audioBytes);
            content.Add(audioContent, "audio", fileName);

            // Add the objectName as StringContent
            string objectName = ON.coordNames[ON.index]; // Replace with the actual objectName
            StringContent objectNameContent = new StringContent(objectName);
            content.Add(objectNameContent, "objectName");

            // Send the audio and objectName as a POST request
            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.Log(responseBody);

            if (responseBody == "\"Correct\"")
            {
                // Start the next question for assignment 3
                NPCV.playAnswer(answerIndex);
                answerIndex++;
                IC.setImage(IC.sprite[2]);
                ON.index++;
                Invoke("invoker", 4);
            }
            else if (responseBody == "\"Incorrect\"")
            {
                NPCV.playAnswer(answerIndex);
                answerIndex++;
                ON.index++;
                IC.setImage(IC.sprite[2]);
                Invoke("invoker", 4);
            }
            else if (responseBody == "\"Repeat\"")
            {
                //Play audio to repeat answer
                //Restart the question
                StartCoroutine(PlayAudio());
            }
        }
        catch (HttpRequestException e)
        {
            ON.index++;
            IC.clearImage();
            ON.answerGraded = true;
            Debug.LogError("Exception Caught!");
            Debug.LogError("Message: " + e.Message);
        }
    }

    private void invoker()
    {
        IC.clearImage();
        ON.answerGraded = true;
    }

    private IEnumerator PlayAudio()
    {
        IC.clearImage();
        NPCV.playAudio(12);

        NPC.anim.SetBool("talk", true);

        yield return new WaitWhile(() => NPCV.aud.isPlaying);

        NPC.anim.SetBool("talk", false);

        CM.StartTimer(5);
    }
}