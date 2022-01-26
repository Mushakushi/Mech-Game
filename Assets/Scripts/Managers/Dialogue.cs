using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogueUtil;

public class Dialogue : MonoBehaviour
{
    private int dialogueStage = 0;
    private LANGUAGE language = LANGUAGE.English;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RawImage portrait;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Set all in-game dialogue to use the language <paramref name="lang"/>.
    /// </summary>
    /// <param name="lang">Language to use.</param>
    public void SetLanguage(LANGUAGE lang)
    {
        language = lang;
    }

    /// <summary>
    /// Reset dialogue stage and load <paramref name="boss"/> dialogue.
    /// </summary>
    /// <param name="boss"></param>
    public void InitializeBossDialogue(Boss boss)
    {
        dialogueStage = 0;
        LoadBossDialogue(boss, language);
    }

    /// <summary>
    /// Display the next line of dialogue on-screen.
    /// </summary>
    public void DisplayNextLine()
    {
        DialogueLine line = GetDialogueLine(dialogueStage);
        dialogueStage++;
        StartCoroutine(DisplayNextLineCoroutine(line));
    }

    /// <summary>
    /// Asynchronously display the next line of dialogue on-screen.
    /// </summary>
    /// <param name="line">Line to display.</param>
    /// <returns></returns>
    private IEnumerator DisplayNextLineCoroutine(DialogueLine line)
    {
        textMeshPro.text = "";

        if (line.Overflow) // simplify?
        {
            textMeshPro.overflowMode = TextOverflowModes.Overflow;
            textMeshPro.enableWordWrapping = false;
        }
        else
        {
            textMeshPro.overflowMode = TextOverflowModes.Truncate;
            textMeshPro.enableWordWrapping = true;
        }

        portrait.texture = line.Portrait; // not sure if easier to fix images or make a class containing manual offsets (first one definitely sounds better...)

        animator.SetTrigger("ToggleDialogueShow"); // show dialogue boxes on screen

        yield return new WaitForSecondsRealtime(0.4f); // 0.2s transition to showing dialogue boxes on screen

        foreach (DialogueSection section in line.Sections)
        {
            foreach (char letter in section.Text)
            {
                if (section.CharacterDelay > 0) yield return new WaitForSecondsRealtime(section.CharacterDelay);
                textMeshPro.text += letter;
            }
        }

        yield return new WaitForSecondsRealtime(1.5f); // give time to read text

        animator.SetTrigger("ToggleDialogueShow"); // hide dialogue boxes on screen
    }
}
