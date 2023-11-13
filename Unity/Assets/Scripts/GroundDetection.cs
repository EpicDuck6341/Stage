using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    //All the shells in the scene
    public GameObject[] shells;
    //Save the data of the shells for transporting them back when they fall out of the map
    private Dictionary<int, TransformData> shellsPos = new Dictionary<int, TransformData>();

    private class TransformData
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public TransformData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }


    private void Start()
    {
        for (int i = 0; i < shells.Length; i++)
        {
            GameObject shell = shells[i];
            int instanceId = shell.GetInstanceID();
            Vector3 position = shell.transform.position;
            Quaternion rotation = shell.transform.rotation;

            // Store instance ID as key and position and rotation as value in the dictionary
            shellsPos[instanceId] = new TransformData(position, rotation);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent == null) // Check if the object has no parent
        {
            if (collision.gameObject.name == "Cube(Clone)")
            {
                Destroy(collision.gameObject); // Destroy the cube when it collides with an object named "Ground"
            }
            else if (collision.gameObject.name == "Shell")
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                int key = collision.gameObject.GetInstanceID();
                TransformData transformData = shellsPos[key];
                Vector3 position = transformData.Position;
                Quaternion rotation = transformData.Rotation;

                collision.gameObject.transform.localPosition = position;
                collision.gameObject.transform.localRotation = rotation;
            }
        }
    }
}