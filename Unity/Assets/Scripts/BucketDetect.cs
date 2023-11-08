using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BucketDetect : MonoBehaviour
{
    [HideInInspector] public int amount = 0;
    public GameObject bucketFilled;
    public GameObject bucket;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent == null) // Check if the object has no parent
        {
            if (collision.gameObject.name == "Cube(Clone)")
            {
                Destroy(collision.gameObject);
                amount++;
                if (amount > 3)
                {
                    GameObject[] cubes = GameObject.FindGameObjectsWithTag("Sand");
                    foreach (GameObject cube in cubes)
                    {
                        Destroy(cube);
                    }
                    GameObject instantiatedPrefab = Instantiate(bucketFilled);
                    instantiatedPrefab.transform.SetParent(bucket.transform);
                    Vector3 newScale = new Vector3(0.1f, 0.1f, 0.1f);
                    instantiatedPrefab.transform.localScale = newScale;
                    Vector3 newPos = new Vector3(0, 0, 0.7f); // Adjust the local position here
                    instantiatedPrefab.transform.localPosition = newPos;
                    instantiatedPrefab.transform.localRotation =
                        Quaternion.Euler(0, 0, 0); // Adjust the local rotation here
                }
            }
        }
    }
}