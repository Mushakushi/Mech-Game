using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BossData
{
    [SerializeField] public string name;
    [SerializeField] public readonly float maxHealth;
    [SerializeField] public float damage;
    [SerializeField] public float resistance;
    [SerializeField] public Phase activePhase;
    [SerializeField] public int maxHealthBars;

    /// <summary>
    /// Initializes boss data
    /// </summary>
    /// <param name="name">Boss's name</param>
    /// <param name="maxHealth">Maximum health</param>
    /// <param name="damage">Damage given to other Hurtboxes</param>
    /// <param name="resistance">Damage modifier against Hitboxes</param>
    /// <param name="activePhase">Phase (or PhaseGroup) wherein this boss is active</param>
    /// <param name="maxHealthBars">Amount of times health bar must be depleted to be defeated</param>
    public BossData(string name, float maxHealth, float damage, float resistance, 
        Phase activePhase, int maxHealthBars)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.resistance = resistance;
        this.activePhase = activePhase;
        this.maxHealthBars = maxHealthBars;
    }
}
