using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements; // Remove if not needed

public class CountdownManager : MonoBehaviour
{
    public GameObject timer;
    [HideInInspector] public bool isStarted;
    private ImageChanger IC;


    private void Start()
    {
        IC = GameObject.Find("Image").GetComponent<ImageChanger>();
        timer.SetActive(false);
    }

    public void StartTimer(float duration)
    {
        IC.setImage(IC.sprite[0]);
        timer.SetActive(true);
        isStarted = true;
        timer.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);

        StartCoroutine(EndTimer(duration));
    }

    IEnumerator EndTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        isStarted = false;
        timer.SetActive(false);
        IC.setImage(IC.sprite[1]);
    }
    
}

