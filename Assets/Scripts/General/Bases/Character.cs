using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static FileUtility; 

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public abstract class Character : MonoBehaviour, IPhaseController, IHitboxOwner
{
    //[Header("Character Stats")]
    // TODO - There is a convoluted way to serialized properties in unity using custom inspector!

    /// <summary>
    /// Name of this character
    /// </summary>
    [SerializeField] public abstract string characterName { get; }

    /// <summary>
    /// Maximum health
    /// </summary>
    [SerializeField] public abstract float maxHealth { get; }

    /// <summary>
    /// Current health
    /// </summary>
    [SerializeField] public float health; 

    /// <summary>
    /// Damage given to hurtboxes
    /// </summary>
    /// <remarks>Everybody does 1 damage rn</remarks>
    [SerializeField] public const float damage = 1; 

    /// <summary>
    /// Multiplier that reduces damage taken 
    /// </summary>
    [SerializeField] public abstract float resistance { get; }

    /// <summary>
    /// Phase wherein this character is active
    /// </summary>
    [SerializeField] public abstract Phase[] activePhases { get; }

    /// <summary>
    /// Phase(s) this Character belongs to
    /// </summary>
    public Phase activePhase => this.GetPhaseFromCollection(activePhases); 

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
    [SerializeField] protected Hitbox hitbox;

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
        if (hurtbox) EnableHurtbox(); 
        if (!hurtbox) Debug.LogError("Script requires a Box Collider component!");

        hitbox = GetComponentInChildren<Hitbox>();
        if (hitbox) DisableHitbox(); 
        if (!hurtbox) Debug.LogError("Script requires hitbox in child!");

        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = LoadFile<RuntimeAnimatorController>($"{animatorsPath}/Characters/{GetType().Name}");

        // initialize sub classes
        OnInitialize();

        health = maxHealth;
        hitbox.damage = damage;
    }

    /// <summary>
    /// Child initialization event, Start should not be used as it may superceed the correct initialization order
    /// </summary>
    protected abstract void OnInitialize();

    /// <summary>
    /// Event that happens when a Hitbox enters this Character's Hurtbox
    /// </summary>
    /// <param name="damage">Damage taken on entrance</param>
    /// aside: should technically be named OnHitboxEnter, but improves readability
    public virtual void OnHitboxEnter(float damage)
    {
        animator.SetTrigger("GetHit");

        health -= damage;
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

    public virtual void OnEnterHurtbox()
    {

    }

    public virtual void OnExitHurtbox()
    {

    }

    /// <summary>
    /// Enables hitbox
    /// </summary>
    public void EnableHitbox()
    {
        if (hitbox) hitbox.boxCollider.enabled = true;
    }

    /// <summary>
    /// Disables hitbox
    /// </summary>
    public void DisableHitbox()
    {
        if (hitbox) hitbox.boxCollider.enabled = false;
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
            //Gamepad.current.SetMotorSpeeds(rumbleSpeed, rumbleSpeed);
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
