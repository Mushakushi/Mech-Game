using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Boss health slider component
/// </summary>
[RequireComponent(typeof(Slider), typeof(Image))]
public class BossHealthSlider : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Slider this gameObject is attached to
    /// </summary>
    [SerializeField] [ReadOnly] private Slider slider;

    /// <summary>
    /// Animator attached to this gameObject
    /// </summary>
    [SerializeField] private Animator animator;

    /// <summary>
    /// White effect behind slider value, first child of this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private RectTransform whiteEffect;

    /// <summary>
    /// Image of slider value, second child of this gameObject
    /// </summary>
    [SerializeField] private Image image;

    /// <summary>
    /// Gradient of colors followed from 0 < HP < HPMax for slider value 
    /// </summary>
    [SerializeField] private Gradient gradient;

    public int group { get; set; }

    public Phase activePhase => this.GetPhaseFromCollection(new Phase[]{ Phase.Player, Phase.Player_Win});

    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = GetComponent<Slider>();
        slider.value = 1;

        whiteEffect = GetComponentsInChildren<RectTransform>()[1];
        image = GetComponentsInChildren<Image>()[2];
    }

    public void OnStart() { }

    /// <summary>
    /// Tints slider color based on current value, called in slider
    /// </summary>
    public void Color()
    {
        image.color = gradient.Evaluate(slider.value); 
    }

    public void SetValue(float value)
    {
        if (value != slider.value)
        {
            animator.SetTrigger("Shake");
            whiteEffect.anchorMax = new Vector2(slider.value, 1);
            slider.value = value;
            this.WaitForSeconds(0.1f, () => StartCoroutine(DoWhiteEffect(value, 0.002f)));
        }
    } 

    private IEnumerator DoWhiteEffect(float toValue, float speed)
    {
        while (whiteEffect.anchorMax.x >= toValue)
        {
            whiteEffect.anchorMax -= speed * Vector2.right;
            yield return null; 
        }
    }

    public void OnPhaseEnter() { }

    public void OnPhaseUpdate()
    {
        SetValue(this.GetManager().boss.health / this.GetManager().boss.maxHealth); 
    }

    public void OnPhaseExit() { }
}
