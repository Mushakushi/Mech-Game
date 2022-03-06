using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNumberText : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Text attached to this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private TMP_Text text;

    public int group { get; set; }

    public Phase activePhase => Phase.Intro;

    public void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        //gameObject.SetActive(false);
    }

    public void OnPhaseEnter()
    {
        gameObject.SetActive(true); // TODO - look into why clone isn't recieving this call
    }

    public void OnPhaseExit()
    {
        gameObject.SetActive(false);
    }

    public void OnPhaseUpdate() { }

    public void OnStart()
    {
        if (GlobalSettings.isMultiplayerGame) text.text = $"YOU ARE P{group + 1}";
        else text.text = "YOU";
    }
}
