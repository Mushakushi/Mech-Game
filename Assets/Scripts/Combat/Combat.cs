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
    private Coroutine queuedBossAttack;

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

    public void DoBossDamage(float damage)
    {
        boss.OnGetHit(damage);
    }
}
