using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] public float health;
    [SerializeField] private float currentSpeed;
    [SerializeField] public float defaultSpeed;
    [SerializeField] public float damage;
    [SerializeField] public float resistance;
    [SerializeField] public Combat combat;
    [SerializeField] public Animator animator;
    public bool isSmoothMoving = false;
    public bool isShaking = false;
    public Vector3 shakePos;
    public float shakingRange;
    public Vector3 smoothMoveTarget;
    public Vector3 startPos;
    public bool returnToStart = false;

    private void Start()
    {
        OnStart();
        currentSpeed = defaultSpeed;
    }

    void Update()
    {
        if (returnToStart)
        {
            animator.applyRootMotion = false;
            transform.position = startPos;
            returnToStart = false;
            animator.ResetTrigger("GetHit");
            animator.ResetTrigger("RunSpecial");
        }
        OnUpdate();
    }

        /// <summary>
        /// Moves character smoothly between current position and smoothMoveTarget.
        /// </summary>
        /// <returns>true when SmoothMove has completed (positions are roughly equal), and false if otherwise.</returns>
        public bool TrySmoothMove()
    {
        if (isSmoothMoving)
        {
            transform.position = MoveUtil.SmoothMove(transform.position, smoothMoveTarget, currentSpeed);
            if (MoveUtil.PosRoughlyEqual(transform.position, smoothMoveTarget))
            {
                isSmoothMoving = false;
                return true;
            }
        }
        return false;
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
    /// Set the character position relative to the character's start position.
    /// </summary>
    /// <param name="x">Horizontal value. Negative for left, positive for right.</param>
    /// <param name="y">Vertical value. Negative for down, positive for up.</param>
    public void SetPosRelStart(float x, float y)
    {
        transform.position = MoveUtil.GetPosFromPos(startPos, x, y);
    }

    /// <summary>
    /// Get a position relative to the character's start position.
    /// </summary>
    /// <param name="x">Horizontal value. Negative for left, positive for right.</param>
    /// <param name="y">Vertical value. Negative for down, positive for up.</param>
    public Vector3 GetPosRelStart(float x, float y)
    {
       return MoveUtil.GetPosFromPos(startPos, x, y);
    }

    /// <summary>
    /// Set the character position relative to the character's current position.
    /// </summary>
    /// <param name="x">Horizontal value. Negative for left, positive for right.</param>
    /// <param name="y">Vertical value. Negative for down, positive for up.</param>
    public void SetPosRelPos(float x, float y)
    {
        transform.position = MoveUtil.GetPosFromPos(transform.position, x, y);
    }

    /// <summary>
    /// Get the a position relative to the character's current position.
    /// </summary>
    /// <param name="x">Horizontal value. Negative for left, positive for right.</param>
    /// <param name="y">Vertical value. Negative for down, positive for up.</param>
    public Vector3 GetPosRelPos(float x, float y)
    {
        return MoveUtil.GetPosFromPos(transform.position, x, y);
    }

    public void BeginSmoothMoveToStart()
    {
        currentSpeed = defaultSpeed;
        smoothMoveTarget = startPos;
        isSmoothMoving = true;
    }

    public void BeginSmoothMoveToPos(Vector3 pos, float speed)
    {
        currentSpeed = speed;
        smoothMoveTarget = pos;
        isSmoothMoving = true;
    }

    public void BeginSmoothMoveToPos(Vector3 pos)
    {
        currentSpeed = defaultSpeed;
        smoothMoveTarget = pos;
        isSmoothMoving = true;
    }

    public abstract void OnGetHit(float damage);
    public abstract void TakeDamage(float damage);
    public abstract void OnStart();
    public abstract void OnUpdate();
}
