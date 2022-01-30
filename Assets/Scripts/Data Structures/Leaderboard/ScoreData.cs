using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// How much damage the player took.
    /// </summary>
    public float damageTaken;

    /// <summary>
    ///
    /// </summary>
    /// <param name="fullScore">Full score achieved by player.</param>
    /// <param name="levelCompleteTime">Time that it took the player to beat the level.</param>
    /// <param name="damageTaken">How much damage the player took.</param>
    public ScoreData(int fullScore, float levelCompleteTime, float damageTaken)
    {
        this.fullScore = fullScore;
        this.levelCompleteTime = levelCompleteTime;
        this.damageTaken = damageTaken;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelCompleteTime">Time that it took the player to beat the level.</param>
    /// <param name="damageTaken">How much damage the player took.</param>
    public ScoreData(float levelCompleteTime, float damageTaken)
    {
        this.levelCompleteTime = levelCompleteTime;
        this.damageTaken = damageTaken;
        this.fullScore = 0; // TODO - make this actually calculate the full score
    }
}
