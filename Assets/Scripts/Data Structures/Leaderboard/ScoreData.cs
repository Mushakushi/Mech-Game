using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class ScoreData
{
    // TODO - what do we want to store for score display?
    // do we want to store variables that we use to calculate full score? (ie Hitman-style) <- will do some of these for now

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
    public float damageTaken;
    /// <summary>
    /// Points for damage taken.
    /// </summary>
    private float damageBonus = 0f;
    /// <summary>
    /// Times the player has hit the boss.
    /// </summary>
    public int timesBossHit;
    /// <summary>
    /// Points for times player hit the boss.
    /// </summary>
    private float hitBonus = 0f;



    public ScoreData(float levelCompleteTime = 0f, float damageTaken = 0f, int timesBossHit = 0)
    {
        this.levelCompleteTime = levelCompleteTime;
        this.damageTaken = damageTaken;
        this.timesBossHit = timesBossHit;
    }


    public static ScoreData operator+(ScoreData a, ScoreData b)
    {
        ScoreData data = new ScoreData();
        data.levelCompleteTime = a.levelCompleteTime > b.levelCompleteTime ? a.levelCompleteTime : b.levelCompleteTime;
        data.damageTaken = a.damageTaken + b.damageTaken;
        data.timesBossHit = a.timesBossHit + b.timesBossHit;

        return data;
    }

    public void CalculateFullScore()
    {
        
        timeBonus = Math.Max(0f, (float) Math.Ceiling(10000 - (levelCompleteTime < 60 ? 0f : (0.8333f * (levelCompleteTime-60)))));
        damageBonus = Math.Max(0f, (float) Math.Ceiling(25000 - (5000 * damageTaken)));
        hitBonus = 

        fullScore = (int) Math.Ceiling(timeBonus + damageBonus + hitBonus); // calculation here 
    }

    public List<(string label, string value)> GetDisplayStrings()
    {
        CalculateFullScore();

        List<(string, string)> result = new List<(string, string)>();

        result.Add(("Level Complete Time:", $"{levelCompleteTime:0.00}s"));
        result.Add(("Damage Taken:", $"{damageTaken}HP"));
        result.Add(("Boss Hits:", $"{timesBossHit}x"));

        return result;
    }
}
