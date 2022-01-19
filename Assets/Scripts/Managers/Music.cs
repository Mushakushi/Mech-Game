using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    /// <summary>
    /// AudioSource component in gameObject
    /// </summary>
    [SerializeField] private AudioSource source; 

    /// <summary>
    /// Optional one-shot clip at start of loop
    /// </summary>
    [SerializeField] private AudioClip intro;

    /// <summary>
    /// Clip to be looped 
    /// </summary>
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
