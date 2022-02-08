using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static DialogueUtil;

[CreateAssetMenu(fileName="language", menuName="Scriptable Object/Dialogue")]
public class BossDialogue : ScriptableObject
{
    [SerializeField] public List<DialogueLine> dialogueLines;
    [SerializeField] public Texture2D defaultPortrait;
}
