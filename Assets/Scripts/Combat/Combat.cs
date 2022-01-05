using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum FIGHT_STAGE
    {
        PlayerAttack,
        BossStun,
        BossAttack,
        BossSpecial
    }

    [SerializeField] private Player player;
    [SerializeField] private Boss boss;
    public FIGHT_STAGE fightStage;
    private bool playerCanAttack;
    public BossSpecial currentBossSpecial = null;

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
        currentBossSpecial?.Cancel(); // runs if not null
        boss.OnGetHit(damage);
    }

    public void DisablePlayerAttack(FIGHT_STAGE newStage)
    {
        fightStage = newStage;
        playerCanAttack = false;
    }
    public void EnablePlayerAttack()
    {
        fightStage = FIGHT_STAGE.PlayerAttack;
        playerCanAttack = true;
    }

    public bool PlayerCanAttack()
    {
       return (fightStage == FIGHT_STAGE.PlayerAttack && playerCanAttack);
    }
}
