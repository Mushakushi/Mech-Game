using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthBarsLeftText : MonoBehaviour, IPhaseController
{
    public int group { get; set; }

    /// <remarks>Decreases count on boss collapse</remarks>
    public Phase activePhase => Phase.Boss_Collapse;

    /// <summary>
    /// Text attached to this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private TMP_Text text;

    public void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnStart() 
    {
        if (this.GetManager().boss.resistance != 0) text.text = $"x{this.GetManager().boss.maxHealthBars}";
    }

    public void OnPhaseEnter() 
    {
        if (this.GetManager().boss.resistance != 0) text.text = $"x{this.GetManager().boss.healthBars}";
    }

    public void OnPhaseExit() { }
    public void OnPhaseUpdate() { }

}
