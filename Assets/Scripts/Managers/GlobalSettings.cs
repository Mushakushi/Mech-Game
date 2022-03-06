using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GlobalSettings
{
    /// <summary>
    /// Whether or not to allow joining
    /// </summary>
    public static bool isMultiplayerGame;

    /// <summary>
    /// Whether or not one-hit mode is enabled
    /// </summary>
    public static bool isOneHitMode { get; private set; }

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

    /// <summary>
    /// Sets the status of one hit mode
    /// </summary>
    public static void SetOneHitMode(bool isEnabled) => isOneHitMode = isEnabled;
}
