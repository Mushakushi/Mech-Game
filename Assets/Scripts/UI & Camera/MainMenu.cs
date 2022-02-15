using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject battleGroupPrefab;
    [SerializeField] private GameObject menuUI;

    public void StartButton_Clicked()
    {
        GameObject newBattleGroup = Instantiate(battleGroupPrefab);
        BattleGroupManager.AddRuntimePhaseManager(newBattleGroup.GetComponent<PhaseManager>());
        newBattleGroup.SetActive(true);
        menuUI.SetActive(false);
    }
}
