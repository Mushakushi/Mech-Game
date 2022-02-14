using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "[attack name]", menuName ="Create Attack...")]
public class AttackProjectileAsset : ScriptableObject
{
    public AnimatorOverrideController projectileAnimations;
    public AttackDirections attackSpawnDirections;
    public int damage;
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

public struct HitboxProperties
{
    [SerializeField] public float sizeX;
    [SerializeField] public float sizeY;

    [SerializeField] public float offsetX;
    [SerializeField] public float offsetY;
}
