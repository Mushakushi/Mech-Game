using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Jario : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Jario's animator
    /// </summary>
    [ReadOnly] [SerializeField] private Animator animator;

    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }

    // TODO - possibly make event args and get rid of jarioUtility

    /// <summary>
    /// Function to execute when jario counts
    /// </summary>
    public event JarioCountCallback onJarioCountStart;
    public delegate void JarioCountCallback();

    /// <summary>
    /// Function to execute when jario counts
    /// </summary>
    public event JarioCountCallback onJarioCount;

    /// <summary>
    /// Function to execute when jario counts
    /// </summary>
    public event JarioCountCallback onJarioCountStop;

    public Phase activePhase => this.GetPhaseFromCollection(new Phase[] { Phase.Intro }) ;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnStart() { }

    /// <summary>
    /// Jario counts three times to start off the match
    /// </summary>
    public void OnPhaseEnter() 
    {
        animator.SetTrigger("Count");
        animator.SetInteger("CountLeft", 3);
    }

    /// <summary>
    /// Jario counts a variable amount of times and Invokes callback(s) on Jario count, called in animator
    /// </summary>
    public void StartCount()
    {
        animator.SetTrigger("Count");
        animator.SetInteger("CountLeft", this.GetCounts());
        onJarioCountStart?.Invoke();
    }  

    /// <summary>
    /// Invokes callback(s) on Jario count, called in animator
    /// </summary>
    public void OnCount() => onJarioCount?.Invoke();

    /// <summary>
    /// Invokes callback(s) on Jario count stop, called in animator
    /// </summary>
    public void StopCount()
    {
        animator.ResetTrigger("Count"); 
        onJarioCountStop?.Invoke();
    }   

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
