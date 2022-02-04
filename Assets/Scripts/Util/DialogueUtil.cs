using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public static class DialogueUtil
{
    public static BossDialogue loadedDialogue;

    /// <summary>
    /// Load the dialogue of <paramref name="boss"/> from file.
    /// </summary>
    /// <param name="fileName">Name of file to load dialogue from.</param>
    /// <param name="language">Language to use.</param>
    public static void LoadDialogue(string fileName, LANGUAGE language)
    {
        string filePath = $"Dialogue/{fileName}/{language.ToString().ToLower()}";
        try
        {
            loadedDialogue = (BossDialogue) Resources.Load(filePath, typeof(BossDialogue));
        }
        catch
        {
            loadedDialogue = new BossDialogue();
            Debug.LogError($"Missing file Assets/Resources/{filePath}.txt or file is malformed! File load failed.");
        }
    }

    /// <summary>
    /// Get the line of loaded dialogue at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the line.</param>
    /// <returns>DialogueLine at <paramref name="index"/>.</returns>
    public static DialogueLine GetDialogueLine(int index)
    {
        // needs some error checking for out of bounds, also might want to add support for repeating phrases
        DialogueLine line = loadedDialogue.dialogueLines[index];
        if (line.portraitOverride == null)
        {
            line.portraitOverride = loadedDialogue.defaultPortrait;
        }

        return line;
    }

    #region SUBCLASSES
    [System.Serializable]
    public enum LANGUAGE
    {
        English, 
        [InspectorName("toki pona")]TokiPona, 
        Debug
    }

    [System.Serializable]
    public struct DialogueLine
    {
        /// <summary>
        /// Sections in this line.
        /// </summary>
        public List<DialogueSection> sections;
        /// <summary>
        /// Name of Character portrait shown when this line is displayed.
        /// </summary>
        public Texture2D portraitOverride;
        /// <summary>
        /// Should the line overflow the text box?
        /// </summary>
        public bool overflow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sections">Sections in this line.</param>
        /// <param name="portrait">Name of Character portrait shown when this line is displayed.</param>
        /// <param name="overflow">Should the line overflow the text box?</param>
        public DialogueLine(List<DialogueSection> sections, Texture2D portrait, bool overflow = false)
        {
            this.sections = sections;
            this.portraitOverride = portrait;
            this.overflow = overflow;
        }
    }

    [System.Serializable]
    public struct DialogueSection
    {
        /// <summary>
        /// Text in the section.
        /// </summary>
        public string text;
        /// <summary>
        /// Seconds to wait between displaying each character. Set to 0 to instantly display.
        /// </summary>
        public float characterDelay;

        /// <summary>
        ///
        /// </summary>
        /// <param name="text">Text in the section.</param>
        /// <param name="characterDelay">Seconds to wait between displaying each character. Set to 0 to instantly display.</param>
        /// <param name="overflow">Overflow the text?</param>
        public DialogueSection(string text, float characterDelay)
        {
            this.text = text;
            this.characterDelay = characterDelay;
        }
    }

    #endregion SUBCLASSES
}
