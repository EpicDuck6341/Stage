using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnCastle : MonoBehaviour
{
    public GameObject[] castlePrefabs;
    public Material castleMaterial;
    public Material white;
    private Vector3 spawnPosition;
    private GrabObject GO;
    private List<GameObject> castleParts = new();
    private bool replaced;
    [HideInInspector] public bool towerBuilt;
    [HideInInspector] public bool bottomBuilt;
    [HideInInspector] public bool[] spawn = { false, false, false, false };
    private bool[] castlePartsProcessed = { false, false, false, false };


    private void Start()
    {
        //Spawn floor tiles on which the bottom part will be built
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        spawnPosition = new Vector3(730, 0.22f, 798);
        Vector3 scale = new Vector3(10, 10, 1);
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[7], spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
            white));
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[6], spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
            white));

        spawnPosition = new Vector3(730, 0.22023f, 798);
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[5], spawnPosition, Quaternion.Euler(90, 0, 180), scale,
            white));
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[8], spawnPosition, Quaternion.Euler(90, 0, 180), scale,
            white));
    }

    //Used in the GrabObject class
    //The name of  object hit with the ray is used as the parameter
    //Then just if statements to match the correct bottom part to the correct floor part
    public void spawnCastlePart(string castlePart)
    {
        Vector3 scale = new Vector3(10, 10, 10);

        if (castlePart == "FloorBackLeft(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22023f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[4], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            spawn[1] = true;
            spawn[0] = false;
        }
        else if (castlePart == "FloorBackRight(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22023f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[3], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            spawn[2] = true;
            spawn[1] = false;
        }
        else if (castlePart == "FloorFrontRight(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[1], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            spawn[3] = false;
            bottomBuilt = true;
        }
        else if (castlePart == "FloorFrontLeft(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[2], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            spawn[3] = true;
            spawn[2] = false;
        }
    }

    //Used in the GrabObject class
    //The name of  object hit with the ray is used as the parameter
    //Then just if statements to match the correct tower part to the correct hit part
    public void spawnTowerPart(string castlePart)
    {
        Vector3 scale = new Vector3(10, 10, 10);

        if (castlePart == "towerBottom(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[10], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            Destroy(castleParts[8]);
            setMaterial(castleParts[9], "Tower", white);
            castleParts[9].AddComponent<HighlightObject>();
        }
        else if (castlePart == "tower1(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[11], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            setMaterial(castleParts[10], "Tower", white);
            castleParts[10].AddComponent<HighlightObject>();
            Destroy(castleParts[9].GetComponent<HighlightObject>());
            setMaterial(castleParts[9], "Untagged", castleMaterial);
        }
        else if (castlePart == "tower2(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[12], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            setMaterial(castleParts[11], "Tower", white);
            castleParts[11].AddComponent<HighlightObject>();
            Destroy(castleParts[10].GetComponent<HighlightObject>());
            setMaterial(castleParts[10], "Untagged", castleMaterial);
        }
        else if (castlePart == "tower3(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[13], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            Destroy(castleParts[11].GetComponent<HighlightObject>());
            setMaterial(castleParts[11], "Untagged", castleMaterial);
            towerBuilt = true;
        }
    }

    //Used for spawning objects
    private GameObject SpawnAndConfigureObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale,
        Material material)
    {
        GameObject obj = Instantiate(prefab, position, rotation);
        obj.transform.localScale = scale;

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }

        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        Rigidbody rigidbody = obj.AddComponent<Rigidbody>();

        if (meshCollider != null && obj.GetComponent<MeshFilter>() != null)
        {
            meshCollider.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
        }

        if (rigidbody != null)
        {
            rigidbody.isKinematic = true;
            rigidbody.mass = 1f;
        }

        return obj;
    }

    private void setMaterial(GameObject go, string tag, Material mat)
    {
        if (go != null)
        {
            if (go.GetComponent<Renderer>().material.name != "Green")
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material newMaterial = new Material(renderer.material);
                    newMaterial = mat;
                    renderer.material = newMaterial;
                }

                go.tag = tag;
            }
        }
    }

    private void Update()
    {
        if (!replaced)
        {
            if (bottomBuilt && !replaced)
            {
                //Replace all the loose castle parts with a single pre-built castle prefab
                spawnPosition = new Vector3(730, 0.221f, 798);
                SpawnAndConfigureObject(castlePrefabs[0], spawnPosition, Quaternion.Euler(-90, 0, 0),
                    new Vector3(10, 10, 10),
                    castleMaterial);
                spawnPosition = new Vector3(730, 0.338f, 798);
                castleParts.Add(SpawnAndConfigureObject(castlePrefabs[9], spawnPosition, Quaternion.Euler(90, 0, 0),
                    new Vector3(17, 17, 17),
                    white));
                setMaterial(castleParts[8], "Tower", white);
                castleParts[8].AddComponent<HighlightObject>();
                Destroy(castleParts[5]);
                Destroy(castleParts[7]);
                Destroy(castleParts[4]);
                Destroy(castleParts[6]);
                replaced = true;
            }
        }

        if (GO.heldObj != null)
        {
// Turn the color of the floor parts green whenever the matching castle part has spawned
            if (spawn[0] && !castlePartsProcessed[1])
            {
                castleParts[1].tag = "On";
                castleParts[1].AddComponent<HighlightObject>();
                castlePartsProcessed[1] = true;
            }
            else if (spawn[1] && !castlePartsProcessed[3])
            {
                castleParts[3].tag = "On";
                castleParts[3].AddComponent<HighlightObject>();
                castlePartsProcessed[3] = true;
            }
            else if (spawn[2] && !castlePartsProcessed[0])
            {
                castleParts[0].tag = "On";
                castleParts[0].AddComponent<HighlightObject>();
                castlePartsProcessed[0] = true;
            }
            else if (spawn[3] && !castlePartsProcessed[2])
            {
                castleParts[2].tag = "On";
                castleParts[2].AddComponent<HighlightObject>();
                castlePartsProcessed[2] = true;
            }
        }
    }
}