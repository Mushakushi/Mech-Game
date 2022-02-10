using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// animates shader on ui image overlay
/// </summary>
[RequireComponent(typeof(Image))]
public class UIShaderOverlay : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Speed of transition
    /// </summary>
    [SerializeField] [Range(0.0025f,0.01f)] private float speed; 

    private Material material;

    /// <summary>
    /// Diamond transition shader
    /// </summary>
    [SerializeField] private Shader diamondTransition;

    /// <summary>
    /// Default unity shader
    /// </summary>
    [SerializeField] private Shader defaultShader; 

    public int group { get; set; }

    public Phase activePhase => Phase.Intro; 

    private void Awake()
    {
        material = GetComponent<Image>().material;
        material.shader = defaultShader; 
    }

    public void OnStart() { }

    public void OnPhaseEnter()
    {
        material.shader = diamondTransition;
        material.SetFloat("_Progress", 0);
        StartCoroutine(Fade(FadeDirection.Out)); 
    }

    /// <summary>
    /// Fades in/out _Progress property in diamond shader
    /// </summary>
    private enum FadeDirection { In, Out }
    private IEnumerator Fade(FadeDirection direction)
    {
        float progress = material.GetFloat("_Progress"); 

        // progress slider
        switch (direction)
        {
            case FadeDirection.In:
                while (progress >= 0)
                {
                    material.SetFloat("_Progress", progress);
                    progress -= speed;
                    yield return null;
                }
                break;
            case FadeDirection.Out:
                while (progress < 1)
                {
                    material.SetFloat("_Progress", progress);
                    progress += speed;
                    yield return null;
                }
                break; 
        }
    }

    /// <summary>
    /// Avoids assigning default shader during flash() coroutine, then starts it
    /// </summary>
    public void StartFlash()
    {
        material.shader = defaultShader;
        StartCoroutine(Flash());
    }

    /// <summary>
    /// Flashes material with alpha coroutine
    /// </summary>
    private IEnumerator Flash()
    {
        for (int i = 0; i < 3; i++)
        {
            SetAlpha(100);
            for (int j = 0; j < 25; j++)
            {
                SetAlpha(material.color.a - 4);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(0.1f); 
        }
    }

    /// <summary>
    /// Sets material alpha to <paramref name="alpha"/>
    /// </summary>
    /// <param name="alpha">Value to set material alpha to</param>
    private void SetAlpha(float alpha)
    {
        Color color = material.color;
        color.a = alpha;
        material.color = color;
    }

    public void OnPhaseUpdate() { }

    public void OnPhaseExit() { }
}
