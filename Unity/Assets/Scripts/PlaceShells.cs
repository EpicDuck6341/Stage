using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceShells : MonoBehaviour
{
    public List<GameObject> shellArray = new List<GameObject>();
    public int size;
    private GameObject shell;
    private GameObject obj;

    void Start()
    {
        size = shellArray.Count;
    }

    public GameObject pickShell()
    {
        int index = Random.Range(0, size);
        shell = shellArray[index];
        shellArray.RemoveAt(index);
        obj = Instantiate(shell);
        size--;
        return obj;
    }


// Update is called once per frame
    void Update()
    {
    }
}