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
    private BucketUI BUI;
    private NPCVoiceLines NPCV;

    private void Start()
    {
        NPCV = GameObject.Find("NPCVoiceLines").GetComponent<NPCVoiceLines>();
        PS = GameObject.Find("FirstPersonController").GetComponent<PlaceShells>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        BUI = GameObject.Find("BucketLevel").GetComponent<BucketUI>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent == null) // Check if the object has no parent
        {
            if (collision.gameObject.name == "Cube(Clone)")
            {
                Destroy(collision.gameObject);
                amount++;
                BUI.increaseFill(GO.sandPieces);
                if (!spawned)
                {
                    //Sand prefab used to show the level of how far the bucket has been filled
                    instantiatedPrefab = Instantiate(bucketFilled);
                    instantiatedPrefab.transform.SetParent(bucket.transform);
                    instantiatedPrefab.GetComponent<MeshCollider>().enabled = false;
                    instantiatedPrefab.GetComponent<BoxCollider>().enabled = false;
                    Vector3 newScale = new Vector3(0.095f, 0.095f, 0.095f);
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
                    StartCoroutine(AdjustHeight(instantiatedPrefab, 0.2f));
                    if (amount == GO.sandPieces)
                    {
                        NPCV.playAudio(4);
                    }
                }

                if (amount == GO.sandPieces)
                {
                    //Destroy all sand objects that are laying around whenever the bucket is filled
                    GameObject[] cubes = GameObject.FindGameObjectsWithTag("Sand");
                    NPCV.playAudio(4);
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
                BUI.increaseFill(GO.shellPieces);
                if (!spawned)
                {
                    //Shell prefab used to show the level of how far the bucket has been filled
                    Destroy(collision.gameObject);
                    instantiatedPrefab = Instantiate(shellFill);
                    instantiatedPrefab.transform.SetParent(bucket.transform);
                    instantiatedPrefab.GetComponent<MeshCollider>().enabled = false;
                    instantiatedPrefab.GetComponent<BoxCollider>().enabled = false;
                    Vector3 newScale = new Vector3(0.095f, 0.095f, 0.095f);
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
                    StartCoroutine(AdjustHeight(instantiatedPrefab, 0.12f));
                    if (amount == GO.shellPieces)
                    {
                        NPCV.playAudio(4);
                    }
                }
            }
        }
    }


    //Used to raise or lower the prefab used to indicate the fill level of the bucket
    public IEnumerator AdjustHeight(GameObject obj, float targetLevel)
    {
        if (obj == null)
        {
            // Object is already destroyed, exit the coroutine
            yield break;
        }

        float elapsedTime = 0f;
        float totalTime = 0.5f;
        float increment = targetLevel / totalTime;

        while (elapsedTime < totalTime)
        {
            if (obj == null)
            {
                // Object was destroyed mid-use, exit the coroutine
                yield break;
            }

            elapsedTime += Time.deltaTime;
            obj.transform.localPosition = new Vector3(
                obj.transform.localPosition.x,
                obj.transform.localPosition.y,
                obj.transform.localPosition.z + increment * Time.deltaTime
            );
            yield return null;
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