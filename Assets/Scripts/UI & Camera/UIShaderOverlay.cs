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
    /// Material being used 
    /// </summary>
    [SerializeField] [ReadOnly] private  Material uiShaderMaterial;

    /// <summary>
    /// Represents a material with variable _Color and shader
    /// </summary>
    [System.Serializable]
    private struct UIMaterial 
    {
        /// <summary>
        /// Shader 
        /// </summary>
        public Shader shader;

        /// <summary>
        /// Color to use as _Color param on shader
        /// </summary>
        public Color color; 

        public UIMaterial(Shader shader, Color color)
        {
            this.shader = shader;
            this.color = color; 
        }
    }

    /// <summary>
    /// Diamond transition shader
    /// </summary>
    [SerializeField] private UIMaterial diamondTransition;

    /// <summary>
    /// Speed of transition
    /// </summary>
    [SerializeField] [Range(0.0025f, 0.01f)] private float speed;

    /// <summary>
    /// Default unity shader
    /// </summary>
    [SerializeField] private UIMaterial defaultMaterial; 

    public int group { get; set; }

    public Phase activePhase => Phase.Intro; 

    private void Awake()
    {
        uiShaderMaterial = GetComponent<Image>().material;
        ApplyUIMaterial(defaultMaterial);
    }

    public void OnStart() { }

    public void OnPhaseEnter()
    {
        ApplyUIMaterial(diamondTransition);
        uiShaderMaterial.EnableKeyword("_Progress");
        uiShaderMaterial.SetFloat("_Progress", 0);
        StartCoroutine(Fade(FadeDirection.Out)); 
    }

    /// <summary>
    /// Fades in/out _Progress property in diamond shader
    /// </summary>
    private enum FadeDirection { In, Out }
    private IEnumerator Fade(FadeDirection direction)
    {
        float progress = uiShaderMaterial.GetFloat("_Progress");

        // progress slider
        switch (direction)
        {
            case FadeDirection.In:
                while (progress >= 0)
                {
                    uiShaderMaterial.SetFloat("_Progress", progress);
                    progress -= speed;
                    yield return null;
                }
                break;
            case FadeDirection.Out:
                while (progress < 1)
                {
                    uiShaderMaterial.SetFloat("_Progress", progress);
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
        ApplyUIMaterial(defaultMaterial);
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
                SetAlpha(uiShaderMaterial.color.a - 4);
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
        Color color = uiShaderMaterial.color;
        color.a = alpha;
        uiShaderMaterial.color = color;
    }

    /// <summary>
    /// Applies uimaterial to this object's material
    /// </summary>
    /// <param name="material">The UIMaterial to be applied to this material</param>
    private void ApplyUIMaterial(UIMaterial material)
    {
        uiShaderMaterial.shader = material.shader;
        uiShaderMaterial.color = material.color;
    }

    public void OnPhaseUpdate() { }

    public void OnPhaseExit() { }
}
