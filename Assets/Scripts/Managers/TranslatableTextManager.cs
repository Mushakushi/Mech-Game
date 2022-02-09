using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TranslatableTextManager
{
    private static LANGUAGE gameLanguage = LANGUAGE.English;
    private static List<TranslatableTextComponent> textComponents = new List<TranslatableTextComponent>();

    /// <summary>
    /// Get the current game language.
    /// </summary>
    /// <returns>Language being used.</returns>
    public static LANGUAGE GetGameLang()
    {
        return gameLanguage;
    }

    /// <summary>
    /// Set all in-game text to use the language <paramref name="lang"/>.
    /// </summary>
    /// <param name="lang">Language to use.</param>
    public static void SetGameLang(LANGUAGE lang)
    {
        gameLanguage = lang;
    }

    /// <summary>
    /// Register this <paramref name="textComponent"/> for text updates if the language changes.
    /// </summary>
    /// <param name="textComponent"></param>
    public static void Register(this TranslatableTextComponent textComponent)
    {
        textComponents.Add(textComponent);
    }

    /// <summary>
    /// Unregister this <paramref name="textComponent"/> from text updates.
    /// </summary>
    /// <param name="textComponent"></param>
    public static void Unregister(this TranslatableTextComponent textComponent)
    {
        textComponents.Remove(textComponent);
    }

    public static void UpdateAllGameText()
    {
        foreach (TranslatableTextComponent textComponent in textComponents)
        {
            textComponent.UpdateTextToNewLang(gameLanguage);
        }
    }

}

[System.Serializable]
public enum LANGUAGE
{
    English,
    TokiPona,
    Debug
}
