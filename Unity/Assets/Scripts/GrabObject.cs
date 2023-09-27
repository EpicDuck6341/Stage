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

    [Header("Physics Parameters")] private float pickupRange = 2.0f;
    private float pickupForce = 3.5f;

    public Camera mainCamera;
    private int originaLayer;


    private Coroutine distanceCheckCoroutine; // Added to store the coroutine

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                    // Start the coroutine when picking up an object
                    distanceCheckCoroutine = StartCoroutine(CheckObjectInCenter());
                }
            }
            else
            {
                ThrowObject();
                // Stop the coroutine when throwing the object
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
        }
    }

    private IEnumerator CheckObjectInCenter()
    {
        while (heldObj != null)
        {
            // Cast a ray from the center of the screen
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string objectName = hit.transform.gameObject.name;
                // Print the name of the object hit by the ray


                if (objectName == "Platform")
                {
                    // Calculate the new position for the held object (closer to the camera)
                    Vector3 newPosition =
                        hit.point - mainCamera.transform.forward *
                        0.05f; // Replace 'someDistance' with your desired distance
                    heldObj.transform.position = newPosition;
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    private void MoveObject()
    {
        RaycastHit hit;
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