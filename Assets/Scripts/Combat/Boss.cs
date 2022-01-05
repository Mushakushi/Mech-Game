using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    [SerializeField] private int specialAttacks = 0;
    public List<BossSpecial> SpecialScripts = new List<BossSpecial>();

    private Coroutine stunTimer;
    [SerializeField] private int currentStunStage;
    private readonly (float x, float y, float duration)[] StunStages = { (.15f, .15f, 2.5f), (-.2f, .25f, 1.5f), (0f, .35f, 0.5f) };

    private void InitSpecials()
    {
        for (int i = 0; i < specialAttacks; i++)
        {
            string typeString = name + "Special" + (i + 1);
            gameObject.AddComponent(System.Type.GetType(typeString));
            SpecialScripts.Add((BossSpecial) gameObject.GetComponent(typeString));
            SpecialScripts[i].Boss = this;
        }
    }

    // Start is called before the first frame update
   override public void OnStart()
    {
        health = 250.0f;
        damage = 0f;
        resistance = 1.0f;
        defaultSpeed = 5.0f;

        startPos = transform.position;
        stunTimer = null;
        InitSpecials();
    }

    // Update is called once per frame
    void Update()
    {
        if (TrySmoothMove())
        {
            if (combat.fightStage == Combat.FIGHT_STAGE.BossStun)
            {
                currentStunStage = 0;
                combat.EnablePlayerAttack();
                // animation
            }
            if (combat.fightStage == Combat.FIGHT_STAGE.BossSpecial)
            {
                combat.currentBossSpecial.AttackStage += 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    override public void RunHitAnimation()
    {
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        stunTimer = StartCoroutine(StunForSeconds(StunStages[currentStunStage].duration));
        SetPosRelStart(StunStages[currentStunStage].x, StunStages[currentStunStage].y);
        // animation
        if (currentStunStage == 2)
        {
            combat.DisablePlayerAttack(Combat.FIGHT_STAGE.BossStun);
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

    public void CancelStun()
    {
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        currentStunStage = 0;
    }

    public abstract void DoNormalAttack();
}