using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Exposes animator functions
/// </summary>
public class AnimatorExposer : MonoBehaviour
{
    /// <summary>
    /// Animator attached to this gameObject
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Calls trigger named <paramref name="trigger"/> in animator
    /// </summary>
    /// <param name="trigger">The name of the trigger</param>
    public void SetTriggerInAnimator(string trigger) => animator.SetTrigger(trigger); 
}
