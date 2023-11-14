using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceShells : MonoBehaviour
{
    //All possible shells are saved into a list
    public List<GameObject> shellArray = new List<GameObject>();
    public int size;
    private GameObject shell;
    private GameObject obj;

    void Start()
    {
        size = shellArray.Count;
    }

    //A random shell is selected and returned, later used as the held object in the GrabObject class
    public GameObject pickShell()
    {
        int index = Random.Range(0, size);
        shell = shellArray[index];
        shellArray.RemoveAt(index);
        obj = Instantiate(shell);
        size--;
        return obj;
    }



}