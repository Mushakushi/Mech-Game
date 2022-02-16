using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Jario : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Jario's animator
    /// </summary>
    [ReadOnly]
    [SerializeField] private Animator animator;

    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }


    public Phase activePhase => this.GetPhaseFromCollection(new Phase[] { Phase.Intro, Phase.Boss_Collapse }) ;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnStart() { }

    /// <summary>
    /// Jario counts a variable amount of times
    /// </summary>
    public void OnPhaseEnter()
    {
        animator.SetTrigger("Count");
        animator.SetInteger("CountLeft", 
            HealthConversion.ConvertBarsToCount(this.GetManager().boss.healthBars, this.GetManager().boss.maxHealthBars));
    }

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
