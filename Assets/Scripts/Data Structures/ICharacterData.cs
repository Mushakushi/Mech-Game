using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Required data for Character class
/// </summary>
/// not neccessary unless there are other characters
/// that are not bosses
public interface ICharacterData
{
    /// <summary>
    /// Character's name
    /// </summary>
    string name { get; set; }

    /// <summary>
    /// Character's max health
    /// </summary>
    float maxHealth { get; }

    /// <summary>
    /// Character's current health
    /// </summary>
    float health { get; set; }

    /// <summary>
    /// Character's attack (damage given)
    /// </summary>
    float attack { get; set; }

    /// <summary>
    /// Character's resitance multiplier 
    /// </summary>
    float resistance { get; set; }

    /// <summary>
    /// Phase(s) wherein this Character is active
    /// </summary>
    Phase[] activePhases { get; set; }
}
