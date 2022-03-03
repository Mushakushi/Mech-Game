using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossNameText : MonoBehaviour
{
    /// <summary>
    /// Text attached to this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private TMP_Text text;

    public void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        text.text = BattleGroupManager.level.bossName; 
    }
}
