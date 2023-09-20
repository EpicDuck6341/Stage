using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GrabObject : MonoBehaviour
{
    [Header("Pickup Settings")] [SerializeField]
    private Transform holdArea;

    [HideInInspector]
    public GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")] [SerializeField]
    private float pickupRange = 5.0f;

    [SerializeField] private float pickupForce = 150.0f;

    private HttpClientUnity http;

    private void Start()
    {
        http = GetComponent<HttpClientUnity>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                        pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }

            if (heldObj != null)
            {
                MoveObject();
            }
        }

        void MoveObject()
        {
            if(Vector3.Distance(heldObj.transform.position,holdArea.position) > 0.1f)
            {
                Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
                heldObjRB.AddForce(moveDirection * pickupForce);
            }
        }

        void PickupObject(GameObject pickObj)
        {
            if (pickObj.GetComponent<Rigidbody>())
            {
                heldObjRB = pickObj.GetComponent<Rigidbody>();
                heldObjRB.useGravity = false;
                heldObjRB.drag = 10;
                heldObjRB.constraints = RigidbodyConstraints.FreezePosition;
                heldObjRB.transform.parent = holdArea;
                heldObj = pickObj;
            }
        }
        
        void DropObject(){
            heldObjRB.useGravity = true; 
            heldObjRB.drag = 1; 
            heldObjRB.constraints = RigidbodyConstraints.None; 
            heldObjRB.transform.parent = null; 
            heldObj = null;
            http.hasObjectBeenPickedUp = false;
        }
    }
}