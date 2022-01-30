using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DialogueUtil;

public class DialogueController : MonoBehaviour, IPhaseController
{
    private int dialogueStage = 0;
    private LANGUAGE language = LANGUAGE.English;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RawImage portrait;

    /// <summary>
    /// Returns Phase.Dialogue_Pre or Phase.Dialogue_Post depending on PhaseManager.phase
    /// </summary>
    public Phase activePhase => new List<Phase> { Phase.Dialogue_Pre, Phase.Dialogue_Post }.GetPhase();

    /// <summary>
    /// Set the language 
    /// </summary>
    public void OnStart()
    {
        SetLanguage(LANGUAGE.TokiPona);
        InitializeBossDialogue(PhaseManager.level.name);
    }

    /// <summary>
    /// Displays the next line of dialogue
    /// </summary>
    public void OnPhaseEnter()
    {
        DisplayNextLine();
    }

    public void OnPhaseUpdate() { }

    public void OnPhaseExit() { }

    /// <summary>
    /// Set all in-game dialogue to use the language <paramref name="lang"/>.
    /// </summary>
    /// <param name="lang">Language to use.</param>
    public void SetLanguage(LANGUAGE lang)
    {
        language = lang;
    }

    /// <summary>
    /// Reset dialogue stage and load <paramref name="fileName"/> dialogue.
    /// </summary>
    public void InitializeBossDialogue(string fileName)
    {
        dialogueStage = 0;
        LoadDialogue(fileName, language);
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

        if (line.overflow) // simplify?
        {
            textMeshPro.overflowMode = TextOverflowModes.Overflow;
            textMeshPro.enableWordWrapping = false;
        }
        else
        {
            textMeshPro.overflowMode = TextOverflowModes.Truncate;
            textMeshPro.enableWordWrapping = true;
        }

        portrait.texture = line.portrait; // not sure if easier to fix images or make a class containing manual offsets (first one definitely sounds better...)

        yield return new WaitForSecondsRealtime(0.4f); // 0.2s transition to showing dialogue boxes on screen

        foreach (DialogueSection section in line.sections)
        {
            foreach (char letter in section.text)
            {
                if (section.characterDelay > 0) yield return new WaitForSecondsRealtime(section.characterDelay);
                textMeshPro.text += letter;
            }
        }

        yield return new WaitForSecondsRealtime(1.5f); // give time to read text

        // Exit phase here? Animator open/close is one of the default phase events
        PhaseManager.ExitPhase();
    }
}
