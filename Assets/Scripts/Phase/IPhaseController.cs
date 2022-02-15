using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for recieving Phase notifcation calls
/// </summary>
public interface IPhaseController
{
    /// <summary>
    /// GameObject this IPhaseController Component is attached to. 
    /// Implemented in MonoBehavior
    /// </summary>
    GameObject gameObject { get; }

    /// <summary>
    /// Index of the battle group the controller belongs to
    /// </summary>
    int group { get; set;}

    /// <summary>
    /// What phase the controller belongs to. 
    /// Interfaces can change return value to allow for multiple active Phases
    /// </summary>
    Phase activePhase { get; }

    /// <summary>
    /// What happens when controller is added to PhaseManager
    /// </summary>
    void OnStart();

    /// <summary>
    /// What happens when the phase ends. 
    /// Called after <see cref="OnStart"/>.
    /// </summary>
    void OnPhaseEnter();

    /// <summary>
    /// What happens when the phase is running 
    /// </summary>
    void OnPhaseUpdate();

    /// <summary>
    /// What happens when the phase ends
    /// </summary>
    void OnPhaseExit();
}
