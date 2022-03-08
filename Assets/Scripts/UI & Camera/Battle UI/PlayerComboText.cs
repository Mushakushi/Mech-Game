using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerComboText : MonoBehaviour, IPhaseController
{
    public int group { get; set; }

    public Phase activePhase => this.GetPhaseFromCollection(new Phase[] { Phase.Player, Phase.Boss_Guard, Phase.Boss_Collapse});

    /// <summary>
    /// Text attached to this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private TMP_Text text;

    public void OnPhaseEnter() 
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Player:
                gameObject.SetActive(true);
                text.text = "0 hits..";
                break;
            case Phase.Boss_Guard:
                switch (Convert.ToInt32(text.text.Substring(0, 1)))
                {
                    case int num when num >= 4:
                        text.text = "PERFECT!";
                        break;
                    case 3:
                        text.text = "Great!";
                        break;
                    case 2:
                        text.text = "Good";
                        break;
                    case 1:
                        text.text = "Only one?";
                        break;
                    case int num when num < 1:
                    default:
                        text.text = "Fail..";
                        break;
                }
                this.WaitForSeconds(1f, () => gameObject.SetActive(false));
                break;
            case Phase.Boss_Collapse:
            default:
                text.text = "DEMOLISHED.";
                this.WaitForSeconds(1f, () => gameObject.SetActive(false));
                break;
        }
    }

    public void OnPhaseExit() { }

    public void OnPhaseUpdate() { }

    public void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // subs to jario events
    public void OnStart()
    {
        this.GetManager().player.onHitboxEnter.AddListener(() =>
        {
            if (this.GetManagerPhase() == Phase.Player)
                text.text = $"{Convert.ToInt32(text.text.Substring(0,1)) + 1} hit combo!";
        });

        gameObject.SetActive(false);
    }
}
