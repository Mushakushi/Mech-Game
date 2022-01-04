using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    [SerializeField] private int specialAttacks = 0;
    private List<BossSpecial> SpecialScripts = new List<BossSpecial>();

    private Coroutine stunTimer;
    [SerializeField] private int currentStunStage;
    private (float x, float y)[] StunStages = { (.15f, .15f), (-.2f, .25f), (0f, .35f) };

    private void InitSpecials()
    {
        for (int i = 0; i < specialAttacks; i++)
        {
            string typeString = name + "Special" + (i + 1);
            gameObject.AddComponent(System.Type.GetType(typeString));
            SpecialScripts.Add((BossSpecial) this.gameObject.GetComponent(typeString));
            SpecialScripts[i].boss = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 250.0f;
        speed = 5.0f;
        damage = 0f;
        resistance = 1.0f;

        startPos = this.transform.position;
        stunTimer = null;
        InitSpecials();
    }

    // Update is called once per frame
    void Update()
    {
        if (TrySmoothMove())
        {
            currentStunStage = 0;
            combat.playerCanAttack = true;
            // animation
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpecialScripts[0].RunAttackAnimation();
        }
    }

    override public void RunHitAnimation()
    {
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        stunTimer = StartCoroutine(StunForSeconds(2.5f - currentStunStage));
        SetPosRelStart(StunStages[currentStunStage].x, StunStages[currentStunStage].y);
        if (currentStunStage == 2)
        {
            combat.playerCanAttack = false;
        }
    }

    override public void OnGetHit(float damage)
    {
        TakeDamage(damage);
        RunHitAnimation();
        currentStunStage++;
    }

    override public void TakeDamage(float damage)
    {
        health -= damage * (1 / resistance);
    }

    IEnumerator StunForSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        smoothMoveTarget = startPos;
        isSmoothMoving = true;
    }

    public abstract void DoNormalAttack();
}