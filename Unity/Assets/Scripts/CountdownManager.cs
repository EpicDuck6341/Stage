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
    private FirstPersonController FPC;
    private GrabObject GO;
    [HideInInspector] public bool timerActive;
    
    private AudioSource aud;
    private ImageChanger IC;


    private void Start()
    {
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        IC = GameObject.Find("Image").GetComponent<ImageChanger>();
        aud = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        timer.SetActive(false);
    }

    public void StartTimer(float duration)
    {
        IC.setImage(IC.sprite[0]);
        timer.SetActive(true);
        isStarted = true;
        timerActive = true;
        timer.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);

        StartCoroutine(EndTimer(duration));
    }

    IEnumerator EndTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        isStarted = false;
        timer.SetActive(false);
        timerActive = false;
        IC.setImage(IC.sprite[1]);
    }
    
}

