using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for notifying phase events in PhaseBehavior state machine. 
/// Every IPhaseController must have PhaseBehavior in a substate machine to function properly.
/// </summary>
public interface IPhaseController
{
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
