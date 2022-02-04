using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private BattleGroupManager battleGroupManager;
    [SerializeField] private GameObject battleGroupPrefab;
    [SerializeField] private GameObject menuUI;

    void Awake()
    {
        battleGroupManager = GetComponent<BattleGroupManager>();
    }

    public void StartButton_Clicked()
    {
        GameObject newBattleGroup = Instantiate(battleGroupPrefab);
        battleGroupManager.AddPhaseManager(newBattleGroup.GetComponent<PhaseManager>());
        newBattleGroup.SetActive(true);
        menuUI.SetActive(false);
    }
}
