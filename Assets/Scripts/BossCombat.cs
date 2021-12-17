using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    [SerializeField] private float health = 250.0f;
    [SerializeField] float speed = 5.0f;
    private Vector3 startPos;
    [SerializeField] public int stunStage;
    private bool isHit = false;
    public bool isReturning = false;
    Coroutine stunTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            if (isReturning)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(this.transform.position, startPos, step);
                if (Vector3.Distance(this.transform.position, startPos) < 0.001f)
                {
                    isHit = false;
                    isReturning = false;
                    stunStage = 0;
                }
            }
        }
    }

    public void PlayerHit(float amount)
    {
        isHit = true;
        switch (stunStage)
        {
            case 0:
                transform.position = startPos + Vector3.up * .15f + Vector3.left * .15f;
                stunTimer = StartCoroutine(StunForSeconds(2.0f));
                health -= amount;
                break;
            case 1:
                transform.position = startPos + Vector3.up * .25f + Vector3.right * .2f;
                StopCoroutine(stunTimer);
                stunTimer = StartCoroutine(StunForSeconds(1.0f));
                health -= amount;
                break;
            case 2:
                transform.position = startPos + Vector3.up * .35f;
                StopCoroutine(stunTimer);
                stunTimer = StartCoroutine(StunForSeconds(0.5f));
                health -= amount;
                break;
            default:
                StopCoroutine(stunTimer);
                isReturning = true;
                break;
        }
        stunStage++;
    }

    IEnumerator StunForSeconds(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        isReturning = true;
    }
}
