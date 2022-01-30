using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct LeaderboardEntryData
{
    /// <summary>
    /// The name of the player who achieved the score.
    /// </summary>
    public string playerName;
    /// <summary>
    /// ScoreData of entry.
    /// </summary>
    public ScoreData scoreData;

    public LeaderboardEntryData(string playerName, ScoreData scoreData)
    {
        this.playerName = playerName;
        this.scoreData = scoreData;
    }
}
