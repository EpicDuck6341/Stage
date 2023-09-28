using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip ballGrab;
    public AudioClip shovelGrab;
    public AudioClip bucketGrab;
    public AudioClip throwObj;

    private GrabObject GO;
    private bool objectThrown = false; 

    private AudioSource aud;

    // Start is called before the first frame update
    void Start()
    {
        GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
        aud = GetComponent<AudioSource>();
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            aud.clip = shovelGrab;
        }
        else if (rand == 1)
        {
            aud.clip = bucketGrab;
        }
        else
        {
            aud.clip = ballGrab;
        }

        aud.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (GO.heldObj != null && !objectThrown)
        {
            objectThrown = true;
            aud.clip = throwObj;
            aud.Play();
        }
    }
}