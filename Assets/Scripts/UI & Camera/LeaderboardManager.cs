using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private LeaderboardData loadedLeaderboard;
    [SerializeField] private GameObject leaderboardObject;

    public void LoadLeaderboard(string bossName)
    {
        //CreateEmptyLeaderboard(bossName);
        loadedLeaderboard = LeaderboardUtil.LoadLeaderboard(bossName);
        DisplayLoadedLeaderboard();
    }

    private LeaderboardData CreateEmptyLeaderboard(string bossName)
    {
        LeaderboardData result = new LeaderboardData(bossName);

        for (int i = 0; i < 11; i++)
        {
            result.TryAdd(new LeaderboardEntryData("ALEXJONES", new ScoreData(10000, 0, 0, 0)));
        }

        return result;      
    }

    private void DisplayLoadedLeaderboard()
    {
        for (int i = 0; i < leaderboardObject.transform.childCount; i++)
        {
            leaderboardObject.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = loadedLeaderboard.entries[i].scoreData.fullScore.ToString();
            leaderboardObject.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = loadedLeaderboard.entries[i].playerName;
        }
    }
}
