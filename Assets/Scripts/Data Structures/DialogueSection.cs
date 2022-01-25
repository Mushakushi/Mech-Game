using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSection
{
    /// <summary>
    /// Text of the section.
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// Seconds to wait between displaying each character. Set to 0 to instantly display.
    /// </summary>
    public float CharacterDelay { get; set; }
    /// <summary>
    /// Overflow the text?
    /// </summary>
    public bool Overflow { get; set; }

    public DialogueSection()
    {
        Overflow = false;
    }
}
