using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public enum FIGHT_STAGE
    {
        PlayerAttack,
        BossAttack,
        BossSpecial
    }

    [SerializeField] private PlayerCombat player;
    [SerializeField] private BossCombat boss;
    public FIGHT_STAGE fightStage;

    // Start is called before the first frame update
    void Start()
    {
        fightStage = FIGHT_STAGE.PlayerAttack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryPlayerAttack(float damage)
    {
        if (!boss.isReturning && !(boss.stunStage > 2))
        {
            player.DoAttack();
        }
    }

    public void DoBossDamage(float damage)
    {
        boss.PlayerHit(damage);
    }
}
