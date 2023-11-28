using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBarricade : MonoBehaviour
{
    public GameObject[] barricadeObjects;
    public Material transparentMat;

    private void Start()
    {
        SpawnAndConfigureObject(barricadeObjects[0], new Vector3(730, 0.5f, 800), new Quaternion(0, 0, 0, 0),
            new Vector3(0.45f, 0.45f, 0.45f),transparentMat);
        SpawnAndConfigureObject(barricadeObjects[1], new Vector3(730f, 0.35f, 798.5f), Quaternion.Euler(0, 0, 90),
            new Vector3(0.3f, 0.3f, 0.3f),transparentMat);
        SpawnAndConfigureObject(barricadeObjects[2], new Vector3(729.85f,0.35f,797.2f), Quaternion.Euler(0, 0, 90),
            new Vector3(0.15f, 0.15f, 0.15f),transparentMat);
        SpawnAndConfigureObject(barricadeObjects[2], new Vector3(730.15f,0.35f,797.2f), Quaternion.Euler(0, 0, -90),
            new Vector3(0.15f, 0.15f, 0.15f),transparentMat);
        SpawnAndConfigureObject(barricadeObjects[3], new Vector3(727, 0.5f, 800), new Quaternion(0, 0, 0, 0),
            new Vector3(0.3f, 0.3f, 0.3f),transparentMat);
            
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

        if (meshCollider != null && obj.GetComponent<MeshFilter>() != null)
        {
            meshCollider.sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
        }

        if (obj.GetComponent<Rigidbody>() != null)
        {
            obj.GetComponent<Rigidbody>() .isKinematic = true;
            obj.GetComponent<Rigidbody>() .mass = 1f;
        }

        return obj;
    }
}