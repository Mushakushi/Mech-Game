using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public string characterName;
    [SerializeField] protected float maxHealth; 
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float resistance;

    [Header("UI and Animation")]
    /// <summary> 
    /// First animator attached to any child object
    /// </summary>
    public Animator animator;

    [Space()]
    /// <summary>
    /// First Box2D Hitbox attached to any child object (should be hitbox)
    /// </summary>
    [SerializeField] protected BoxCollider2D hitbox;

    /// <summary>
    /// Box2D Hurtbox attached to this object
    /// </summary>
    [SerializeField] protected BoxCollider2D hurtbox;
    
    [Space()]
    public bool isHit;
    public bool isShaking = false;
    public Vector3 shakePos;
    public float shakingRange;
    public bool returnToIdle = false;

    private void Start()
    {
        hurtbox = GetComponent<BoxCollider2D>();
        // GetComponentInChildren (wierdly) searches parent, search through array instead
        foreach (BoxCollider2D c in GetComponentsInChildren<BoxCollider2D>())
            if (c.gameObject.transform.parent)
                hitbox = c; 

        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = Resources.Load($"Animation/Animators/{GetType().Name}") as RuntimeAnimatorController;

        DisableHitbox();
        EnableHurtbox(); 
        OnStart();
    }

    /*
    /// <summary>
    /// Moves character slightly around current position.
    /// </summary>
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
    }*/
    // Note: Idk how this works and it's messing up the animations so I'll comment it out for now...

    /// <summary>
    /// Event that happens when a Hitbox enters this Character's Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entrance</param>
    public virtual void OnHitboxEnter(float damage)
    {
        animator.SetTrigger("GetHit");
        health -= damage * (1 / resistance);
        isHit = true; 
    }

    /// <summary>
    /// Event that happens when a Hitbox exits this Character's Hurtbox
    /// </summary>
    public virtual void OnHitboxExit()
    {
        isHit = false; 
    }

    /// <summary>
    /// Enables hitbox
    /// </summary>
    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    /// <summary>
    /// Disables hitbox
    /// </summary>
    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    /// <summary>
    /// Enables hurtbox
    /// </summary>
    public void EnableHurtbox()
    { 
        hurtbox.enabled = true; 
    }


    /// <summary>
    /// Disables hurtbox
    /// </summary>
    public void DisableHurtbox()
    {
        hurtbox.enabled = false; 
    }

    /// <summary>
    /// Child initialization event, Start should not be used as it hides start in this class
    /// </summary>
    public abstract void OnStart();
}
