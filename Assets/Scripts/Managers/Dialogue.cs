using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogueUtil;

public class Dialogue : MonoBehaviour
{
    private int dialogueStage = 0;
    private LANGUAGE language = LANGUAGE.English;
    [SerializeField] private TextMeshProUGUI textMeshPro;

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
    private IEnumerator DisplayNextLineCoroutine(DialogueLine line) // TODO: Set portrait using line.Portrait
    {
        textMeshPro.text = "";

        if (line.Overflow)
        {
            textMeshPro.overflowMode = TextOverflowModes.Overflow;
            textMeshPro.enableWordWrapping = false;
        }
        else
        {
            textMeshPro.overflowMode = TextOverflowModes.Truncate;
            textMeshPro.enableWordWrapping = true;
        }

        foreach (DialogueSection section in line.Sections)
        {
            foreach (char letter in section.Text)
            {
                if (section.CharacterDelay > 0) yield return new WaitForSecondsRealtime(section.CharacterDelay);
                textMeshPro.text += letter;
            }
        }
    }
}
