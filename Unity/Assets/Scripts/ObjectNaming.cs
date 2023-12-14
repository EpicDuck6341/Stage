using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectNaming : MonoBehaviour
{
    //Used to navigate the NPC around the island
    public List<Vector3> walkingCoords;
    //Used to write out the names of the object in question,must be in th same sequence that the NPC walk
    public String[] coordNames;
    public GameObject[] objects;
    //Used to initiate the next object question/inpsection
    [HideInInspector] public bool answerGraded;
    private FirstPersonController FPC;
    private GrabObject GO;
    private NPCcontroller NPC;
    private BlackScreen BS; 
    [HideInInspector] public int index = 0;
    [HideInInspector] public int size;
    private bool switched;

    // Start is called before the first frame update
    void Start()
    {
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        NPC = GameObject.Find("NPC").GetComponent<NPCcontroller>();
        BS = GameObject.Find("CountdownManager").GetComponent<BlackScreen>();
        size = walkingCoords.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (index == size && !switched)
        {
            switched = true;
            answerGraded = false;
            FPC.cameraCanMove = true;
            GO.canPickup = true;
            GO.assignmentFour = true;
            BS.reticle.GetComponent<Image>().enabled = true;

        }
        else if (answerGraded && !switched)
        {
            BS.reticle.GetComponent<Image>().enabled = false;
            answerGraded = false;
            
            GO.canPickup = false;
            FPC.cameraCanMove = false;
            StartCoroutine(NPC.MoveNPC(8, walkingCoords[index],objects[index]));
        }
    }
}