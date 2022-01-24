using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
