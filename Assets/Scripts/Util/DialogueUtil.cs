using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static TranslatableTextManager;

public static class DialogueUtil
{
    /// <summary>
    /// Load the dialogue of a boss using its name from file.
    /// </summary>
    /// <param name="bossName">Name of boss to load dialogue for.</param>
    public static BossDialogueAsset LoadDialogue(string bossName)
    {
        return FileUtility.LoadFile<BossDialogueAsset>($"{FileUtility.dialoguePath}/{BattleGroupManager.level.bossName}");
    }

    /// <summary>
    /// Get the line of loaded dialogue at <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the line.</param>
    /// <returns>DialogueLine at <paramref name="index"/>.</returns>
    public static DialogueLine GetDialogueLine(this BossDialogueAsset loadedDialogue, int index)
    {
        // needs some error checking for out of bounds, also might want to add support for repeating phrases
        DialogueLine line = loadedDialogue.GetTranslationIn(GetGameLang())[index];
        if (line.portraitOverride == null)
        {
            line.portraitOverride = loadedDialogue.defaultPortrait;
        }

        return line;
    }

    /// <summary>
    /// Get all lines of loaded dialogue.
    /// </summary
    /// <returns>List of all DialogueLines.</returns>
    public static List<DialogueLine> GetAllDialogueLines(this BossDialogueAsset loadedDialogue)
    {
        // needs some error checking for out of bounds, also might want to add support for repeating phrases
        List<DialogueLine> lines = loadedDialogue.GetTranslationIn(GetGameLang());

        return lines;
    }

    #region SUBCLASSES

    [Serializable]
    public struct TranslatableDialogue
    {
        public readonly LANGUAGE language;
        [SerializeField] [ReadOnly] private string languageName;
        [SerializeField] public List<DialogueLine> dialogueLines;

        public TranslatableDialogue(LANGUAGE language)
        {
            this.language = language;
            dialogueLines = new List<DialogueLine>();
            languageName = language.ToString();
        }
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
        /// Voiceline to play with the dialogueline.
        /// </summary>
        public AudioClip voiceline;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sections">Sections in this line.</param>
        /// <param name="portrait">Name of Character portrait shown when this line is displayed.</param>
        /// <param name="overflow">Should the line overflow the text box?</param>
        public DialogueLine(List<DialogueSection> sections, Texture2D portrait, AudioClip voiceline, bool overflow = false)
        {
            portraitOverride = portrait;
            this.overflow = overflow;
            this.sections = sections;
            this.voiceline = voiceline;
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
