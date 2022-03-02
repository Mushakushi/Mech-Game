using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player health ui "slider" graphic
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerHealthSlider : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    /// <summary>
    /// Decreases health by one 
    /// </summary>
    public void DepleteOneHealth()
    {
        animator.SetTrigger("GetHit"); 
    }
}
