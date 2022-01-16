using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float resistance;

    [SerializeField] public Combat combat;
    [SerializeField] public Animator animator;

    [SerializeField] protected BoxCollider2D hitbox;
    [SerializeField] protected BoxCollider2D hurtbox;

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

    public void OnGetHit(float damage)
    {
        animator.SetTrigger("GetHit");
        health -= damage * (1 / resistance);
    }
}
