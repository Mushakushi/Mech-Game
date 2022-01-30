using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

/// <summary>
/// Phase Controller that publishes events
/// </summary>
[System.Serializable]
public sealed class DefaultPhaseEvent : IPhaseController
{
    /// <summary>
    /// Phase(s) in which this gameObject belongs to
    /// </summary>
    [SerializeField] public List<Phase> activePhases;

    /// <summary>
    /// Returns PhaseManager.Phase if activePhases contains PhaseManager.Phase. Phase.Invalid otherwise
    /// </summary>
    [HideInInspector] public Phase activePhase { get => activePhases.GetPhase(); }

    [Header("OnPhaseEnter()")]
    /// <summary>
    /// Event associated with Phase enter
    /// </summary>
    [SerializeField] private PhaseEnter phaseEnter = new PhaseEnter { }; 
    [System.Serializable] private class PhaseEnter : UnityEvent { }

    [Header("OnPhaseUpdate()")]
    /// <summary>
    /// Event associated with Phase update
    /// </summary>
    [SerializeField] private PhaseUpdate phaseUpdate = new PhaseUpdate { }; 
    [System.Serializable] private class PhaseUpdate : UnityEvent { }

    [Header("OnPhaseExit()")]
    /// <summary>
    /// Event associated with Phase exit
    /// </summary>
    [SerializeField] private PhaseExit phaseExit = new PhaseExit { }; 
    [System.Serializable] private class PhaseExit : UnityEvent { }

    /// <summary>
    /// What happens when this controller is added as a default Phase event
    /// </summary>
    public void OnStart() { }

    /// <summary>
    /// Sets enter trigger
    /// </summary>
    public void OnPhaseEnter() => phaseEnter.Invoke();

    /// <summary>
    /// Sets update trigger
    /// </summary>
    public void OnPhaseUpdate() => phaseUpdate.Invoke();

    /// <summary>
    /// Sets exit trigger
    /// </summary>
    public void OnPhaseExit() => phaseUpdate.Invoke(); 
}
