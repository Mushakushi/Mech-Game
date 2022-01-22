using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
public abstract class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public string characterName;
    [SerializeField] protected float maxHealth; 
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float resistance;

    /// <summary>
    /// Character's active phase, may have restricted functionality in other phases
    /// </summary>
    [SerializeField] Phase activePhase;

    /// <summary>
    /// Phase that directly follows this phase
    /// </summary>
    [SerializeField] protected Phase targetPhase; 

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

    public void Start()
    {
        hurtbox = GetComponent<BoxCollider2D>();
        EnableHurtbox(); 

        // GetComponentInChildren (wierdly) searches parent, search through array instead
        foreach (BoxCollider2D c in GetComponentsInChildren<BoxCollider2D>())
        {
            if (c.gameObject.transform.parent)
            {
                hitbox = c;
                //DisableHitbox(); 
            }
        }
        if (!hitbox) Debug.LogError("Script requires hitbox in child!"); 

        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = Resources.Load($"Animation/Animators/{GetType().Name}") as RuntimeAnimatorController;
        
        OnStart();
    }

    /// <summary>
    /// Child initialization event, Start should not be used as it hides start in this class
    /// </summary>
    public abstract void OnStart();

    // shouldnt be too hard to implement with statemachines i think
    // really adds to game polish imo
    #region keep this?  
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
    #endregion

    /// <summary>
    /// Event that happens when a Hitbox enters this Character's Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entrance</param>
    public virtual void OnHitboxEnter(float damage)
    {
        animator.SetTrigger("GetHit");
        print("ow"); 
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

    #region ANIMATOR FUNCTIONS
    /// <summary>
    /// Describes what should happen when Character's phase is entered
    /// </summary>
    public void OnPhaseEnter()
    {
        animator.SetTrigger("EnterPhase"); 
        PhaseEnterBehavior(); 
    }
    /// <summary>
    /// Describes what can happen when Character's phase is entered 
    /// </summary>
    protected abstract void PhaseEnterBehavior(); 

    /// <summary>
    /// Describes what should happen when Character's phase is running
    /// </summary>
    public void OnPhaseUpdate()
    {
        PhaseUpdateBehavior(); 
    }
    /// <summary>
    /// Describes what can happen when Character's phase is running
    /// </summary>
    protected abstract void PhaseUpdateBehavior(); 

    /// <summary>
    /// Describes what should happen when Character's phase is exited
    /// </summary>
    public void OnPhaseExit()
    {
        Combat.ChangePhase(targetPhase);
        DisableHitbox();
        EnableHurtbox();
        PhaseExitBehavior(); 
    }
    /// <summary>
    /// Describes what can happen when Character's phase is exited 
    /// </summary>
    protected abstract void PhaseExitBehavior(); 

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
        //hurtbox.enabled = false; 
    }
    #endregion
}
