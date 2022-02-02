using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public static class InputActionUtility
{
    /// <summary>
    /// Checks if button input action <paramref name="buttonAction"/> was pressed this frame
    /// </summary>
    /// <param name="buttonAction">The input action of action type button</param>
    /// <returns>True if <paramref name="buttonAction"/> was pressed this frame. False otherwise</returns>
    public static bool WasPressedThisFrame(this InputAction buttonAction)
    {
        if (buttonAction.type != InputActionType.Button) throw new System.Exception("Input action is not of type Button!");
        return buttonAction.triggered; 
    }
}
