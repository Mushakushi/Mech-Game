using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterData : ICharacterData
{
    [SerializeField] public string name { get; set; }
    [SerializeField] public float maxHealth { get; }
    [SerializeField] public float health { get; set; }
    [SerializeField] public float attack { get; set; }
    [SerializeField] public float resistance { get; set; }
    [SerializeField] public Phase[] activePhases { get; set; }

    /// <param name="name">Character's name</param>
    /// <param name="maxHealth">Maximum health</param>
    /// <param name="damage">Damage given to other Hurtboxes</param>
    /// <param name="resistance">Damage modifier against Hitboxes</param>
    /// <param name="activePhases">Phase(s) wherein this character is active</param>
    public CharacterData(string name, float maxHealth, float damage, float resistance, Phase[] activePhases)
    {
        this.name = name;
        this.maxHealth = maxHealth;
        this.health = maxHealth; 
        this.attack = damage;
        this.resistance = resistance;
        this.activePhases = activePhases; 
    }
}
