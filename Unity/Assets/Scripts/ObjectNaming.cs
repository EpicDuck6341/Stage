using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectNaming : MonoBehaviour
{
    //Used to navigate the NPC around the island
    public List<Vector3> walkingCoords;
    //Used to write out the names of the object in question,must be in th same sequence that the NPC walk
    public String[] coordNames;

    //Used to initiate the next object question/inpsection
    [HideInInspector] public bool answerGraded;
    private FirstPersonController FPC;
    private GrabObject GO;
    private CountdownManager CM;
    private NPCcontroller NPC;
    private AssignmentChange AC;
    [HideInInspector] public int index = 0;
    [HideInInspector] public int size;
    private bool switched;

    // Start is called before the first frame update
    void Start()
    {
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
        AC = GameObject.Find("FirstPersonController").GetComponent<AssignmentChange>();
        NPC = GameObject.Find("NPC").GetComponent<NPCcontroller>();
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

        }
        else if (answerGraded)
        {
            answerGraded = false;
            
            GO.canPickup = false;
            FPC.cameraCanMove = false;
            StartCoroutine(NPC.MoveNPC(10, walkingCoords[index]));
        }
    }
}