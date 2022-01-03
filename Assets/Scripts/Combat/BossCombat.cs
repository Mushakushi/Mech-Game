using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossCombat : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private float health = 250.0f;
    [SerializeField] private float resistance = 1.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] public int stunStage;
    [SerializeField] private int specialAttacks = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CombatManager cm;
    private List<BossSpecial> SpecialScripts = new List<BossSpecial>();
    private Vector3 startPos;
    private bool isHit = false;
    private bool isReturning = false;
    Coroutine stunTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        stunTimer = null;
        InitSpecials();
    }

    private void InitSpecials()
    {
        for(int i = 0; i < specialAttacks; i++)
        {
            string typeString = name + "Special" + (i+1);
            gameObject.AddComponent(System.Type.GetType(typeString));
            SpecialScripts.Add((BossSpecial) this.gameObject.GetComponent(typeString));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit && isReturning)
        {
            transform.position = MoveUtil.SmoothMove(this.transform.position, startPos, speed);
            if (MoveUtil.PosRoughlyEqual(this.transform.position, startPos))
            {
                isHit = false;
                isReturning = false;
                stunStage = 0;
                cm.playerCanAttack = true;
                //spriteRenderer.color = Color.white; // stand-in for animation
            }
        }
    }

    public void PlayerHit(float damage)
    {
        isHit = true;
        health -= damage * (1 / resistance);
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
                transform.position = MoveUtil.GetPosFromPos(startPos, .15f, .15f);
                break;
            case 1:
                //spriteRenderer.color = Color.yellow; //stand-in for animation
                transform.position = MoveUtil.GetPosFromPos(startPos, .25f, -.2f);
                break;
            case 2:
                //spriteRenderer.color = Color.blue; //stand-in for animation
                transform.position = MoveUtil.GetPosFromPos(startPos, .35f, 0f);
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
