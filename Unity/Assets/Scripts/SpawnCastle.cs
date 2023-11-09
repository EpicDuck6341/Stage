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
    public Material floorMaterial;
    public Material green;
    private Vector3 spawnPosition;
    private GrabObject GO;
    private List<GameObject> castleParts = new();
    private bool replaced;
    [HideInInspector] public bool towerBuilt;
    [HideInInspector] public bool bottomBuilt;
    [HideInInspector] public bool[] spawn = { false, false, false, false };


    private void Start()
    {
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        spawnPosition = new Vector3(730, 0.22f, 798);
        Vector3 scale = new Vector3(10, 10, 1);
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[7], spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
            floorMaterial));
        castleParts.Add( SpawnAndConfigureObject(castlePrefabs[6], spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
            floorMaterial));

        spawnPosition = new Vector3(730, 0.22023f, 798);
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[5], spawnPosition, Quaternion.Euler(90, 0, 180), scale,
            floorMaterial));
        castleParts.Add(SpawnAndConfigureObject(castlePrefabs[8], spawnPosition, Quaternion.Euler(90, 0, 180), scale,
            floorMaterial));
    }

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
            castleParts.Add( SpawnAndConfigureObject(castlePrefabs[1], spawnPosition, Quaternion.Euler(-90, 0, 0),
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
            setMaterial(castleParts[9], "Tower", green);
        }
        else if (castlePart == "tower1(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[11], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            setMaterial(castleParts[10], "Tower", green);
            setMaterial(castleParts[9], "Untagged", castleMaterial);
        }
        else if (castlePart == "tower2(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[12], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            setMaterial(castleParts[11], "Tower", green);
            setMaterial(castleParts[10], "Untagged", castleMaterial);
        }
        else if (castlePart == "tower3(Clone)")
        {
            spawnPosition = new Vector3(730, 0.595f, 798);
            castleParts.Add(SpawnAndConfigureObject(castlePrefabs[13], spawnPosition, Quaternion.Euler(-90, 0, 0),
                scale,
                castleMaterial));
            setMaterial(castleParts[11], "Untagged", castleMaterial);
            towerBuilt = true;
        }
    }

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
            if (bottomBuilt)
            {
                spawnPosition = new Vector3(730, 0.221f, 798);
                SpawnAndConfigureObject(castlePrefabs[0], spawnPosition, Quaternion.Euler(-90, 0, 0),
                    new Vector3(10, 10, 10),
                    castleMaterial);
                spawnPosition = new Vector3(730, 0.338f, 798);
                castleParts.Add(SpawnAndConfigureObject(castlePrefabs[9], spawnPosition, Quaternion.Euler(90, 0, 0),
                    new Vector3(17, 17, 17),
                    floorMaterial));
                setMaterial(castleParts[8], "Tower", green);
                Destroy(castleParts[5]);
                Destroy(castleParts[7]);
                Destroy(castleParts[4]);
                Destroy(castleParts[6]);
                replaced = true;
            }
        }

        if (GO.heldObj != null)
        {
            if (spawn[0])
            {
                setMaterial(castleParts[1], "On", green);
            }
            else if (spawn[1])
            {
                setMaterial(castleParts[3], "On", green);
            }
            else if (spawn[2])
            {
                setMaterial(castleParts[0], "On", green);
            }
            else if (spawn[3])
            {
                setMaterial(castleParts[2], "On", green);
            }
        }
    }
}