using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StunSlider : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// The group this controller belongs to
    /// </summary>
    public int group { get; set; }

    /// <summary>
    /// Slider this gameObject is attached to
    /// </summary>
    private Slider slider;

    /// <summary>
    /// How fast the slider counts down 
    /// </summary>
    [SerializeField] [Range(0,1f)] private float speed; 

    public Phase activePhase => Phase.Player;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// Hides the slider
    /// </summary>
    public void OnStart()
    {
        gameObject.SetActive(false); 
    }

    /// <summary>
    /// Initializes the slider
    /// </summary>
    public void OnPhaseEnter()
    {
        slider.value = 1f;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Counts down to next phase on phase update
    /// </summary>
    public void OnPhaseUpdate()
    {
        slider.value -= speed * Time.deltaTime;
        if (slider.value <= 0) this.ExitPhase(); 
    }

    /// <summary>
    /// Hides the slider
    /// </summary>
    public void OnPhaseExit()
    {
        gameObject.SetActive(false);
    }
}
