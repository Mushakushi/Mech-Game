using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // commented these out bc i changed the level SO

        //LeaderboardData lob = new Level("Lobstobotomizer").LoadLeaderboard();
        //FillWithEmptyData(lob);
        //LeaderboardData john = new Level("John").LoadLeaderboard();
        //FillWithEmptyData(john);
    }

    private void FillWithEmptyData(LeaderboardData leaderboard) // test method to ensure that functions work
    {
        for (int i = 0; i < 11; i++)
        {
            leaderboard.TryAdd(new LeaderboardEntryData("----", new ScoreData()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
