using System.Collections.Generic;
using UnityEngine;

public class Lobstobotomizer : Boss
{
    protected override BossData SetBossData()
    {
        List<AudioClip> dialogueAudio = new List<AudioClip> { (AudioClip)(Resources.Load($"Audio/Voicelines/Lobstobotomizer/snd_ugh")) , (AudioClip)(Resources.Load($"Audio/Voicelines/Lobstobotomizer/snd_khan")) };

        return new BossData(
            "Lobstobotomizer",
            10f,
            1,
            1f,
            Phase.Player,
            3,
            (AudioClip)(Resources.Load($"Audio/Voicelines/Lobstobotomizer/snd_ugh")),
            (AudioClip)(Resources.Load($"Audio/Voicelines/Lobstobotomizer/snd_khan")),
            dialogueAudio,
            new List<float> { 33f, 33f, 33f }
            ) ; 
    }
}
