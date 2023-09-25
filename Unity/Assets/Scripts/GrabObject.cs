using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabObject : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdArea;

    [HideInInspector] public GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    private float pickupRange = 0.6f; 
    private float pickupForce = 3.5f;
    
    public Camera mainCamera;
    private TextDisplay td;

    private void Start()
    {
        mainCamera = Camera.main;
        td = GetComponent<TextDisplay>();
        td.UpdateMessage("Pak de rode bal op",false,0);
    }

    private void Update()
    {
        if (td == null)
        {
            td = GetComponent<TextDisplay>();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                ThrowObject();
            }

            if (heldObj != null)
            {
                MoveObject();
            }
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    private void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.constraints = RigidbodyConstraints.FreezePosition;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
            td.UpdateMessage("Gooi de rode bal in de hoepel",false,0);
        }
    }


    private void ThrowObject()
    {
        // Calculate the throw direction as the camera's forward direction
        Vector3 throwDirection = mainCamera.transform.forward;

        // Release the object and apply force to it
        heldObjRB.useGravity = true;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObjRB.AddForce(throwDirection * pickupForce, ForceMode.Impulse);

        heldObj = null;
    }
}
