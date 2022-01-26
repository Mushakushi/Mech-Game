using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class DialogueUtil
{
    private static List<DialogueLine> loadedDialogue = new List<DialogueLine>();
    private static Boss currentBossLoaded;

    public enum LANGUAGE
    {
        English, TokiPona, Debug
    }

    /// <summary>
    /// Load the dialogue of <paramref name="boss"/> from file.
    /// </summary>
    /// <param name="boss">Boss to load dialogue from.</param>
    /// <param name="language">Language to use.</param>
    public static void LoadBossDialogue(Boss boss, LANGUAGE language)
    {
        currentBossLoaded = boss;
        loadedDialogue = new List<DialogueLine>();
        string filePath = $"Assets/Resources/Dialogue/{boss.characterName}/{language.ToString().ToLower()}.txt";

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    loadedDialogue.Add(ParseLine(sr.ReadLine()));
                }
            }
        }
        catch
        {
            Debug.LogError($"Missing file {filePath}! File load failed.");
        }
    }

    /// <summary>
    /// Parse the <paramref name="line"/> and return a DialogueLine.
    /// </summary>
    /// <param name="line">Line to parse.</param>
    /// <returns>Parsed DialogueLine.</returns>
    private static DialogueLine ParseLine(string line)
    {
        DialogueLine parsedLine = new DialogueLine();

        string[] lineWithArgs = line.Split('|'); // a line with a specified portrait looks like portraitName|<sections>|args
                                                 // a line without looks like |<sections>|args

        string portraitFile = (lineWithArgs[0] != "") ? lineWithArgs[0] : currentBossLoaded.characterName; // use specified portrait or use default if not specified
        parsedLine.Portrait = (Texture2D) Resources.Load($"Art/UI/Portraits/{portraitFile}", typeof(Texture2D));


        string[] sections = lineWithArgs[1].Split('['); // a text section looks like [delay]text

        for (int i = 1; i < sections.Length; i++) // ignore first index - will always be '' (if formatted correctly)
        {
            parsedLine.Sections.Add(ParseSection(sections[i]));
        }

        string[] lineArgs = lineWithArgs[2].Split(',');

        foreach (string arg in lineArgs)
        {
            switch (arg) // can add more - currently only overflow
            {
                case "overflow":
                    parsedLine.Overflow = true;
                    break;
            }
        }

        return parsedLine;
    }

    /// <summary>
    /// Parse the <paramref name="section"/> and return a DialogueSection.
    /// </summary>
    /// <param name="section">Section to parse.</param>
    /// <returns>Parsed DialogueSection.</returns>
    private static DialogueSection ParseSection(string section)
    {
        DialogueSection parsedSection = new DialogueSection();

        string[] textWithDelay = section.Split(']'); // array looks like ["delay", "text"]

        parsedSection.CharacterDelay = (float) Convert.ToDouble(textWithDelay[0]); // don't know how to directly convert to float. this works?
        parsedSection.Text = textWithDelay[1];
        return parsedSection;
    }

    /// <summary>
    /// Get the line of loaded dialogue at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the line.</param>
    /// <returns>DialogueLine at <paramref name="index"/>.</returns>
    public static DialogueLine GetDialogueLine(int index)
    {
        return loadedDialogue[index];
    }
}
