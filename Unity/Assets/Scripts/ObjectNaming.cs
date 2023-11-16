using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNaming : MonoBehaviour
{
    public List<GameObject> namingObjects;
    public GameObject camera;

    //Used to initiate the next object question/inpsection
    [HideInInspector] public bool answerGraded;
    private FirstPersonController FPC;
    private GrabObject GO;
    private CountdownManager CM;
    [HideInInspector] public int index = 0;
    [HideInInspector] public int size;

    // Start is called before the first frame update
    void Start()
    {
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        CM = GameObject.Find("CountdownManager").GetComponent<CountdownManager>();
        size = namingObjects.Count;
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
            InspectObject(namingObjects[index]);
        }
    }

    private void InspectObject(GameObject heldObj)
    {
        index++;
        GO.canPickup = false;
        Vector3 oPos = heldObj.transform.position;
        Quaternion oRot = heldObj.transform.rotation;
        Vector3 oScale = heldObj.transform.localScale;
        //Spins the object around for a couple of seconds
        heldObj.layer = LayerMask.NameToLayer("FirstPerson");
        Rigidbody heldObjRB = heldObj.GetComponent<Rigidbody>();
        FPC.cameraCanMove = false;
        Vector3 newScale = heldObj.transform.localScale * 0.5f;
        heldObj.transform.localScale = newScale;
        Vector3 newPosition = camera.transform.position + camera.transform.forward * 0.6f;
        heldObj.transform.position = newPosition;
        Vector3 newRotation = new Vector3(0, 0, 0);
        heldObj.transform.rotation = Quaternion.Euler(newRotation);
        heldObjRB.constraints = RigidbodyConstraints.None;
        StartCoroutine(SpinForSeconds(5f, heldObj, heldObjRB, oPos, oRot, oScale));
    }

//Used for inspect object spinning
    private IEnumerator SpinForSeconds(float duration, GameObject heldObj, Rigidbody heldObjRB, Vector3 oPos,
        Quaternion oRot, Vector3 oScale)
    {
        float startRotationY = heldObj.transform.rotation.eulerAngles.y;
        float endRotationY = startRotationY + 360f; // 360 degrees rotation

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            float yRotation = Mathf.Lerp(startRotationY, endRotationY, t);
            heldObj.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }

        heldObj.transform.position = oPos;
        heldObj.transform.rotation = oRot;
        heldObj.transform.localScale = oScale;
        heldObjRB.useGravity = true;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObj.layer = LayerMask.NameToLayer("Default");
        CM.StartTimer(5);
    }
}