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


    // Start is called before the first frame update
    void Start()
    {
        EnablePlayerAttack();
        boss.combat = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DoBossSpecial()
    {
        fightStage = FIGHT_STAGE.BossSpecial;
        boss.currentState = Boss.BOSS_STATE.AttackSpecial;
        // choose based on weight/list/whatever (tbd)
        int weightSum = 0;
        for (int i = 0; i < boss.SpecialAttackWeights.Count; i++)
        {
            weightSum += boss.SpecialAttackWeights[i];
        }
        int rand = Random.Range(0, weightSum);
        for (int i = 0; i < boss.SpecialAttackWeights.Count; i++)
        {
            if (rand < boss.SpecialAttackWeights[i])
            {
                boss.animator.SetInteger("SpecialIndex", i + 1);
                boss.animator.SetTrigger("RunSpecial");
            }

            rand -= boss.SpecialAttackWeights[i];
        }
    }

    public void DisablePlayerAttack()
    {
        player.canAttack = false;
    }

    public void EnablePlayerAttack()
    {
        fightStage = FIGHT_STAGE.PlayerAttack;
        player.canAttack = true;
    }
}
