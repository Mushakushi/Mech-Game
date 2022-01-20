using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class DialogueUtil
{
    

    public enum LANGUAGE
    {
        English, TokiPona
    }

    /// <summary>
    /// Load the dialogue of <paramref name="boss"/> from file.
    /// </summary>
    /// <param name="boss">Boss to load dialogue from.</param>
    /// <param name="language">Language to use.</param>
    public static List<string> LoadBossDialogue(Boss boss, LANGUAGE language)
    {
        List<string> dialogueStrings = new List<string>();
        using (StreamReader sr = new StreamReader($"Assets/Resources/Dialogue/{boss.characterName}/{language}.txt"))
        {
            while (!sr.EndOfStream)
            {
                dialogueStrings.Add(sr.ReadLine());
            }
        }
        return dialogueStrings;
    }
}
