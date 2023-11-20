using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNoises : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource aud;
    
    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.loop = false;
    }

    public void playAudio(int index)
    {
        aud.clip = clips[index];
        aud.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
