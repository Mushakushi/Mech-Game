using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BossData
{
    [SerializeField] public string name;
    [SerializeField] public readonly float maxHealth;
    [SerializeField] public float damage;
    [SerializeField] public float resistance;
    [SerializeField] public Phase activePhase;
    [SerializeField] public int maxHealthBars;
    [SerializeField] public List<float> accumulatedWeights;
    [SerializeField] public float accumulatedWeightSum;
    [SerializeField] public AudioClip hurt;

    /// <summary>
    /// Initializes boss data
    /// </summary>
    /// <param name="name">Boss's name</param>
    /// <param name="maxHealth">Maximum health</param>
    /// <param name="damage">Damage given to other Hurtboxes</param>
    /// <param name="resistance">Damage modifier against Hitboxes</param>
    /// <param name="activePhase">Phase (or PhaseGroup) wherein this boss is active</param>
    /// <param name="maxHealthBars">Amount of times health bar must be depleted to be defeated</param>
    /// <param name="specialWeights">List of weights of the boss's special attacks.</param>
    public BossData(string name, float maxHealth, float damage, float resistance, 
        Phase activePhase, int maxHealthBars, AudioClip hurt, List<float> specialWeights)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.resistance = resistance;
        this.activePhase = activePhase;
        this.maxHealthBars = maxHealthBars;
        this.hurt = hurt;

        // these are needed to stop the thing from yelling at me. oh well
        accumulatedWeightSum = 0;
        accumulatedWeights = null;
        Accumulate(specialWeights);
    }

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
