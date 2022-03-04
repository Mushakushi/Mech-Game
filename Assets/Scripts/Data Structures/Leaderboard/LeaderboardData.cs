using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct LeaderboardData
{
    /// <summary>
    /// Entries in the leaderboard.
    /// </summary>
    public List<LeaderboardEntryData> entries;
    /// <summary>
    /// The level this leaderboard is for.
    /// </summary>
    public string bossName;

    public LeaderboardData(string bossName)
    {
        entries = new List<LeaderboardEntryData>();
        this.bossName = bossName;
    }

    public LeaderboardData(List<LeaderboardEntryData> entries, string bossName)
    {
        this.entries = entries;
        this.bossName = bossName;
    }
}
