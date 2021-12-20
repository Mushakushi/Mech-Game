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
    private Vector3 startPos;
    private Vector3 moveTarget;

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
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(this.transform.position, moveTarget, step);
            if (Vector3.Distance(this.transform.position, moveTarget) < 0.001f)
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
        moveTarget = startPos + Vector3.up * 1.2f;
        isAttacking = true;
    }
}
