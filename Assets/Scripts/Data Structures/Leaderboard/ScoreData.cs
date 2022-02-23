using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public struct ScoreData
{
    // TODO - what do we want to store for score display?
    // do we want to store variables that we use to calculate full score? (ie Hitman-style) <- will do some of these for now

    /// <summary>
    /// Full score achieved by player.
    /// </summary>
    public int fullScore; // calculate fullScore from below variables
    /// <summary>
    /// Time that it took the player to beat the level.
    /// </summary>
    public float levelCompleteTime;
    /// <summary>
    /// Time that it took the player to beat the level.
    /// </summary>
    private float timeBonus;
    /// <summary>
    /// How much damage the player has taken.
    /// </summary>
    public float damageTaken;
    /// <summary>
    /// Times the player has hit the boss.
    /// </summary>
    public int timesBossHit;



    public ScoreData(float levelCompleteTime = 0f, float damageTaken = 0f, int timesBossHit = 0)
    {
        this.levelCompleteTime = levelCompleteTime;
        this.damageTaken = damageTaken;
        this.timesBossHit = timesBossHit;


        fullScore = -1; // TODO - make this actually calculate the full score
        timeBonus = 0;
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
        fullScore = 1; // calculation here 
        levelCompleteTime = (levelCompleteTime == 0 ? 1 : levelCompleteTime);
        timeBonus = (float) (10000f - Math.Round(10000f / levelCompleteTime));
    }

    public List<(string label, string value)> GetDisplayStrings()
    {
        CalculateFullScore();

        List<(string, string)> result = new List<(string, string)>();

        result.Add(("Time Bonus:", timeBonus.ToString()));
        result.Add(("Time Bonus:", timeBonus.ToString()));

        return result;
    }
}
