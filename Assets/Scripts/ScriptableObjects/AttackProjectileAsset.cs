using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[attack name]", menuName = "Boss Attack")]
public class AttackProjectileAsset : ScriptableObject
{
    public AnimatorOverrideController animations;
    public AttackDirections spawnDirections;
    public HitboxProperties hitboxProperties;
    
}

[Serializable]
public struct AttackDirections
{
    [SerializeField] public bool top;
    [SerializeField] public bool sides;

    public AttackDirections(bool top, bool sides)
    {
        this.top = top;
        this.sides = sides;
    }
}

[Serializable]
public struct HitboxProperties
{
    [SerializeField] public int damage;
    [SerializeField] public Vector2 size;
    [SerializeField] public Vector2 offset;
}
