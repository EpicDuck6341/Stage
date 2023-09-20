using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class HttpClientUnity : MonoBehaviour
{
    private readonly HttpClient client = new HttpClient();
    private GrabObject go;
    [HideInInspector]
    public bool hasObjectBeenPickedUp = false; // Flag to track if the condition has been met

    private async Task Start()
    {
        // Initialize the 'go' instance properly.
        go = GetComponent<GrabObject>();
    }

    // Update is called once per frame
    private async Task Update()
    {
        if (go.heldObj != null && !hasObjectBeenPickedUp)
        {
            hasObjectBeenPickedUp = true; // Set the flag to true

            // Call asynchronous network methods in a try/catch block to handle exceptions.
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
}