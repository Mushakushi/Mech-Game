using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TranslatableTextManager;

[CreateAssetMenu(fileName = "[text field name]", menuName = "Game Text/TranslatableText")]
public class TranslatableTextAsset : ScriptableObject
{
    public List<TranslationText> languages;

    public TranslatableTextAsset()
    {
        languages = new List<TranslationText>();
        foreach (LANGUAGE lang in Enum.GetValues(typeof(LANGUAGE)))
        {
            languages.Add(new TranslationText(lang, ""));
        }
    }

    public string GetTranslationIn(LANGUAGE lang)
    {
        foreach (TranslationText translation in languages)
        {
            if (translation.language == lang)
            {
                return translation.text;
            }
        }
        return "translation missing";
    }
}

[System.Serializable]
public class TranslationText
{
    public readonly LANGUAGE language;
    #if UNITY_EDITOR
    [SerializeField] [ReadOnly] private string languageName;
    #endif
    public string text;

    public TranslationText(LANGUAGE language, string text)
    {
        this.language = language;
        this.text = text;
        languageName = language.ToString();
    }
}