using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BucketDetect : MonoBehaviour
{
    //Class used for counting the amount inside of the bucket
    //Difference between the amount of sand and shells
    [HideInInspector] public int amount = 0;
    public GameObject bucketFilled;
    public GameObject shellFill;
    public GameObject bucket;
    private bool spawned;
    private GameObject instantiatedPrefab;
    private PlaceShells PS;
    private GrabObject GO;
    private ObjectNaming ON;

    private void Start()
    {
        PS = GameObject.Find("FirstPersonController").GetComponent<PlaceShells>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        ON = GameObject.Find("FirstPersonController").GetComponent<ObjectNaming>();
    }

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
                    //Sand prefab used to show the level of how far the bucket has been filled
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
                    //Increase the prefabs height
                    Vector3 newPos = new Vector3(instantiatedPrefab.transform.localPosition.x,
                        instantiatedPrefab.transform.localPosition.y,
                        instantiatedPrefab.transform.localPosition.z + 0.2f); // Adjust the local position here
                    instantiatedPrefab.transform.localPosition = newPos;
                }

                if (amount > 3)
                {
                    //Destroy all sand objects that are laying around whenever the bucket is filled
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
                if (!spawned)
                {
                    //Shell prefab used to show the level of how far the bucket has been filled
                    Destroy(collision.gameObject);
                    instantiatedPrefab = Instantiate(shellFill);
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
                    //Increase height
                    Vector3 newPos = new Vector3(instantiatedPrefab.transform.localPosition.x,
                        instantiatedPrefab.transform.localPosition.y,
                        instantiatedPrefab.transform.localPosition.z + 0.12f); // Adjust the local position here
                    instantiatedPrefab.transform.localPosition = newPos;
                }
            }
        }
    }

    private void Update()
    {
        //The amount in the bucket gets decreased trough the GrabObject class
        //Constantly check to see what needs to be done to the spawned variable, this regulates the prefab that is used for the fill height
        if (amount == 0)
        {
            spawned = false;
        }

        if (PS.size == 0)
        {
            Destroy(instantiatedPrefab);
        }
    }
}