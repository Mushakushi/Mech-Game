using System.Collections.Generic;
using UnityEngine;

// Not currently in use 

[System.Serializable]
public struct BossData 
{
    public int maxHealthBars;
    public int healthBars;
    public List<float> accumulatedWeights;
    public float accumulatedWeightSum;
    public AudioClip hurtClip;
    public AudioClip knockClip;
    public List<AudioClip> dialogueClips;


    /// <param name="maxHealthBars">Amount of times health bar must be depleted to be defeated</param>
    /// <param name="specialWeights">List of weights of the boss's special attacks.</param>
    public BossData(int maxHealthBars, List<float> specialWeights)
    {
        this.maxHealthBars = maxHealthBars;
        this.healthBars = maxHealthBars;

        // TODO - standardize naming these so we can add variables 
        hurtClip = FileUtility.LoadFile<AudioClip>($"Audio/Voicelines/Lobstobotomizer/snd_ugh");
        knockClip = FileUtility.LoadFile<AudioClip>($"Audio/Voicelines/Lobstobotomizer/snd_khan");
        dialogueClips = new List<AudioClip>() { hurtClip, knockClip };

        // these are needed to stop the thing from yelling at me. oh well
        accumulatedWeightSum = 0;
        accumulatedWeights = null;
        Accumulate(specialWeights);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="weights"></param>
    private void Accumulate(List<float> weights)
    {
        List<float> result = new List<float>();
        float accumulatedWeight = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            accumulatedWeight += weights[i];
            result.Insert(i, accumulatedWeight);
        }

        accumulatedWeightSum = accumulatedWeight;
        accumulatedWeights = result;
    }
}
