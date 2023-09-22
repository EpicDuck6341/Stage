using UnityEngine;

public class CollissionHandler : MonoBehaviour
{
    // This method is called when a collision occurs.
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        // Check if the collision involves a specific tag.
        if (collision.gameObject.CompareTag("YourTag"))
        {
            // Print a message to the console.
            Debug.Log("Collision detected with object tagged 'YourTag'.");
        }
    }
}