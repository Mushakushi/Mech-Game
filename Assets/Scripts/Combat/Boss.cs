using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    [SerializeField] private int specialAttacks = 0;
    public List<BossSpecial> SpecialScripts = new List<BossSpecial>();

    private Coroutine stunTimer;
    [SerializeField] private int currentStunStage;
    public (float x, float y, float duration)[] StunStages { get; set; }
    
    public enum BOSS_STATE { AttackNormal, AttackSpecial, Stun, Default }
    public BOSS_STATE currentState;

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
        currentState = BOSS_STATE.Default;
        startPos = transform.position;
        stunTimer = null;
        InitSpecials();
        SetBossValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (TrySmoothMove())
        {
            switch (currentState)
            {
                case BOSS_STATE.AttackSpecial:
                    combat.currentBossSpecial.AttackStage += 1;
                    break;
                case BOSS_STATE.Stun:
                    StopStun();
                    combat.EnablePlayerAttack();
                    // animation
                    break;
            }
        }

        TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    override public void RunHitAnimation()
    {
        currentState = BOSS_STATE.Stun;
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        stunTimer = StartCoroutine(StunForSeconds(StunStages[currentStunStage].duration));
        SetPosRelStart(StunStages[currentStunStage].x, StunStages[currentStunStage].y);
        // animation
        if (currentStunStage == 2)
        {
            combat.DisablePlayerAttack();
        }
    }

    override public void OnGetHit(float damage)
    {
        TakeDamage(damage);
        RunHitAnimation();
        StartShake(0.05f, 0.03f);
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

    public void StopStun()
    {
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        currentStunStage = 0;
        currentState = BOSS_STATE.Default;
    }

    public void StopStun(BOSS_STATE newState)
    {
        if (stunTimer != null)
            StopCoroutine(stunTimer);
        currentStunStage = 0;
        currentState = newState;
    }

    public void HitShake()
    {

    }

    public abstract void DoNormalAttack();
    public abstract void SetBossValues();
}