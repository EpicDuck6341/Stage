using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ImageChanger : MonoBehaviour
{
    //Used for swapping the images used during assignment 3
    //Image for, talking,grading and when giving the grade
    public List<Sprite> sprite = new List<Sprite>();

    private Image img;
    private FirstPersonController FPC;
    private GrabObject GO;
    private ObjectNaming ON;


    void Start()
    {
        img = gameObject.GetComponent<Image>();
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        ON = GameObject.Find("FirstPersonController").GetComponent<ObjectNaming>();
    }

    public void setImage(Sprite sprite)
    {
        img.sprite = sprite;

// Set alpha to the maximum (1.0f)
        Color imgColor = img.color;
        imgColor.a = 1.0f;
        img.color = imgColor;
    }

    public void clearImage()
    {
        img.sprite = null;

        Color imgColor = img.color;
        imgColor.a = 0;
        img.color = imgColor;
        if (ON.index == ON.size)
        {
            FPC.cameraCanMove = true;
            GO.canPickup = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}