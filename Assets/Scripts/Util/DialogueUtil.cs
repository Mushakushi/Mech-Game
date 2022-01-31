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
        string filePath = $"Dialogue/{fileName}/{language.ToString().ToLower()}";

        try
        {
            TextAsset dialogueFile = (TextAsset) Resources.Load(filePath, typeof(TextAsset));
            foreach (string line in dialogueFile.text.Split('\n'))
            {
                loadedDialogue.Add(ParseLine(line));
            }
        }
        catch
        {
            Debug.LogError($"Missing file Assets/Resources/{filePath}.txt or file is malformed! File load failed.");
        }
    }

    /// <summary>
    /// Parse the <paramref name="lineFromFile"/> and return a DialogueLine.
    /// </summary>
    /// <param name="lineFromFile">Line to parse.</param>
    /// <returns>Parsed DialogueLine.</returns>
    private static DialogueLine ParseLine(string lineFromFile)
    {
        string[] splitLineFromFile = lineFromFile.Split('|'); // a line with a specified portrait looks like portraitName|<sections>|args
                                                              // a line without looks like |<sections>|args

        string portraitFileName = (splitLineFromFile[0] != "") ? splitLineFromFile[0] : BattleGroupManager.level.name; // use specified portrait or use default if not specified
        Texture2D portrait = (Texture2D) Resources.Load($"Art/UI/Portraits/{portraitFileName}", typeof(Texture2D));

        List<DialogueSection> sections = new List<DialogueSection>();

        string[] dialogueSections = splitLineFromFile[1].Split('['); // a text section looks like [delay]text
        for (int i = 1; i < dialogueSections.Length; i++) // ignore first index - will always be '' (if formatted correctly)
        {
            sections.Add(ParseSection(dialogueSections[i]));
        }

        DialogueLine dialogueLine = new DialogueLine(sections, portrait);

        string[] lineArgs = splitLineFromFile[2].Split(',');
        foreach (string arg in lineArgs)
        {
            switch (arg) // switch so we can add more if required - currently only overflow
            {
                case "overflow":
                    dialogueLine.overflow = true;
                    break;
            }
        }

        return dialogueLine;
    }

    /// <summary>
    /// Parse the <paramref name="section"/> and return a DialogueSection.
    /// </summary>
    /// <param name="section">Section to parse.</param>
    /// <returns>Parsed DialogueSection.</returns>
    private static DialogueSection ParseSection(string section)
    {
        string[] textWithDelay = section.Split(']'); // array looks like ["delay", "text"]

        float delay = (float) Convert.ToDouble(textWithDelay[0]); // don't know how to directly convert to float. this works?
        string text = textWithDelay[1];

        return new DialogueSection(text, delay);
    }

    /// <summary>
    /// Get the line of loaded dialogue at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the line.</param>
    /// <returns>DialogueLine at <paramref name="index"/>.</returns>
    public static DialogueLine GetDialogueLine(int index)
    {
        // needs some error checking for out of bounds, also might want to add support for repeating phrases
        return loadedDialogue[index];
    }
}
