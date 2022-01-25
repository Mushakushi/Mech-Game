using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DialogueLine
{
    /// <summary>
    /// Sections of this line.
    /// </summary>
    public List<DialogueSection> Sections { get; set; }
    /// <summary>
    /// Name of Character portrait shown when this line is displayed.
    /// </summary>
    public Texture2D Portrait { get; set; }
    /// <summary>
    /// Have the line overflow the text box?
    /// </summary>
    public bool Overflow { get; set; }


    public DialogueLine()
    {
        Sections = new List<DialogueSection>();
        Overflow = false;
    }
}
