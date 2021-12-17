using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] float health = 100.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float damage = 5f;
    [SerializeField] CombatManager combatManager;
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
                }
                else
                {
                    combatManager.DoBossDamage(damage);
                    moveTarget = startPos;
                    isReturning = true;
                }
            }
        }
        else if (!isReturning && Input.GetKeyDown(KeyCode.W) && combatManager.fightStage == CombatManager.FIGHT_STAGE.PlayerAttack)
        {
            combatManager.TryPlayerAttack(5.0f);
        }
        
    }

    // Runs at a standard rate
    void FixedUpdate()
    {
        
    }

    public void DoAttack()
    {
        moveTarget = startPos + Vector3.up * 1.2f;
        isAttacking = true;
    }
}
