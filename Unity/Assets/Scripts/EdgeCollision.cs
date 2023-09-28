using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollision : MonoBehaviour
{
    private Vector3 ballPos;
    private Vector3 bucketPos;
    private Vector3 shovelPos;

    public bool isCollision = false;
    
    private GrabObject GO;
    

    private Dictionary<string, Vector3> objectPositions;
    private Dictionary<string, bool> hasCollided = new Dictionary<string, bool>();


    private void Start()
    {
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        ballPos = GameObject.Find("Ball").transform.position;
        bucketPos = GameObject.Find("Bucket").transform.position;
        shovelPos = GameObject.Find("Shovel").transform.position;
        // Initialize the dictionary with the predefined positions
        objectPositions = new Dictionary<string, Vector3>
        {
            { "Ball", ballPos },
            { "Bucket", bucketPos },
            { "Shovel", shovelPos }
        };
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (gameObject.name != "Platform")
        {
            string objectName = collision.gameObject.name;

            // Check if the object has already triggered a collision
            if (!hasCollided.ContainsKey(objectName) || !hasCollided[objectName])
            {
                // Mark the object as collided
                hasCollided[objectName] = true;

                StartCoroutine(WaitAndHandleCollision(collision));
            }
        }
        else
        {
            // Handle the collision here without waiting
            HandleCollision(collision);
        }
    }

    private IEnumerator WaitAndHandleCollision(Collider collision)
    {
        GO.canPickup = false;
        // Wait for 2 seconds (you can adjust the duration as needed)
        yield return new WaitForSeconds(2.0f);

        // After waiting, handle the collision
        HandleCollision(collision);
    }

    private void HandleCollision(Collider collision)
    {
        // Handle the collision here
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        string objectName = collision.gameObject.name;

        // Check if the object name is in the dictionary
        if (objectPositions.ContainsKey(objectName))
        {
            // Get the position from the dictionary and assign it to the object
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            collision.gameObject.transform.rotation = Quaternion.identity;
            collision.gameObject.transform.position = objectPositions[objectName];
            hasCollided[objectName] = false;
            isCollision = true;
        }
    }
}