using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // public AudioClip ballGrab;
    // public AudioClip shovelGrab;
    // public AudioClip bucketGrab;
    // public AudioClip throwObj;
    // public AudioClip praise;
    // public AudioClip ballQuestion;
    // public AudioClip shovelQuestion;
    // public AudioClip bucketQuestion;
    //
    // private GrabObject GO;
    // private bool objectThrown = false;
    // private bool praiseSaid = false;
    // private string chosenObj;
    //
    // [HideInInspector]
    // public bool questionAsked;
    //
    // private AudioSource aud;
    //
    // void Start()
    // {
    //     GO = GameObject.Find("FirstPersonController").GetComponent<GrabObject>();
    //     aud = GetComponent<AudioSource>();
    //     int rand = Random.Range(0, 2);
    //     if (rand == 0)
    //     {
    //         aud.clip = shovelGrab;
    //         chosenObj = "shovel";
    //     }
    //     else if (rand == 1)
    //     {
    //         aud.clip = bucketGrab;
    //         chosenObj = "bucket";
    //     }
    //     else
    //     {
    //         aud.clip = ballGrab;
    //         chosenObj = "ball";
    //     }
    //
    //     aud.Play();
    // }
    //
    // void Update()
    // {
    //     if (GO.heldObj != null && !objectThrown)
    //     {
    //         objectThrown = true;
    //         aud.clip = throwObj;
    //         aud.Play();
    //     }
    //
    //     if (!GO.canPickup && !praiseSaid)
    //     {
    //         praiseSaid = true;
    //         aud.clip = praise;
    //         aud.Play();
    //     }
    //     
    //     if (!aud.isPlaying && !GO.canPickup && !questionAsked )
    //     {
    //         questionAsked = true;
    //         PlayNextClip();
    //     }
    // }
    //
    // void PlayNextClip()
    // {
    //     switch (chosenObj)
    //     {
    //         case "shovel":
    //             aud.clip = shovelQuestion;
    //             break;
    //         case "bucket":
    //             aud.clip = bucketQuestion;
    //             break;
    //         case "ball":
    //             aud.clip = ballQuestion;
    //             break;
    //         default:
    //             // Handle the default case if needed
    //             break;
    //     }
    //
    //     aud.Play();
    // }

}