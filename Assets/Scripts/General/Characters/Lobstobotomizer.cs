using System.Collections.Generic;
using UnityEngine;

public class Lobstobotomizer : Boss
{
    protected override BossData SetBossData()
    {
        return new BossData(
            "Lobstobotomizer",
            10f,
            1,
            1f,
            Phase.Player,
            3,
            (AudioClip) (Resources.Load($"Audio/Voicelines/Lobstobotomizer/hurt1")),
            new List<float>{ 33f, 33f, 33f }
            ); 
    }
}
