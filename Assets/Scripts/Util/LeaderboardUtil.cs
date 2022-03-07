using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;

public static class LeaderboardUtil
{
    /// <summary>
    /// Universal maximum size for leaderboards.
    /// </summary>
    public static readonly int leaderboardMaxSize = 10;

    /// <summary>
    /// Load the leaderboard for <paramref name="level"/> from file.
    /// </summary>
    /// <param name="level">Level of the leaderboard to load.</param>
    public static LeaderboardData LoadLeaderboard(string name)
    {
        string assetPath = GetLeaderboardFilePath(name);
        try
        {
            List<string> leaderboardFromFile = File.ReadAllLines(assetPath).ToList();
            return DeserializeLeaderboard(leaderboardFromFile, name);
        }
        catch
        {
            Debug.LogError($"Missing leaderboard file! Creating a new empty file.");
            if (!Directory.Exists(GetLeaderboardFilesDirectory()))
            {
                Directory.CreateDirectory(GetLeaderboardFilesDirectory());
            }
            File.Create($"{assetPath}").Dispose();
            LeaderboardData newData = new LeaderboardData(CreateEmptyLeaderboard(), name);
            newData.SaveToFile();
            return newData;
        }
    }

    /// <summary>
    /// Get the persistent data path to the leaderboard for <paramref name="level"/>.
    /// </summary>
    /// <param name="level">Level the leaderboard is for.</param>
    /// <returns> Persistent file path to leaderboard file.</returns>
    private static string GetLeaderboardFilePath(string name)
    {
        return $"{GetLeaderboardFilesDirectory()}/{name.ToLower()}.csv";
    }

    /// <summary>
    /// Get the persistent directory of leaderboard files.
    /// </summary>
    /// <returns>Persistent directory of leaderboard files.</returns>
    private static string GetLeaderboardFilesDirectory()
    {
        return $"{Application.persistentDataPath}/Leaderboards";
    }

    /// <summary>
    /// Structure leaderboard data from .csv file text.
    /// </summary>
    /// <param name="textFromFile">Full text from .csv file.</param>
    /// <param name="name">Level this leaderboard is for.</param>
    /// <returns>Data in form of LeaderboardData.</returns>
    private static LeaderboardData DeserializeLeaderboard(List<string> textFromFile, string name)
    {
        List<LeaderboardEntryData> entriesData = new List<LeaderboardEntryData>();

        for (int i = 0; i < textFromFile.Count; i++)
        {
            List<string> entryDataText = textFromFile[i].Split(',').ToList();

            List<string> scoreDataText = new List<string>(entryDataText);
            scoreDataText.RemoveAt(0);

            entriesData.Add(new LeaderboardEntryData(entryDataText[0], DeserializeScore(scoreDataText))); // probably fine to hardcode here
        }

        return new LeaderboardData(entriesData, name);
    }

    /// <summary>
    /// Structure score data from a split .csv string.
    /// </summary>
    /// <param name="scoreDataText">List of values in a line from .csv file.</param>
    /// <returns>Data in the form of ScoreData.</returns>
    private static ScoreData DeserializeScore(List<string> scoreDataText)
    {
        object scoreData = new ScoreData();

        var fields = typeof(ScoreData).GetFields();
        for (int i = 0; i < scoreDataText.Count; i++)
        {
            switch (fields[i].GetValue(scoreData)) // hard work-around to not hard-code. if there are more types, we'll have to add parse functions here.
            {
                case int t1:
                    fields[i].SetValue(scoreData, int.Parse(scoreDataText[i]));
                    break;
                case float t2:
                    fields[i].SetValue(scoreData, float.Parse(scoreDataText[i]));
                    break;
            }
        }

        return (ScoreData) scoreData;
    }

    /// <summary>
    /// Save LeaderboardData to file as "{<paramref name="leaderboard"/>.level.name}.csv".
    /// </summary>
    /// <param name="leaderboard">Leaderboard to save to file.</param>
    public static void SaveToFile(this LeaderboardData leaderboard)
    {
        string assetPath = GetLeaderboardFilePath(leaderboard.bossName);
        using (StreamWriter sw = new StreamWriter($"{assetPath}"))
        {
            foreach (LeaderboardEntryData entry in leaderboard.entries)
            {
                sw.WriteLine($"{entry.playerName},{SerializeScoreData(entry.scoreData)}");
            }
        }
    }

    /// <summary>
    /// Create a csv string from ScoreData.
    /// </summary>
    /// <param name="scoreData">Data to serialize.</param>
    /// <returns>Serialized csv string.</returns>
    private static string SerializeScoreData(ScoreData scoreData)
    {
        string serializedData = "";

        var fields = typeof(ScoreData).GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            serializedData += fields[i].GetValue(scoreData);
            if (i != fields.Length-1)
            {
                serializedData += ",";
            }
        }

        return serializedData;
    }

    private static List<LeaderboardEntryData> CreateEmptyLeaderboard()
    {
        List<LeaderboardEntryData> output = new List<LeaderboardEntryData>();
        for (int i = 0; i < leaderboardMaxSize; i++)
        {
            output.Add(new LeaderboardEntryData("YOUR NAME HERE!", new ScoreData(float.MaxValue, 3, 0, 0)));
        }
        return output;
    }


    /// <summary>
    /// Try to add a <paramref name="newEntry"/> to <paramref name="leaderboard"/>.
    /// </summary>
    /// <param name="leaderboard">Leaderboard to add new entry to.</param>
    /// <param name="newEntry">Entry to try to add to the leaderboard.</param>
    /// <returns>True if successful, False if not.</returns>
    public static bool TryAdd(this LeaderboardData leaderboard, LeaderboardEntryData newEntry)
    {
        newEntry.scoreData.CalculateFullScore();
        if (leaderboard.entries.Count == 0)
        {
            leaderboard.entries.Add(newEntry);
            leaderboard.SaveToFile();
            return true;
        }

        for (int i = 0; i < leaderboard.entries.Count; i++)
        {
            LeaderboardEntryData currentEntry = leaderboard.entries[i];
            if (newEntry.scoreData.fullScore >= currentEntry.scoreData.fullScore) // >= is to give priority to newer scores if equal
            {
                leaderboard.entries.Insert(i, newEntry);

                if (leaderboard.entries.Count > leaderboardMaxSize)
                {
                    leaderboard.entries.RemoveAt(leaderboardMaxSize);
                }

                leaderboard.SaveToFile();
                return true;
            }                                                                                                                      
            else if (i == leaderboard.entries.Count - 1 && leaderboard.entries.Count < leaderboardMaxSize)
            {
                leaderboard.entries.Add(newEntry);
                leaderboard.SaveToFile();
                return true;
            }
        }
        return false;
    }
}
