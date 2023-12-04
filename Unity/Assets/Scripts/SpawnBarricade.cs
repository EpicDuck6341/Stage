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
            new Vector3(0.45f, 0.45f, 0.45f), transparentMat, barricadeObjects[0].name);
        SpawnAndConfigureObject(barricadeObjects[1], new Vector3(730f, 0.35f, 798.5f), Quaternion.Euler(0, 0, 90),
            new Vector3(0.3f, 0.3f, 0.3f), transparentMat, barricadeObjects[1].name);
        SpawnAndConfigureObject(barricadeObjects[2], new Vector3(729.85f, 0.35f, 797.2f), Quaternion.Euler(0, 0, 90),
            new Vector3(0.15f, 0.15f, 0.15f), transparentMat, barricadeObjects[2].name);
        SpawnAndConfigureObject(barricadeObjects[2], new Vector3(730.15f, 0.35f, 797.2f), Quaternion.Euler(0, 0, -90),
            new Vector3(0.15f, 0.15f, 0.15f), transparentMat, barricadeObjects[2].name);
        SpawnAndConfigureObject(barricadeObjects[3], new Vector3(729.142f, 0.338f, 798.117f),
            Quaternion.Euler(90, 0, 0),
            new Vector3(0.3f, 0.3f, 0.35f), transparentMat, barricadeObjects[3].name);
        SpawnAndConfigureObject(barricadeObjects[3], new Vector3(730.87f, 0.338f, 798.117f),
            Quaternion.Euler(90, 0, 0),
            new Vector3(0.3f, 0.3f, 0.35f), transparentMat, barricadeObjects[3].name);
    }

    private void SpawnAndConfigureObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale,
        Material material, string name)
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
            obj.GetComponent<Rigidbody>().isKinematic = true;
            obj.GetComponent<Rigidbody>().mass = 1f;
        }

        obj.name = name;
    }
}