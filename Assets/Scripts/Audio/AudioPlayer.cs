using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound effects
/// </summary>
public enum SoundEffect { Paused, LowPass }

[RequireComponent(typeof(AudioSource), typeof(AudioLowPassFilter))]
public class AudioPlayer : MonoBehaviour
{
    /// <summary>
    /// AudioSource component in gameObject
    /// </summary>
    private static AudioSource source;

    /// <summary>
    /// LowPassFilter component
    /// </summary>
    private static AudioLowPassFilter eq;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        eq = GetComponent<AudioLowPassFilter>();
        ClearEffects(); 
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

    /// <summary>
    /// Applies sound effect <paramref name="effect"/>
    /// </summary>
    public static void AddEffect(SoundEffect effect)
    {
        switch (effect)
        {
            case SoundEffect.LowPass:
                eq.enabled = true;
                break;
            case SoundEffect.Paused:
                source.Stop();
                break;
            default : throw new System.Exception($"Effect {effect} is not a valid Effect!");
        }
    }

    /// <summary>
    /// Removes sound effect <paramref name="effect"/>
    /// </summary>
    public static void RemoveEffect(SoundEffect effect)
    {
        switch (effect)
        {
            case SoundEffect.LowPass:
                eq.enabled = false;
                break;
            case SoundEffect.Paused:
                if (!source.isPlaying) source.Play();
                else Debug.LogWarning("Plyaing Audio source failed because it is not paused!"); 
                break;
            default: throw new System.Exception($"Effect {effect} is not a valid Effect!");
        }
    }

    /// <summary>
    /// Removes all current sound effects
    /// </summary>
    public static void ClearEffects()
    {
        RemoveEffect(SoundEffect.LowPass);
        RemoveEffect(SoundEffect.Paused); 
    }
}
