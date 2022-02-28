using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Jario : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Jario's animator
    /// </summary>
    [ReadOnly]
    [SerializeField] private Animator animator;

    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }

    /// <summary>
    /// Whether or not Jario is ready to count
    /// </summary>
    /// <remarks>Set to true when the boss collapses</remarks>
    public bool isReady;

    /// <summary>
    /// Function to execute when jario counts
    /// </summary>
    public delegate void JarioCountCallback();
    public event JarioCountCallback onJarioCountStart;

    /// <summary>
    /// Function to execute when jario counts
    /// </summary>
    public delegate void JarioStopCallback();
    public event JarioCountCallback onJarioCountStop;

    public Phase activePhase => this.GetPhaseFromCollection(new Phase[] { Phase.Intro, Phase.Boss_Collapse }) ;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnStart() { }

    /// <summary>
    /// Jario counts a variable amount of times
    /// </summary>
    public void OnPhaseEnter()
    {
        animator.SetTrigger("Count");
        animator.SetInteger("CountLeft", this.GetCounts());
    }

    /// <summary>
    /// Invokes callback(s) on Jario count, called in animator
    /// </summary>
    public void StartCount() => onJarioCountStart?.Invoke();

    /// <summary>
    /// Invokes callback(s) on Jario count, called in animator
    /// </summary>
    public void OnCount() => onJarioCountStart?.Invoke();

    /// <summary>
    /// Invokes callback(s) on Jario count stop, called in animator
    /// </summary>
    public void StopCount() => onJarioCountStop?.Invoke();

    public void OnPhaseUpdate() { }
    public void OnPhaseExit() { }
}
