using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitApplication() => Application.Quit();

    /// <summary>
    /// Quits the application on escape key
    /// </summary>
    public void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("escape"))) QuitApplication();
    }
}
