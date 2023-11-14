using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShells : MonoBehaviour
{
    public GameObject shell1;
    public GameObject shell2;
    public GameObject shell3;
    public Material transparentMatRed;
    public Material transparentMatGreen;
    public Material transparentMatBlue;
    private SpawnCastle SC;
    private bool shellSpawned;

    // Start is called before the first frame update
    void Start()
    {
        SC = GameObject.Find("Bucket").GetComponent<SpawnCastle>();
    }

    private void SpawnAndConfigureObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale,
        Material material, string tag)
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

        obj.tag = tag;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (SC.towerBuilt)
        {
            if (!shellSpawned)
            {
                //Links
                SpawnAndConfigureObject(shell1, new Vector3(730.24f, 0.39f, 798.416f), Quaternion.Euler(0, 0, 0),
                    new Vector3(0.3f, 0.3f, 0.3f), transparentMatRed,"Shell1");
                //Midden 
                SpawnAndConfigureObject(shell1, new Vector3(730, 0.345f, 798.36f), Quaternion.Euler(0, 0, 0),
                    new Vector3(0.3f, 0.3f, 0.3f), transparentMatRed,"Shell1");
                //Rechts
                SpawnAndConfigureObject(shell1, new Vector3(729.76f, 0.39f, 798.416f), Quaternion.Euler(0, 0, 0),
                    new Vector3(0.3f, 0.3f, 0.3f), transparentMatRed,"Shell1");
                //Links
                SpawnAndConfigureObject(shell2, new Vector3(730.243f, 0.485f, 798.3f), Quaternion.Euler(0, -90, -90),
                    new Vector3(0.3f, 0.3f, 0.3f), transparentMatGreen,"Shell2");
                //Rechst
                SpawnAndConfigureObject(shell2, new Vector3(729.77f, 0.485f, 798.3f), Quaternion.Euler(0, -90, -90),
                    new Vector3(0.3f, 0.3f, 0.3f), transparentMatGreen,"Shell2");
                //Midden
                SpawnAndConfigureObject(shell3, new Vector3(730, 0.939f, 798.08f), Quaternion.Euler(-35, 0, 0),
                    new Vector3(0.4f, 0.4f, 0.4f), transparentMatBlue,"Shell3");
                shellSpawned = true;
            }
        }
    }
}