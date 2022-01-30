using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class DialogueUtil
{
    private static List<DialogueLine> loadedDialogue = new List<DialogueLine>();

    public enum LANGUAGE
    {
        English, TokiPona, Debug
    }

    /// <summary>
    /// Load the dialogue of <paramref name="boss"/> from file.
    /// </summary>
    /// <param name="fileName">Name of file to load dialogue from.</param>
    /// <param name="language">Language to use.</param>
    public static void LoadDialogue(string fileName, LANGUAGE language)
    {
        loadedDialogue = new List<DialogueLine>();
        string filePath = $"Assets/Resources/Dialogue/{fileName}/{language.ToString().ToLower()}.txt";

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
    /// Parse the <paramref name="lineFromFile"/> and return a DialogueLine.
    /// </summary>
    /// <param name="lineFromFile">Line to parse.</param>
    /// <returns>Parsed DialogueLine.</returns>
    private static DialogueLine ParseLine(string lineFromFile)
    {
        DialogueLine parsedLine = new DialogueLine();

        string[] splitLineFromFile = lineFromFile.Split('|'); // a line with a specified portrait looks like portraitName|<sections>|args
                                                 // a line without looks like |<sections>|args
        string portraitFileName = (splitLineFromFile[0] != "") ? splitLineFromFile[0] : PhaseManager.level.name; // use specified portrait or use default if not specified
        parsedLine.Portrait = (Texture2D) Resources.Load($"Art/UI/Portraits/{portraitFileName}", typeof(Texture2D));

        string[] dialogueSections = splitLineFromFile[1].Split('['); // a text section looks like [delay]text
        for (int i = 1; i < dialogueSections.Length; i++) // ignore first index - will always be '' (if formatted correctly)
        {
            parsedLine.Sections.Add(ParseSection(dialogueSections[i]));
        }

        string[] lineArgs = splitLineFromFile[2].Split(',');
        foreach (string arg in lineArgs)
        {
            switch (arg) // switch so we can add more if required - currently only overflow
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
