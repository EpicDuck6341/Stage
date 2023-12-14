using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVoiceLines : MonoBehaviour
{
    //0 = Click op de Schep 
    //1 = Schep nog niet gevonden
    //2 = Click op het zand
    //3 = Schep boven de emmer hounder/Emmer vullen
    //4 = Emmer vol/Emmer oppakken
    //5 = Zandkasteel bouwen/Op de groene plaat clicken
    //6 = Zandkasteel versieren/Emmer oppakken
    //7 = Schelpen oppakken
    //8 = Emmer vullen/Schelpen boven de emmer
    //9 = Click op de emmer
    //10 = Click op de goede schelp
    //11 = Vraag over hoe het object heet/NPC stelt vraag in opdracht 3
    //12 = Niet goed verstaan
    //13 = Antwoord op de vraag geven

    public AudioClip[] clips;
    [HideInInspector] public AudioSource aud;
    
    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
        aud.loop = false;
        playAudio(0);
    }

    public void playAudio(int index)
    {
        aud.clip = clips[index];
        aud.Play();
    }
}
