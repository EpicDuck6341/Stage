using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BucketDetect : MonoBehaviour
{
    [HideInInspector] public int amount = 0;
    public GameObject bucketFilled;
    public GameObject bucket;
    private bool spawned;
    private GameObject instantiatedPrefab;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent == null) // Check if the object has no parent
        {
            if (collision.gameObject.name == "Cube(Clone)")
            {
                Destroy(collision.gameObject);
                amount++;
                if (!spawned)
                {
                    instantiatedPrefab = Instantiate(bucketFilled);
                    instantiatedPrefab.transform.SetParent(bucket.transform);
                    instantiatedPrefab.GetComponent<MeshCollider>().enabled = false;
                    instantiatedPrefab.GetComponent<BoxCollider>().enabled = false;
                    Vector3 newScale = new Vector3(0.1f, 0.1f, 0.1f);
                    instantiatedPrefab.transform.localScale = newScale;
                    Vector3 newPos = new Vector3(0, 0, 0.1f); // Adjust the local position here
                    instantiatedPrefab.transform.localPosition = newPos;
                    instantiatedPrefab.transform.localRotation =
                        Quaternion.Euler(0, 0, 0); // Adjust the local rotation here
                    spawned = true;

                }
                else if (spawned)
                {
                    Vector3 newPos = new Vector3(instantiatedPrefab.transform.localPosition.x, instantiatedPrefab.transform.localPosition.y, instantiatedPrefab.transform.localPosition.z+0.2f); // Adjust the local position here
                    instantiatedPrefab.transform.localPosition = newPos;   
                }
                if (amount > 3)
                {
                    GameObject[] cubes = GameObject.FindGameObjectsWithTag("Sand");
                    foreach (GameObject cube in cubes)
                    {
                        Destroy(cube);
                    }
                    
                }
            }
            else if (collision.gameObject.name == "Shell")
            {
                Destroy(collision.gameObject);
                amount++;
            }
        }
        
    }

    private void Update()
    {
        if (amount == 0)
        {
            spawned = false;
        }
    }
}