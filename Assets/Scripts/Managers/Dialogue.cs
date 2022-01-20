using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Dialogue
{
    private static int dialogueStage = 0;
    private static List<string> dialogueStrings = new List<string>();
    private static LANGUAGE language = LANGUAGE.English;

    public enum LANGUAGE
    {
        English, TokiPona
    }

    /// <summary>
    /// Set all in-game dialogue to use the language <paramref name="lang"/>.
    /// </summary>
    /// <param name="lang">Language to use.</param>
    public static void SetLanguage(LANGUAGE lang)
    {
        language = lang;
    }

    /// <summary>
    /// Reset dialogue stage and load <paramref name="boss"/> dialogue.
    /// </summary>
    /// <param name="boss"></param>
    public static void InitializeBossDialogue(Boss boss)
    {
        dialogueStage = 0;
        LoadBossDialogue(boss);
    }

    /// <summary>
    /// Load the dialogue of <paramref name="boss"/> from file.
    /// </summary>
    /// <param name="boss">Boss to load dialogue from.</param>
    public static void LoadBossDialogue(Boss boss)
    {
        using (StreamReader sr = new StreamReader($"Assets/Resources/Dialogue/{boss.characterName}/{language}.txt")) 
        {
            while (!sr.EndOfStream)
            {
                dialogueStrings.Add(sr.ReadLine());
            }
        }
    }

    /// <summary>
    /// Get the next line of dialogue.
    /// </summary>
    /// <returns>Next line of dialogue.</returns>
    public static string GetNextLine()
    {
        string nextLine = dialogueStrings[dialogueStage];
        dialogueStage++;
        return nextLine;
    }

    /// <summary>
    /// Get all currently loaded lines of dialogue.
    /// </summary>
    /// <returns>List of lines of dialogue.</returns>
    public static List<string> GetAllCurrentDialogue()
    {
        return dialogueStrings;
    }

    /// <summary>
    /// Display the next line of dialogue on-screen.
    /// </summary>
    public static void DisplayNextDialogue()
    {

    }

}
