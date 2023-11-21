using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNaming : MonoBehaviour
{
    public List<GameObject> namingObjects;

    //Used to initiate the next object question/inpsection
    [HideInInspector] public bool answerGraded;
    private FirstPersonController FPC;
    private GrabObject GO;
    private CountdownManager CM;
    private NPCcontroller NPC;
    [HideInInspector] public int index = 0;
    [HideInInspector] public int size;

    // Start is called before the first frame update
    void Start()
    {
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
        NPC = GameObject.Find("NPC").GetComponent<NPCcontroller>();
        size = namingObjects.Count;
        answerGraded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (index == size)
        {
            answerGraded = false;
        }
        else if (answerGraded)
        {
            answerGraded = false;
            
            GO.canPickup = false;
            FPC.cameraCanMove = false;
            StartCoroutine(NPC.MoveNPC(10, namingObjects[index].transform.position));
            index++;
        }
    }
}