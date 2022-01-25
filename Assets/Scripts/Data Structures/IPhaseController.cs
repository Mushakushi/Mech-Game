using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for notifying phase events in PhaseBehavior state machine. 
/// Every IPhaseController must have PhaseUpdateBehvaior and PhaseExitBehavior to function properly
/// </summary>
public interface IPhaseController
{
    /// <summary>
    /// What phase the controller belongs to
    /// </summary>
    Phase activePhase { get; }

    /// <summary>
    /// What happens when controller is added to Combat
    /// </summary>
    void OnStart(); 

    /// <summary>
    /// What happens when the phase ends 
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
