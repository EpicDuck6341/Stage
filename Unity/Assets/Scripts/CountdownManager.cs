using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Unity.VisualScripting; // Remove if not needed

public class CountdownManager : MonoBehaviour
{
    public GameObject timer;
    [HideInInspector] public bool isStarted;
    private FirstPersonController FPC;
    private GrabObject GO;
    [HideInInspector] public bool timerActive;

    private AudioPlayer AP;
    private AudioSource aud;


    private void Start()
    {
        FPC = GameObject.Find("FirstPersonController").GetComponent<FirstPersonController>();
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        AP = GameObject.Find("AudioManager").GetComponent<AudioPlayer>();
        aud = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        timer.SetActive(false);
    }

    public void StartTimer(float duration)
    {
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
        AP.questionAsked = false;
    }
    
}

