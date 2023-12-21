using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentChange : MonoBehaviour
{
    private PlaceShells PS;
    private GrabObject GO;
    private ObjectNaming ON;
    private bool switched;

    // Start is called before the first frame update
    void Start()
    {
        PS = GameObject.Find("FirstPersonController").GetComponent<PlaceShells>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        ON = GameObject.Find("FirstPersonController").GetComponent<ObjectNaming>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!switched)
        {
            if (GO.heldObj == null && PS.size == 0 && !ON.answerGraded)
            {
                switched = true;
                GO.canPickup = false;
                // Invoke the SetAnswerGraded method with a delay of 0.5 seconds 
                Invoke("SetAnswerGraded", 5);
            }
        }
    }

// The method to be invoked after the delay
    void SetAnswerGraded()
    {
        ON.answerGraded = true;
    }
}