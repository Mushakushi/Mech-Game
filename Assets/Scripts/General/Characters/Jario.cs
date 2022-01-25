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


    public Phase activePhase { get; }

    public Jario()
    {
        activePhase = Phase.Intro; 
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPhaseEnter()
    {
        animator.SetTrigger("Count");
        int count;
        if (Combat.phase == Phase.Intro) count = 3;
        else count = 10;

        animator.SetInteger("CountLeft", count);
    }

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
