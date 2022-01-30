using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogueLine
{
    /// <summary>
    /// Sections in this line.
    /// </summary>
    public List<DialogueSection> sections;
    /// <summary>
    /// Name of Character portrait shown when this line is displayed.
    /// </summary>
    public Texture2D portrait;
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
        this.portrait = portrait;
        this.overflow = overflow;
    }
}
