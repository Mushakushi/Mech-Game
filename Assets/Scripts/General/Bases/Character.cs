using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// Slider UI Component in scene that this object controls
    /// </summary>
    [SerializeField] private Slider healthSlider; 

    [Space()]
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
        hitbox = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = Resources.Load($"Animation/Animators/{GetType().Name}") as RuntimeAnimatorController;

        EnableHurtbox();
        DisableHitbox(); 
        OnStart();
    }

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
