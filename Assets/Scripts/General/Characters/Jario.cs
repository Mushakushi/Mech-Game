using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Jario : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Jario's animator
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }


    public Phase activePhase => this.GetPhaseFromCollection(new Phase[] { Phase.Intro, Phase.Boss_Collapse }) ;

    public void OnStart()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Jario counts a variable amount of times
    /// </summary>
    public void OnPhaseEnter()
    {
        animator.SetTrigger("Count");
        int count;
        if (this.GetManagerPhase() == Phase.Intro) count = 3;
        else count = 10;

        animator.SetInteger("CountLeft", count-1); // count-1 or he'll count one more time than you want him to
    }

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
