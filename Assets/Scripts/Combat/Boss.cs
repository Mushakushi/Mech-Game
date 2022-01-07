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

    public Vector3 lastPos;

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
        startPos = transform.position;
        stunTimer = null;
        InitSpecials();
        SetBossValues();
    }

    // Update is called once per frame
    override public void OnUpdate()
    {
        TryShake();

        if (Input.GetKeyDown(KeyCode.P))
        {
            combat.DoBossSpecial();
        }
    }

    override public void OnGetHit(float damage)
    {
        animator.SetTrigger("GetHit");
        TakeDamage(damage);
    }

    override public void TakeDamage(float damage)
    {
        health -= damage * (1 / resistance);
    }

    public abstract void DoNormalAttack();
    public abstract void SetBossValues();
}