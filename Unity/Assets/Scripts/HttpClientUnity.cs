using System;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HttpClientUnity : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();
    private TextDisplay td;
    private float recordingStartTime = -1f; // Initialize to a negative value
    private float recordAfterSec = 3f;

    private void Start()
    {
        td = GetComponent<TextDisplay>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider belongs to an object with a specific tag.
        if (other.gameObject.name == "Sphere")
        {
            td.UpdateMessage("Goed gedaan", false, 1.5f);
            td.UpdateMessage("Zeg hardop in de microfoon wat je zojuist gegooid hebt", true, 1.5f);
            
            // Set the recording start time
            recordingStartTime = Time.time;
        }
    }

    private void Update()
    {
        // Check if it's time to start recording
        if (recordingStartTime >= 0 && Time.time - recordingStartTime >= recordAfterSec)
        {
            StartRecording();
            // Reset the recording start time to prevent it from triggering again
            recordingStartTime = -1f;
        }
    }

    private async void StartRecording()
    {
        try
        {
            Debug.Log("Recording Started");
            using HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:5000/");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.Log(responseBody);
        }
        catch (HttpRequestException e)
        {
            Debug.LogError("Exception Caught!");
            Debug.LogError("Message: " + e.Message);
        }
    }
}
