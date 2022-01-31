using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
public abstract class Character : MonoBehaviour, IPhaseController
{
    [Header("Stats")]
    /// <summary>
    /// Name of this Character
    /// </summary>
    [SerializeField] public string characterName;

    /// <summary>
    /// Damage required to defeat Character
    /// </summary>
    [SerializeField] protected float maxHealth; 

    /// <summary>
    /// Current health of Character
    /// </summary>
    public float health;

    /// <summary>
    /// Damage this Character deals
    /// </summary>
    [SerializeField] protected float damage;

    /// <summary>
    /// Multiplier that reduces damage taken 
    /// </summary>
    /// Can't we just increase health?
    [SerializeField] protected float resistance;
    
    /// <summary>
    /// Phase(s) this Character belongs to
    /// </summary>
    [HideInInspector] protected IList<Phase> activePhases { get; set; }

    /// <summary>
    /// Tells PhaseManager whether the current phase is equal to one in activePhase
    /// </summary>
    [HideInInspector] public Phase activePhase => this.GetPhaseFromCollection(activePhases);

    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }

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

    /// <summary>
    /// Is a hitbox colliding with this hurtbox?
    /// </summary>
    public bool isHit;

    [Space()]
    public bool isShaking = false;
    public Vector3 shakePos;
    public float shakingRange;
    public bool returnToIdle = false;

    public void OnStart()
    {
        hurtbox = GetComponent<BoxCollider2D>();
        EnableHurtbox(); 

        // GetComponentInChildren (wierdly) searches parent, search through array instead
        foreach (BoxCollider2D c in GetComponentsInChildren<BoxCollider2D>())
        {
            if (c != hurtbox)
            {
                hitbox = c;
                DisableHitbox();
                break;
            }
        }
        if (!hitbox) Debug.LogError("Script requires hitbox in child!"); 

        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = Resources.Load($"Animation/Animators/{GetType().Name}") as RuntimeAnimatorController;
        
        activePhases = InitializeCharacter();
    }

    /// <summary>
    /// Child initialization event, Start should not be used as it may superceed the correct initialization order
    /// </summary>
    public abstract IList<Phase> InitializeCharacter();

    /// <summary>
    /// Event that happens when a Hitbox enters this Character's Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entrance</param>
    public virtual void OnHitboxEnter(float damage)
    {
        animator.SetTrigger("GetHit");

        /*if (isShaking)
        {
            animator.applyRootMotion = true;
            //transform.position = MoveUtil.GetPosFromPos(shakePos, Random.Range(-shakingRange, shakingRange), Random.Range(-shakingRange, shakingRange));
        }
        else
        {
           // if (MoveUtil.PosWithinRange(transform.position, shakePos, shakingRange) && !transform.position.Equals(shakePos))
            {
                transform.position = shakePos;
                animator.applyRootMotion = false;
            }
        }*/

        health -= damage * (1 / resistance); // we could just adjust hp if player doesn't have a way to level up stats
        isHit = true;

        if (health <= 0)
        {
            // put anything that should happen here
            OnHealthDeplete(); 
        }
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
    /// What should happen when Character's health depletes to 0?
    /// </summary>
    protected abstract void OnHealthDeplete(); 

    #region PHASE BEHAVIOR
    /// <summary>
    /// Describes what should happen when Character's phase is entered
    /// </summary>
    public void OnPhaseEnter()
    {
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
        DisableHitbox();
        EnableHurtbox();
        PhaseExitBehavior(); 
    }
    /// <summary>
    /// Describes what can happen when Character's phase is exited 
    /// </summary>
    protected abstract void PhaseExitBehavior();
    #endregion
}
