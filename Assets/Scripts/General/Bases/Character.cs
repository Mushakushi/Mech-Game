using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(BoxCollider2D))]
public abstract class Character : MonoBehaviour, IPhaseController
{
    [Header("Character Stats")]
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

    [Header("Animation")]
    /// <summary> 
    /// First animator attached to any child object
    /// </summary>
    public Animator animator; 

    [Header("Colliders")]
    /// <summary>
    /// First Box2D Hitbox attached to any child object (should be hitbox)
    /// </summary>
    [SerializeField] protected BoxCollider2D hitbox;

    /// <summary>
    /// Box2D Hurtbox attached to this object
    /// </summary>
    [SerializeField] protected BoxCollider2D hurtbox;

    [Space]
    /// <summary>
    /// Is a hitbox colliding with this hurtbox?
    /// </summary>
    public bool isHit;

    /// <summary>
    /// Coroutine that shakes the character. null if not shaking.
    /// </summary>
    private Coroutine shakeCoroutine;

    public bool returnToIdle;

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
    protected abstract IList<Phase> InitializeCharacter();

    /// <summary>
    /// Event that happens when a Hitbox enters this Character's Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entrance</param>
    public virtual void OnHitboxEnter(float damage)
    {
        animator.SetTrigger("GetHit");

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
        StartCoroutine(DoInvulnFrames(0.05f));
    }

    /// <summary>
    /// Event that happens when this Character's Hitbox enters another Character's Hurtbox
    /// </summary>
    public virtual void OnHurtboxEnter()
    {

    }

    /// <summary>
    /// Event that happens when this Character's Hitbox exits another Character's Hurtbox
    /// </summary>
    public virtual void OnHurtboxExit()
    {
        
    }

    public IEnumerator DoInvulnFrames(float duration)
    {
        DisableHurtbox();
        yield return new WaitForSeconds(duration);
        EnableHurtbox();
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

    #region ANIMATION METHODS

    public void ShakeCharacterStart(float range)
    {
        Vector3 shakePos = transform.position;
        animator.applyRootMotion = true;
        shakeCoroutine = StartCoroutine(ShakeCoroutine(range, shakePos));
    }

    public void ShakeCharacterStop()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            transform.position = new Vector3(0, 0, 0);
            animator.applyRootMotion = false;
        }
    }

    private IEnumerator ShakeCoroutine(float range, Vector3 pos)
    {
        while (true)
        {
            transform.position = new Vector3(pos.x + Random.Range(-range, range), pos.y + Random.Range(-range, range), pos.z);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ShakeControllerStart(float rumbleSpeed)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(rumbleSpeed, rumbleSpeed);
        }
    }

    public void ShakeControllerStop()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
    }

    #endregion

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
