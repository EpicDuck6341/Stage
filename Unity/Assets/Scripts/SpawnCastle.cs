using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnCastle : MonoBehaviour
{
    public GameObject frontRightCastle;
    public GameObject frontLeftCastle;
    public GameObject backRightCastle;
    public GameObject backLeftCastle;
    public GameObject frontPrefabRight;
    public GameObject backPrefabLeft;
    public GameObject frontPrefabLeft;
    public GameObject backPrefabRight;
    public Material castleMaterial;
    public Material floorMaterial;
    private Vector3 spawnPosition;
    private GrabObject GO;
    private GameObject frontLeft;
    private GameObject backLeft;
    private GameObject frontRight;
    private GameObject backRight;
    [HideInInspector] public bool bottomBuilt;
    [HideInInspector] public bool[] spawn = { false,false,false,false};
   


    private void Start()
    {
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        spawnPosition = new Vector3(730, 0.22f, 798);
        Vector3 scale = new Vector3(10, 10, 1);
        frontLeft = SpawnAndConfigureObject(frontPrefabLeft, spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
            floorMaterial);
        backLeft = SpawnAndConfigureObject(backPrefabLeft, spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
            floorMaterial);

        spawnPosition = new Vector3(730, 0.22023f, 798);
        frontRight = SpawnAndConfigureObject(frontPrefabRight, spawnPosition, Quaternion.Euler(90, 0, 180), scale,
            floorMaterial);
        backRight = SpawnAndConfigureObject(backPrefabRight, spawnPosition, Quaternion.Euler(90, 0, 180), scale,
            floorMaterial);
    }

    public void spawnCastlePart(string castlePart)
    {
        Vector3 scale = new Vector3(10, 10, 10);

        if (castlePart == "FloorBackLeft(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22023f, 798);
            SpawnAndConfigureObject(backLeftCastle, spawnPosition, Quaternion.Euler(-90, 0, 0), scale, castleMaterial);
            spawn[1] = true;
            spawn[0] = false;
        }
        else if (castlePart == "FloorBackRight(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22023f, 798);
            SpawnAndConfigureObject(backRightCastle, spawnPosition, Quaternion.Euler(-90, 0, 0), scale, castleMaterial);
            spawn[2] = true;
            spawn[1] = false;
        }
        else if (castlePart == "FloorFrontRight(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22f, 798);
            SpawnAndConfigureObject(frontRightCastle, spawnPosition, Quaternion.Euler(-90, 0, 0), scale,
                castleMaterial);
            spawn[3] = false;
            bottomBuilt = true;
        }
        else if (castlePart == "FloorFrontLeft(Clone)")
        {
            spawnPosition = new Vector3(730, 0.22f, 798);
            SpawnAndConfigureObject(frontLeftCastle, spawnPosition, Quaternion.Euler(-90, 0, 0), scale, castleMaterial);
            spawn[3] = true;
            spawn[2] = false;
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

    private void setMaterial(GameObject go)
    {
        if (go != null)
        {
            if (go.GetComponent<Renderer>().material.color != Color.green)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material newMaterial = new Material(renderer.material);
                    newMaterial.color = Color.green;
                    renderer.material = newMaterial;
                }

                go.tag = "On";
            }
        }
    }

    private void Update()
    {
        if (GO.heldObj != null)
        {
            // if (GO.heldObj.name == "Bucket")
            // {
            
                if (spawn[0])
                {
                    setMaterial(backLeft);
                }
                else if (spawn[1])
                {
                    setMaterial(backRight);
                }
                else if (spawn[2])
                {
                    setMaterial(frontLeft);
                }
                else if (spawn[3])
                {
                    setMaterial(frontRight);
                }
            
        }
    }
}