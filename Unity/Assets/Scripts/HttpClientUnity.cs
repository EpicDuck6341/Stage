using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class HttpClientUnity : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();

    private async void OnTriggerEnter(Collider other)
    {
        // Check if the other collider belongs to an object with a specific tag.
        if (other.gameObject.name == "Sphere")
        {
            try
            {
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

    private async Task Start()
    {
        // Initialize the 'go' instance properly.
    }

    // Update is called once per frame
    private async Task Update()
    {
    }
}