using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] public float health;
    [SerializeField] public float damage;
    [SerializeField] public float resistance;
    [SerializeField] public Combat combat;
    [SerializeField] public Animator animator;
    [SerializeField] public BoxCollider2D hitbox;
    [SerializeField] public BoxCollider2D hurtbox;
    public bool isShaking = false;
    public Vector3 shakePos;
    public float shakingRange;
    public bool returnToIdle = false;
    private bool beingHit;
    public ContactFilter2D attackLayerFilter = new ContactFilter2D();

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

    public void CheckBeingHit()
    {
        if (!beingHit)
        {
            if (Physics2D.OverlapCollider(hurtbox, attackLayerFilter, new List<Collider2D>()) > 0)
            {
                beingHit = true;
                animator.SetTrigger("GetHit");
            }
        }
        else
        {
            if (Physics2D.OverlapCollider(hurtbox, attackLayerFilter, new List<Collider2D>()) == 0)
            {
                beingHit = false;
            }
        }
    }

    public void OnGetHit(float damage)
    {
        animator.SetTrigger("GetHit");
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage * (1 / resistance);
    }
}
