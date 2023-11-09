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

    [Header("Physics Parameters")] private float pickupRange = 8.0f;
    private float pickupForce = 3.5f;

    public Camera mainCamera;
    private int originaLayer;
    private FirstPersonController FPC;
    private BucketDetect BD;
    private SpawnCastle SC;
    public GameObject sand;

    public bool canPickup = true;


    private Coroutine distanceCheckCoroutine; // Added to store the coroutine

    private Vector3 initialPosition;
    private bool shovelEmpty = true;
    private Vector3 oPos;
    private Vector3 oScale;
    private Quaternion oRotation;
    private bool bucketFull;


    private void Start()
    {
        mainCamera = Camera.main;
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        BD = GameObject.Find("Bucket").GetComponent<BucketDetect>();
        SC = GameObject.Find("Bucket").GetComponent<SpawnCastle>();
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
                        if (hit.collider.gameObject.tag == "PickUp")
                        {
                            PickupObject(hit.transform.gameObject);
                        }
                    }
                }
                else if (BD.amount <= 3)
                {
                    ShovelSand(); //Used for picking up sand
                }
                else if (bucketFull)
                {
                    UseBucket();
                }
                else
                {
                    ThrowObject();
                }
            }

            if (heldObj != null)
            {
                MoveObject();
            }
        }
    }


    private void ShovelSand()
    {
        if (shovelEmpty)
        {
            RaycastHit hit;
            if (Physics.Raycast(heldObj.transform.position, mainCamera.transform.forward, out hit, 0.4f))
            {
                if (hit.collider.name == "Ground")
                {
                    // Add animation for the held object
                    StartCoroutine(MoveHeldObject(heldObj, 0.3f,
                        hit.point));
                }
            }
        }

        else
        {
            GameObject cube = heldObj.transform.Find("Cube(Clone)")?.gameObject;
            if (cube != null)
            {
                cube.transform.SetParent(null); // Unparent the cube from the held object
                cube.layer = LayerMask.NameToLayer("Default");
                Rigidbody cubeRigidbody = cube.GetComponent<Rigidbody>();
                if (cubeRigidbody == null)
                {
                    cubeRigidbody = cube.AddComponent<Rigidbody>(); // Add a Rigidbody if none exists
                }

                cubeRigidbody.isKinematic = false; // Enable the Rigidbody to allow the cube to fall
                cubeRigidbody.useGravity = true; // Enable gravity for the cube to fall down
                shovelEmpty = true;
            }
        }
    }

    private IEnumerator MoveHeldObject(GameObject obj, float duration, Vector3 hit)
    {
        FPC.cameraCanMove = false;
        canPickup = false;
        Vector3 orgPos = obj.transform.position;
        Vector3 targetPos = hit;

        // Move forward
        float elapsedTime = 0;
        while (elapsedTime < duration / 2)
        {
            obj.transform.position = Vector3.Lerp(orgPos, targetPos, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heldObj.layer = LayerMask.NameToLayer("Default");


        // Wait for 1 second
        yield return new WaitForSeconds(0.6f);
        GameObject instantiatedPrefab = Instantiate(sand);
        Vector3 newScale = new Vector3(9.5f, 11.5f, 7.5f);
        instantiatedPrefab.transform.localScale = newScale;
        instantiatedPrefab.transform.SetParent(heldObj.transform);
        Vector3 newPos = new Vector3(0, -0.45f, 0.06f); // Adjust the local position here
        instantiatedPrefab.transform.localPosition = newPos;
        instantiatedPrefab.transform.localRotation =
            Quaternion.Euler(-8.3f, 0f, 180f); // Adjust the local rotation here
        MeshCollider collider = instantiatedPrefab.AddComponent<MeshCollider>();
        collider.convex = true;
        instantiatedPrefab.layer = LayerMask.NameToLayer("FirstPerson");
        instantiatedPrefab.tag = "Sand"; // Set the tag to "Sand"


        // Move back
        elapsedTime = 0;
        while (elapsedTime < duration / 2)
        {
            obj.transform.position = Vector3.Lerp(targetPos, orgPos, (elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heldObj.layer = LayerMask.NameToLayer("FirstPerson");

        // Ensure the object returns to its original position
        obj.transform.position = orgPos;
        FPC.cameraCanMove = true;
        canPickup = true;
        shovelEmpty = false;
    }


    private void UseBucket()
    {
        Collider[] colliders = heldObj.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, pickupRange))
        {
            if (hit.collider.gameObject.tag == "On")
            {
                SC.spawnCastlePart(hit.collider.gameObject.name);
                Destroy(hit.collider.gameObject);
                foreach (Transform child in heldObj.transform)
                {
                    Vector3 newPos =
                        new Vector3(0, 0,
                            child.gameObject.transform.localPosition.z - 0.2f); // Adjust the local position here
                    child.gameObject.transform.localPosition = newPos;
                }

                if (SC.bottomBuilt)
                {
                    bucketFull = false;
                    BD.amount = 0;
                    foreach (Transform child in heldObj.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    foreach (Collider collider in colliders)
                    {
                        collider.enabled = true;
                    }

                    ThrowObject();
                }
            }
            else if (hit.collider.gameObject.tag == "Tower")
            {
                SC.spawnTowerPart(hit.collider.gameObject.name);
                foreach (Transform child in heldObj.transform)
                {
                    Vector3 newPos =
                        new Vector3(0, 0,
                            child.gameObject.transform.localPosition.z - 0.2f); // Adjust the local position here
                    child.gameObject.transform.localPosition = newPos;
                }

                if (SC.towerBuilt)
                {
                    bucketFull = false;
                    BD.amount = 0;
                    foreach (Transform child in heldObj.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    foreach (Collider collider in colliders)
                    {
                        collider.enabled = true;
                    }

                    ThrowObject();
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
            heldObjRB.constraints = RigidbodyConstraints.FreezeAll;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
            heldObjC = pickObj.GetComponent<Collider>();
            pickObj.layer = LayerMask.NameToLayer("FirstPerson");
            oPos = heldObj.transform.position;
            oScale = heldObj.transform.localScale;
            oRotation = heldObj.transform.rotation;

            foreach (Transform child in heldObj.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("FirstPerson");
            }


            if (heldObj.gameObject.name == "Shovel")
            {
                Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * 0.58f +
                    mainCamera.transform.right * 0.3f - mainCamera.transform.up * 0.2f;
                heldObj.transform.position = newPosition;
                Vector3 newRotation = new Vector3(-70, 0, 0);
                heldObj.transform.localRotation = Quaternion.Euler(newRotation);
            }
            else if (heldObj.gameObject.name == "Bucket" && BD.amount > 3)
            {
                SC.spawn[0] = true;
                Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * 0.65f -
                                      new Vector3(0, 0.5f, 0);
                heldObj.transform.position = newPosition;
                bucketFull = true;
            }
            else
            {
                InspectObject(oPos, oScale, oRotation);
            }
        }
    }


    private void InspectObject(Vector3 oPos, Vector3 oScale, Quaternion oRotation)
    {
        FPC.cameraCanMove = false;
        canPickup = false;
        Vector3 newScale = heldObj.transform.localScale * 0.5f;
        heldObj.transform.localScale = newScale;
        Vector3 newPosition = mainCamera.transform.position + mainCamera.transform.forward * 0.6f;
        heldObj.transform.position = newPosition;
        initialPosition = heldObj.transform.position;
        Vector3 newRotation = new Vector3(0, 0, 0);
        heldObj.transform.rotation = Quaternion.Euler(newRotation);
        heldObjRB.constraints = RigidbodyConstraints.None;
        StartCoroutine(SpinForSeconds(5f, oPos, oScale, oRotation));
    }

    private IEnumerator SpinForSeconds(float duration, Vector3 oPos, Vector3 oScale, Quaternion oRotation)
    {
        float startRotationY = heldObj.transform.rotation.eulerAngles.y;
        float endRotationY = startRotationY + 360f; // 360 degrees rotation

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float yRotation = Mathf.Lerp(startRotationY, endRotationY, t);
            heldObj.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            heldObj.transform.position = initialPosition; // Reset position
            yield return null;
        }

        heldObjRB.constraints = RigidbodyConstraints.FreezeAll;
        heldObj.transform.position = oPos;
        heldObj.transform.localScale = oScale;
        heldObj.transform.rotation = oRotation;
        heldObj.layer = originaLayer;
        heldObjRB.useGravity = true;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObj = null;
        FPC.cameraCanMove = true;
        canPickup = true;
    }


    private void ThrowObject()
    {
        heldObj.layer = originaLayer;
        heldObjRB.useGravity = true;
        heldObjRB.transform.parent = null;
        heldObj.transform.position = oPos;
        heldObj.transform.localScale = oScale;
        heldObj.transform.rotation = oRotation;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObj = null;
    }
}