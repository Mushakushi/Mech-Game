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
    /// Load the dialogue of <paramref name="boss"/> from file.
    /// </summary>
    /// <param name="boss">Boss to load dialogue from.</param>
    public static void LoadDialogue(Boss boss)
    {
        using (StreamReader sr = new StreamReader($"Assets/Resources/Dialogue/{boss.characterName}/{language}.txt")) 
        {
            while (!sr.EndOfStream)
            {
                dialogueStrings.Add(sr.ReadLine());
            }
        }
    }

    public static string[] ParseText(string text)
    {
        return text.Split('\n');
    }

    public static List<string> GetCurrentDialogue()
    {
        return dialogueStrings;
    }

}
