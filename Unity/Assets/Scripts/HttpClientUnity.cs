using System;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class HttpClientUnity : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();
    private float recordingStartTime = -1f; // Initialize to a negative value
    private float recordAfterSec = 3f;
    

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider belongs to an object with a specific tag.
        if (other.gameObject.name == "Sphere")
        {

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
