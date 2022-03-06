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

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays looping audio with optional intro
    /// </summary>
    public static void PlayBGM(BGM bgm)
    {
        // Stop current audio
        source.Stop();

        // Play the BGM
        Play(bgm.intro); 

        // Schedule loop to be played indefinitely
        source.loop = true;
        source.clip = bgm.loop;
        source.PlayScheduled(AudioSettings.dspTime + bgm.intro.length);
    }

    /// <summary>
    /// Plays audio
    /// </summary>
    /// <remarks>(e.g. a sound effect)</remarks>
    public static void Play(AudioClip audio) => source.PlayOneShot(audio); 

    // TODO - add low pass filter option when paused 
}
