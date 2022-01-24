using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BossData
{
    [SerializeField] public string name;
    [SerializeField] public readonly float maxHealth;
    [SerializeField] public float damage;
    [SerializeField] public float resistance;
    [SerializeField] public Phase targetPhase;
    [SerializeField] public int healthBars;
    [SerializeField] public List<int> SpecialAttackWeights { get; set; }

    /// <summary>
    /// Initializes boss data
    /// </summary>
    /// <param name="name">Boss's name</param>
    /// <param name="maxHealth">Maximum health</param>
    /// <param name="damage">Damage given to other Hurtboxes</param>
    /// <param name="resistance">Damage modifier against Hitboxes</param>
    /// <param name="targetPhase">Phase that follows the boss phase</param>
    /// <param name="healthBars">Amount of times health bar must be depleted to be defeated</param>
    /// <param name="SpecialAttackWeights">List of weights used to determine attack</param>
    public BossData(string name, float maxHealth, float damage, float resistance, 
        Phase targetPhase, int healthBars, List<int> SpecialAttackWeights)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.resistance = resistance;
        this.targetPhase = targetPhase;
        this.healthBars = healthBars;
        this.SpecialAttackWeights = SpecialAttackWeights; 
    }
}
