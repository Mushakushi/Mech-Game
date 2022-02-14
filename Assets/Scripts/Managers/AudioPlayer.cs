using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    /// <summary>
    /// AudioSource component in gameObject
    /// </summary>
    [SerializeField] private static AudioSource source; 

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
        source = GetComponent<AudioSource>(); 
        PlayBGM(loop, intro); 
    }

    /// <summary>
    /// Plays looping audio with optional intro
    /// </summary>
    public static void PlayBGM(AudioClip loop, AudioClip intro = null)
    {
        // Stop current audio
        source.Stop(); 

        // If there is no intro, use the loop as one
        if (!intro) intro = loop;

        // Play intro once
        source.PlayOneShot(intro);

        // Schedule loop to be played indefinitely
        source.loop = true;
        source.clip = loop;
        source.PlayScheduled(AudioSettings.dspTime + intro.length);
    }

    /// <summary>
    /// One parameter PlayBGM()
    /// </summary>
    public static void PlayBGM(AudioClip loop) => PlayBGM(loop, null);

    /// <summary>
    /// Plays audio one shoot
    /// </summary>
    public static void Play(AudioClip audio) => source.PlayOneShot(audio); 
}