using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] public float health;
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] public float resistance;
    [SerializeField] public Combat combat;
    public bool isSmoothMoving = false;
    public Vector3 smoothMoveTarget;
    public Vector3 startPos;

    /// <summary>
    /// Moves character smoothly between current position and smoothMoveTarget.
    /// </summary>
    /// <returns>true when SmoothMove has completed, and false if otherwise.</returns>
    public bool TrySmoothMove()
    {
        if (isSmoothMoving)
        {
            transform.position = MoveUtil.SmoothMove(this.transform.position, smoothMoveTarget, speed);
            if (MoveUtil.PosRoughlyEqual(this.transform.position, smoothMoveTarget))
            {
                isSmoothMoving = false;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Set the character position relative to the character's startPos.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetPosRelStart(float x, float y)
    {
        transform.position = MoveUtil.GetPosFromPos(startPos, x, y);
    }

    public abstract void OnGetHit(float damage);
    public abstract void TakeDamage(float damage);
    public abstract void RunHitAnimation();
}
