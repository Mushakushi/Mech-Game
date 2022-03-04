using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalSettings
{
    /// <summary>
    /// Whether or not to allow joining
    /// </summary>
    public static bool isMultiplayerGame = false;

    /// <summary>
    /// Enable or disables joining based on <see cref="isMultiplayerGame"/>
    /// </summary>
    /// <remarks>
    /// Utility that should only be called when a PlayerInputManager exists. 
    /// Leaving the respective paramter enabled in editor can unitentionally
    /// allow player joining before it is called.
    /// </remarks>
    public static void EnforceJoiningState()
    {
        if (isMultiplayerGame) PlayerInputManager.instance?.EnableJoining();
        else PlayerInputManager.instance?.DisableJoining();
    }
}
