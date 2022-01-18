using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{

    [SerializeField] private AudioSource source; 
    [SerializeField] private AudioClip intro;
    [SerializeField] private AudioClip loop; 

    /// <summary>
    /// Plays looping audio with optional intro
    /// </summary>
    void Start()
    {
        source.loop = true;
        source.clip = loop;
        if (!intro) intro = loop;
        source.PlayOneShot(intro);
        source.PlayScheduled(AudioSettings.dspTime + intro.length); 
    }
}
