using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected string characterName;
    [SerializeField] protected float maxHealth; 
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float resistance;

    [Header("UI and Animation")]
    [SerializeField] private Slider healthSlider; 

    [Space()]
    public Animator animator;

    [Space()]
    [SerializeField] protected BoxCollider2D hitbox;
    [SerializeField] protected BoxCollider2D hurtbox;
    
    [Space()]
    public bool isHit;
    public bool isShaking = false;
    public Vector3 shakePos;
    public float shakingRange;
    public bool returnToIdle = false;

    /// <summary>
    /// Moves character slightly around current position.
    /// </summary>
    /// <returns>true when shake has completed (time has elapsed), and false if otherwise.</returns>
    public void TryShake()
    {
        if (isShaking)
        {
            animator.applyRootMotion = true;
            transform.position = MoveUtil.GetPosFromPos(shakePos, Random.Range(-shakingRange, shakingRange), Random.Range(-shakingRange, shakingRange));
        }
        else
        {
            if (MoveUtil.PosWithinRange(transform.position, shakePos, shakingRange) && !transform.position.Equals(shakePos))
            {
                transform.position = shakePos;
                animator.applyRootMotion = false;
            }
        }
    }

    /// <summary>
    /// Event that happens when Hitbox enters Character Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entrance</param>
    public void OnHitboxEnter(float damage)
    {
        print("ow");
        animator.SetTrigger("GetHit");
        health -= damage * (1 / resistance);
        healthSlider.value = health / maxHealth;
        isHit = true; 
    }

    /// <summary>
    /// Event that happens when Hitbox exits Character Hurtbox
    /// </summary>
    public void OnHitboxExit()
    {
        isHit = false; 
    }
}
