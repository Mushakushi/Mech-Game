using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

// IMPORTANT - make sure you save to version control when REFACTORING because changing Unity's
// reserialization can actually delete all of the events without undo! 
// Please don't be like me :) (really be careful...)

/// <summary>
/// Phase Controller that publishes events
/// </summary>
[System.Serializable]
public sealed class DefaultPhaseEvent : IPhaseController
{
    // only accessible to this class doesn't matter what this is
    [HideInInspector] public GameObject gameObject => null;

    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }

    /// <summary>
    /// Phase(s) in which this gameObject belongs to
    /// </summary>
    [SerializeField] public List<Phase> activePhases;

    /// <summary>
    /// Returns PhaseManager.Phase if activePhases contains PhaseManager.Phase. Phase.Invalid otherwise
    /// </summary>
    [HideInInspector] public Phase activePhase { get => this.GetPhaseFromCollection(activePhases); }

    // note: different classes to serialize in unity
    [Header("OnPhaseEnter()")]
    /// <summary>
    /// Event associated with Phase enter
    /// </summary>
    [SerializeField] private PhaseEvent phaseEnter = new PhaseEvent();

    [Header("OnPhaseUpdate()")]
    /// <summary>
    /// Event associated with Phase update
    /// </summary>
    [SerializeField] private PhaseEvent phaseUpdate = new PhaseEvent();

    [Header("OnPhaseExit()")]
    /// <summary>
    /// Event associated with Phase exit
    /// </summary>
    [SerializeField] private PhaseEvent phaseExit = new PhaseEvent();

    /// <summary>
    /// Allows derived classes to be serializable
    /// </summary>
    [System.Serializable] private class PhaseEvent : UnityEvent { }

    /// <summary>
    /// What happens when this controller is added as a default Phase event
    /// </summary>
    public void OnStart() { }

    /// <summary>
    /// Sets enter trigger
    /// </summary>
    public void OnPhaseEnter()
    {
        phaseEnter.Invoke();
    }

    /// <summary>
    /// Sets update trigger
    /// </summary>
    public void OnPhaseUpdate()
    {
        phaseUpdate.Invoke();
    }

    /// <summary>
    /// Sets exit trigger
    /// </summary>
    public void OnPhaseExit()
    {
        phaseExit.Invoke();
    }
}
