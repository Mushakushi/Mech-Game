using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
