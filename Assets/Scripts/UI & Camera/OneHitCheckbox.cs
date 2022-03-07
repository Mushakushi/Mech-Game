using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class OneHitCheckbox : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Toggle>().isOn = GlobalSettings.isOneHitMode;
    }
}
