using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent == null) // Check if the object has no parent
        {
            if (collision.gameObject.name == "Cube(Clone)")
            {
                Destroy(collision.gameObject); // Destroy the cube when it collides with an object named "Ground"
            }
        }
    }
}