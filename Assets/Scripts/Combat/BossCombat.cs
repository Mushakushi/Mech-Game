using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [SerializeField] private float health = 250.0f;
    [SerializeField] private float resistance = 1.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private CombatManager cm;
    [SerializeField] public int stunStage;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector3 startPos;
    private bool isHit = false;
    private bool isReturning = false;
    Coroutine stunTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        stunTimer = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            if (isReturning)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(this.transform.position, startPos, step);
                if (Vector3.Distance(this.transform.position, startPos) < 0.001f)
                {
                    isHit = false;
                    isReturning = false;
                    stunStage = 0;
                    cm.playerCanAttack = true;
                    //spriteRenderer.color = Color.white; // stand-in for animation
                }
            }
        }
    }

    public void PlayerHit(float damage)
    {
        isHit = true;

        health = damage * (resistance / 1);

        HitAnimation();
        stunStage++;
    }

    private void HitAnimation()
    {
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        stunTimer = StartCoroutine(StunForSeconds(2.5f - stunStage));
        switch (stunStage)
        {
            case 0:
                //spriteRenderer.color = Color.red; //stand-in for animation
                transform.position = startPos + Vector3.up * .15f + Vector3.left * .15f;
                break;
            case 1:
                //spriteRenderer.color = Color.yellow; //stand-in for animation
                transform.position = startPos + Vector3.up * .25f + Vector3.right * .2f;
                break;
            case 2:
                //spriteRenderer.color = Color.blue; //stand-in for animation
                transform.position = startPos + Vector3.up * .35f;
                cm.playerCanAttack = false;
                break;
        }
    }

    IEnumerator StunForSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        isReturning = true;
    }
}
