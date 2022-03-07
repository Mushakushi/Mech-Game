using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreUtil
{
    private static List<PlayerScore> playerScores = new List<PlayerScore>();

    public static void CreatePlayerScore(int groupNumber)
    {
        playerScores.Add(new PlayerScore(groupNumber));
    }

    public static void AddToPlayerScore(this ScoreData data, int groupNumber)
    {
        playerScores[GetIndex(groupNumber)].scoreData += data;
    }

    public static ScoreData GetPlayerScoreData(int groupNumber)
    {
        foreach (PlayerScore score in playerScores)
        {
            if (score.group == groupNumber)
            {
                return score.scoreData;
            }
        }
        throw new Exception("No score with this group number!");
    }

    public static List<PlayerScore> GetPlayerScores()
    {
        return playerScores;
    }

    public static void ResetPlayerScores()
    {
        playerScores = new List<PlayerScore>();
    }

    private static int GetIndex(int groupNumber)
    {
        for (int i = 0; i < playerScores.Count; i++)
        {
            if (playerScores[i].group == groupNumber)
            {
                return i;
            }
        }
        throw new Exception("No score with this group number!");
    }
}

[Serializable]
public class PlayerScore
{
    public int group;
    public ScoreData scoreData;

    public PlayerScore(int group)
    {
        this.group = group;
        scoreData = new ScoreData();
    }

    public PlayerScore(int group, ScoreData data)
    {
        this.group = group;
        scoreData = data;
    }
}