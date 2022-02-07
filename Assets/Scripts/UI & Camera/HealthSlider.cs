using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Boss health slider component
/// </summary>
[RequireComponent(typeof(Slider), typeof(Image))]
public class HealthSlider : MonoBehaviour
{
    /// <summary>
    /// Slider this gameObject is attached to
    /// </summary>
    [SerializeField] private Slider slider;

    /// <summary>
    /// Image this gameObject is attached to
    /// </summary>
    [SerializeField] private Image image; 

    /// <summary>
    /// Gradient of colors followed from 0<HP<HPMax
    /// </summary>
    [SerializeField] private Gradient gradient; 

    private void Start()
    {
        slider = GetComponent<Slider>();
        image = GetComponentsInChildren<Image>()[1]; 
    }

    /// <summary>
    /// Tints slider color based on current value
    /// </summary>
    public void Color()
    {
        image.color = gradient.Evaluate(slider.value); 
    }
}
