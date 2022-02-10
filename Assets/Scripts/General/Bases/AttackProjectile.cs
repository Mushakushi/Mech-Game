using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour, IHitboxOwner
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnHitboxEnter(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnHitboxExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnHurtboxEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnHurtboxExit()
    {
        throw new System.NotImplementedException();
    }

    
}
