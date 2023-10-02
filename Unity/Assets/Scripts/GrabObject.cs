using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabObject : MonoBehaviour
{
    [Header("Pickup Settings")] [SerializeField]
    private Transform holdArea;

    [HideInInspector] public GameObject heldObj;
    private Rigidbody heldObjRB;
    private Collider heldObjC;

    [Header("Physics Parameters")] private float pickupRange = 2.0f;
    private float pickupForce = 3.5f;

    public Camera mainCamera;
    private int originaLayer;

    public bool canPickup = true;


    private Coroutine distanceCheckCoroutine; // Added to store the coroutine

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (canPickup)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (heldObj == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit,
                            pickupRange))
                    {
                        PickupObject(hit.transform.gameObject);
                        // Check for collisions with the SphereCollider
                    }
                }
                else
                {
                    ThrowObject();
                    if (distanceCheckCoroutine != null)
                    {
                        StopCoroutine(distanceCheckCoroutine);
                        distanceCheckCoroutine = null;
                    }
                }
            }

            if (heldObj != null)
            {
                MoveObject();
                CheckCubeCollision();
            }
        }
    }


    private void CheckCubeCollision()
    {
        if (heldObjC != null)
        {
            Collider objCollider = heldObjC.GetComponent<Collider>();

            if (objCollider != null)
            {
                if (Physics.CheckBox(objCollider.bounds.center, objCollider.bounds.extents))
                {
                    Collider hitCollider = null;
                    RaycastHit hit;
                    if (Physics.Raycast(objCollider.bounds.center, -mainCamera.transform.forward, out hit, 0.1f))
                    {
                        hitCollider = hit.collider;
                    }
                    
                    if (hitCollider != null && hitCollider.gameObject.name == "Platform")
                    {
                        Vector3 newPosition = heldObj.transform.position - mainCamera.transform.forward * 0.05f;
                        heldObj.transform.position = newPosition;
                    }
                }
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
            originaLayer = pickObj.layer;
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.constraints = RigidbodyConstraints.FreezePosition;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
            heldObjC = pickObj.GetComponent<Collider>();
            pickObj.layer = LayerMask.NameToLayer("PickedUpObjectLayer");
        }
    }


    private void ThrowObject()
    {
        heldObj.layer = originaLayer;
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