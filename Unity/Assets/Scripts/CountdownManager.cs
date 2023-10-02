using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using Unity.VisualScripting; // Remove if not needed

public class CountdownManager : MonoBehaviour
{
    public List<GameObject> goalColliders; // Store references to goal colliders
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

    void StartTimer(float duration)
    {
        timerActive = true;
        FPC.cameraCanMove = false;
        isStarted = true;
        timer.SetActive(true);
        timer.transform.Find("RadialProgressBar").GetComponent<CircularProgressBar>().ActivateCountdown(duration);

        StartCoroutine(EndTimer(duration));
    }

    IEnumerator EndTimer(float delay)
    {
        yield return new WaitForSeconds(delay);

        isStarted = false;
        timer.SetActive(false);
        FPC.cameraCanMove = true;
        GO.canPickup = true;
        timerActive = false;
        AP.questionAsked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!aud.isPlaying)
        {
            foreach (GameObject goalCollider in goalColliders)
            {
                EdgeCollision EC = goalCollider.GetComponent<EdgeCollision>();
                if (EC != null && EC.isCollision && !isStarted && !aud.isPlaying)
                {
                    StartTimer(8);
                    EC.isCollision = false;
                }
                else
                {
                    EC.isCollision = false;
                }
            }
        }
    }
}

