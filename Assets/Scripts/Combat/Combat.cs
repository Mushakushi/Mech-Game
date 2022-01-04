using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum FIGHT_STAGE
    {
        PlayerAttack,
        BossAttack,
        BossSpecial
    }

    [SerializeField] private Player player;
    [SerializeField] private Boss boss;
    public FIGHT_STAGE fightStage;
    public bool playerCanAttack;
    public BossSpecial currentBossSpecial;

    // Start is called before the first frame update
    void Start()
    {
        fightStage = FIGHT_STAGE.PlayerAttack;
        playerCanAttack = true;
        player.combat = this;
        boss.combat = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoBossSpecial()
    {
        fightStage = FIGHT_STAGE.BossSpecial;
        // choose based on weight/list/whatever (tbd)
        currentBossSpecial = boss.SpecialScripts[0];
        currentBossSpecial.RunSpecial();
    }

    public void DoBossDamage(float damage)
    {
        boss.OnGetHit(damage);
    }
}
