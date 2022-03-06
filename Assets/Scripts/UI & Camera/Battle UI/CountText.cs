using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CountText : MonoBehaviour, IPhaseController
{
    public int group { get; set; }

    public Phase activePhase => Phase.None;

    /// <summary>
    /// Text attached to this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private TMP_Text text;

    public void OnPhaseEnter() { }
    public void OnPhaseExit() { }
    public void OnPhaseUpdate() { }

    public void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // subs to jario events
    public void OnStart()
    {
        gameObject.SetActive(false); 

        this.GetManager().jario.onJarioCountStart += () => {
            gameObject.SetActive(true);
            text.text = (this.GetCounts() + 1).ToString();
        };

        this.GetManager().jario.onJarioCount += () => {
            text.text = (Convert.ToInt32(text.text) - 1).ToString();
        };

        this.GetManager().jario.onJarioCountStop += () => gameObject.SetActive(false);
    }
}
