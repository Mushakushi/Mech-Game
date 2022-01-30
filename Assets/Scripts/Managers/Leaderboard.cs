using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeaderboardData lob = new Level(0, "Lobstobotomizer").LoadLeaderboard();
        FillWithEmptyData(lob);
        LeaderboardData john = new Level(1, "John").LoadLeaderboard();
        FillWithEmptyData(john);
    }

    private void FillWithEmptyData(LeaderboardData leaderboard) // test method to ensure that functions work
    {
        for (int i = 0; i < 11; i++)
        {
            leaderboard.TryAdd(new LeaderboardEntryData("----", new ScoreData(10-i, 1f, 0f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
