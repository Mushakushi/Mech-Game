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

    public void OnPhaseEnter()
    {
        animator.SetTrigger("Count");
    }

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
