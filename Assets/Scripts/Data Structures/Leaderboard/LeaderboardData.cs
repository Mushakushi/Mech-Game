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
    public Level level;

    public LeaderboardData(List<LeaderboardEntryData> entries, Level level)
    {
        this.entries = entries;
        this.level = level;
    }
}
