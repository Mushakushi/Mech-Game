using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for recieving Phase notifcation calls. Will change Phase on complete
/// </summary>
// TODO - require PhaseExitBehavior component
public interface IPhaseController : IPhaseObserver
{
    /// <summary>
    /// What happens when the phase ends. Exits Phase on complete
    /// </summary>
    new void OnPhaseExit(); 
}
