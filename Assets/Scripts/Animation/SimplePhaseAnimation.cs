using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]

/// <summary>
/// Animates gameObject based on Phase calls without changing the current Phase
/// </summary>
public sealed class SimplePhaseAnimation : MonoBehaviour, IPhaseObserver
{
    /// <summary>
    /// Animator attached to this gameObject
    /// </summary>
    private Animator animator; 

    /// <summary>
    /// Phase(s) in which this gameObject belongs to
    /// </summary>
    [SerializeField] private List<Phase> activePhases;

    [Header("Trigger Names")]
    /// <summary>
    /// Trigger associated with Phase enter
    /// </summary>
    [SerializeField] private string phaseEnter;

    /// <summary>
    /// Trigger associated with Phase update
    /// </summary>
    [SerializeField] private string phaseUpdate; 

    /// <summary>
    /// Trigger associated with Phase exit
    /// </summary>
    [SerializeField] private string phaseExit; 

    /// <summary>
    /// Returns Combat.Phase if activePhases contains Combat.Phase. Phase.Invalid otherwise
    /// </summary>
    public Phase activePhase
    {
        get
        {
            foreach (Phase p in activePhases)
                if (p == Combat.phase)
                    return Combat.phase;
            return Phase.Invalid; 
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    /// <summary>
    /// Sets enter trigger
    /// </summary>
    public void OnPhaseEnter() => animator.SetTrigger(phaseEnter);

    /// <summary>
    /// Sets update trigger
    /// </summary>
    public void OnPhaseUpdate() => animator.SetTrigger(phaseUpdate); 

    /// <summary>
    /// Sets exit trigger
    /// </summary>
    public void OnPhaseExit() => animator.SetTrigger(phaseExit);

    public void OnStart() { }
}
