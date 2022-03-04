using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

/// <summary>
/// Can be used in the animator to do a event on complete
/// </summary>
public class SimpleEventHandler : MonoBehaviour
{
    public UnityEvent @event = new UnityEvent();

    public void DoEvent() { @event.Invoke(); }
}
