using UnityEngine;

public class ColisionHandler : MonoBehaviour
{
// This method is called when a trigger collision occurs.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider belongs to an object with a specific tag.
        if (other.gameObject.name == "Sphere")
        {
            // Print a message to the console.
            Debug.Log("Trigger collision detected with object tagged 'YourTag'.");
        }
    }
}

