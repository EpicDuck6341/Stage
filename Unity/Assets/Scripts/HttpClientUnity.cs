using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Object = System.Object;

public class HttpClientUnity : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();
    private string serverUrl = "http://127.0.0.1:5000"; 
    private RecordAudio RA;
    private ObjectNaming ON;
    private ImageChanger IC;

    private void Start()
    {
        RA = GameObject.Find("AudioManager").GetComponent<RecordAudio>();
        ON = GameObject.Find("FirstPersonController").GetComponent<ObjectNaming>();
        IC = GameObject.Find("Image").GetComponent<ImageChanger>();
    }


    public async void sendAudio()
    {
        try
        {
            var content = new MultipartFormDataContent();
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

            // Send the audio as a POST request
            HttpResponseMessage response = await client.PostAsync($"http://127.0.0.1:5000", content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            
            Debug.Log(responseBody);
            // Start the next question for assignment 3
            IC.setImage(IC.sprite[2]);
            Invoke("invoker",4);
        }
        catch (HttpRequestException e)
        {
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
}


