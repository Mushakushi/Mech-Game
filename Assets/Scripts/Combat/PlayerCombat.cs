using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private float health = 100.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float damage = 5.0f;
    [SerializeField] private CombatManager cm;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    private bool isReturning = false;
    private Vector3 moveTarget;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            transform.position = MoveUtil.SmoothMove(this.transform.position, moveTarget, speed);
            if (MoveUtil.PosRoughlyEqual(this.transform.position, moveTarget))
            {
                if (isReturning)
                {
                    isAttacking = false;
                    isReturning = false;
                    //spriteRenderer.color = Color.green; //stand-in for animation
                }
                else
                {
                    cm.DoBossDamage(damage);
                    moveTarget = startPos;
                    isReturning = true;
                }
            }
        }
        else if (!isReturning && cm.playerCanAttack)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                DoAttack();
            }
        }
        
    }

    // Runs at a standard rate
    void FixedUpdate()
    {
        
    }

    public void DoAttack()
    {
        //spriteRenderer.color = Color.cyan; //stand-in for animation
        moveTarget = MoveUtil.GetPosFromPos(startPos, 1.2f, 0f);
        isAttacking = true;
    }
}
