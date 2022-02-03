using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject battleGroupPrefab;
    [SerializeField] private GameObject menuUI;

    public void StartButton_Clicked()
    {
        Instantiate(battleGroupPrefab);
        menuUI.SetActive(false);
    }
}
