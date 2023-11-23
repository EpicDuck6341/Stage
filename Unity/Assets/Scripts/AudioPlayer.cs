using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
   public List<AudioClip> clips;

   [HideInInspector] public AudioSource auds;

   private void Start()
   {
      auds = gameObject.GetComponent<AudioSource>();
   }

   public void playAudio(AudioClip audio)
   {
      auds.clip = audio;
      auds.Play();
   }
}