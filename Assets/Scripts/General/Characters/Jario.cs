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

    private void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    /// <summary>
    /// Animates jario counting
    /// </summary>
    public void Count()
    {
        animator.SetTrigger("Count"); 
    }

    public void OnPhaseEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
