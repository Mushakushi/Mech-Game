using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogueUtil;

public class Dialogue : MonoBehaviour
{
    private int dialogueStage = 0;
    private List<string> dialogueStrings = new List<string>();
    private LANGUAGE language = LANGUAGE.English;
    private TextMeshProUGUI textMeshPro;

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
        dialogueStrings = LoadBossDialogue(boss, language);
        ;
    }

    public void DisplayNextLine()
    {
        string line = GetNextLine();
        StartCoroutine(DisplayNextLineCoroutine(line));
    }

    /// <summary>
    /// Display the next line of dialogue on-screen.
    /// </summary>
    /// <param name="line">Line to display.</param>
    /// <returns></returns>
    private IEnumerator DisplayNextLineCoroutine(string line) // TODO: lots of customization options. Need params - parse first?
    {
        textMeshPro.text = "";
        foreach (char letter in line)
        {
            yield return new WaitForSecondsRealtime(0.05f);
            textMeshPro.text += letter;
        }
    }

    /// <summary>
    /// Get the next line of dialogue.
    /// </summary>
    /// <returns>Next line of dialogue.</returns>
    private string GetNextLine()
    {
        string nextLine = dialogueStrings[dialogueStage];
        dialogueStage++;
        return nextLine;
    }
}
