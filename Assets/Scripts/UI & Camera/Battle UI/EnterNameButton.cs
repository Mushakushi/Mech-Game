using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnterNameButton : MonoBehaviour
{
    /// <summary>
    /// Button text
    /// </summary>
    [SerializeField] private TextMeshProUGUI text;

    /// <summary>
    /// What happens when the player punches
    /// </summary>
    [Header("Normal behaviour")]
    [SerializeField] public UnityEvent onClickNormal = new UnityEvent();

    /// <summary>
    /// What happens when the player punches
    /// </summary>
    [Header("Don't show score behaviour")]
    [SerializeField] public UnityEvent onClickAbnormal = new UnityEvent();

    public void Start()
    {
        if (BattleGroupManager.level.bossName == "MrsSandbag") text.text = "return to menu"; 
    }

    public void OnClick()
    {
        if (BattleGroupManager.level.bossName == "MrsSandbag") onClickAbnormal?.Invoke();
        else onClickNormal?.Invoke();
    }
}
