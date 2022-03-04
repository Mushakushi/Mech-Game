using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ScoreData
{
    /// <summary>
    /// Full score achieved by player.
    /// </summary>
    public int fullScore = 0; // calculate fullScore from below variables
    /// <summary>
    /// Time that it took the player to beat the level.
    /// </summary>
    public float levelCompleteTime;
    /// <summary>
    /// Points for complete time.
    /// </summary>
    private float timeBonus;
    /// <summary>
    /// How much damage the player has taken.
    /// </summary>
    public int damageTaken;
    /// <summary>
    /// Points for damage taken.
    /// </summary>
    private float damageBonus = 0f;
    /// <summary>
    /// Times the player has hit the boss.
    /// </summary>
    public int timesHitBoss;
    /// <summary>
    /// Points for times player hit the boss.
    /// </summary>
    private float hitBonus = 0f;
    /// <summary>
    /// Times the player tried to hit the boss but was blocked.
    /// </summary>
    public int timesBossBlocked;
    /// <summary>
    /// Points for times player hit the boss.
    /// </summary>
    private float blockPenalty = 0f;



    public ScoreData(float levelCompleteTime = 0f, int damageTaken = 0, int timesHitBoss = 0, int timesBossBlocked = 0)
    {
        this.levelCompleteTime = levelCompleteTime;
        this.damageTaken = damageTaken;
        this.timesHitBoss = timesHitBoss;
        this.timesBossBlocked = timesBossBlocked;
    }


    public static ScoreData operator+(ScoreData a, ScoreData b)
    {
        ScoreData data = new ScoreData();
        data.levelCompleteTime = Math.Max(a.levelCompleteTime, b.levelCompleteTime);
        data.damageTaken = a.damageTaken + b.damageTaken;
        data.timesHitBoss = a.timesHitBoss + b.timesHitBoss;
        data.timesBossBlocked = a.timesBossBlocked + b.timesBossBlocked;
        return data;
    }

    public void CalculateFullScore()
    {
        timeBonus = Math.Max(0f, (float) Math.Ceiling(10000 - (levelCompleteTime < 60 ? 0f : (0.8333f * (levelCompleteTime-60)))));
        damageBonus = Math.Max(0f, damageTaken == 0f ? 15000f : 10000f - (5000f * damageTaken));
        hitBonus = timesHitBoss * 300f;
        blockPenalty = timesBossBlocked * -150f;

        fullScore = (int) Math.Ceiling(timeBonus + damageBonus + hitBonus + blockPenalty); // calculation here 
    }

    public List<(string label, string value)> GetDisplayStrings()
    {
        CalculateFullScore();

        List<(string, string)> result = new List<(string, string)>();

        result.Add(("Level Complete Time:", $"{levelCompleteTime:0.000}s"));
        result.Add(("HP Remaining:", $"{3-damageTaken}HP"));
        result.Add(("Punches Landed:", $"{timesHitBoss}x"));
        result.Add(("Punches Blocked:", $"{timesBossBlocked}x"));

        result.Add(("Time Bonus", $"{timeBonus}"));
        result.Add(("HP Bonus", $"{damageBonus}"));
        result.Add(("Damage Dealt", $"{hitBonus}"));
        result.Add(("Hits Blocked", $"{blockPenalty}"));

        return result;
    }

    public List<(string label, string value)> GetFullScore()
    {
        return new List<(string label, string value)> { ("Full Score", $"{fullScore}") };
    }
}
