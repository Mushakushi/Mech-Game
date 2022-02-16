using UnityEngine;
using static FileUtility; 

/// <summary>
/// Represents one looping audio 
/// </summary>
[System.Serializable]
public class BGM
{
    /// <summary>
    /// Optional one-shot clip at start of loop
    /// </summary>
    public AudioClip intro;

    /// <summary>
    /// Clip to be looped after intro
    /// </summary>
    public AudioClip loop; 

    /// <param name="intro">Optional one-shot clip at start of loop</param>
    /// <param name="loop">Clip to be looped after intro</param>
    public BGM(AudioClip intro, AudioClip loop)
    {
        this.intro = intro;
        this.loop = loop; 
    }

    /// <param name="audio">Clip to be looped wihout intro</param> // ... intro = loop
    public BGM(AudioClip audio) : this(audio, audio) { }


    /// <param name="introFileName">Intro clip file name at bgmPath</param>
    /// <param name="loopFileName">Loop clip file name at bgmPath</param>
    public BGM(string introFileName, string loopFileName)
        : this(LoadFile<AudioClip>($"{bgmPath}/{introFileName}"), LoadFile<AudioClip>($"{bgmPath}/{loopFileName}"))
    { }

    /// <param name="fileName">Loop clip file name at bgmPath</param>
    public BGM(string fileName) : this(LoadFile<AudioClip>($"{bgmPath}/{fileName}")) { }
}
