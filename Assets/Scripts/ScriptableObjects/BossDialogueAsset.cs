using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static DialogueUtil;

[CreateAssetMenu(fileName="[boss name]", menuName="Game Text/BossDialogue")]
public class BossDialogueAsset : ScriptableObject
{
    [SerializeField] public List<TranslatableDialogue> dialogue;
    [SerializeField] public Texture2D defaultPortrait;

    public BossDialogueAsset()
    {
        dialogue = new List<TranslatableDialogue>();
        foreach (LANGUAGE lang in Enum.GetValues(typeof(LANGUAGE)))
        {
            dialogue.Add(new TranslatableDialogue(lang));
        }
    }

    public List<DialogueLine> GetTranslationIn(LANGUAGE lang)
    {
        foreach (TranslatableDialogue translation in dialogue)
        {
            if (translation.language == lang)
            {
                return translation.dialogueLines;
            }
        }
        return new List<DialogueLine>();
    }
}