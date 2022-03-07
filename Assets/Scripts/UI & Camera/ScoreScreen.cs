using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class ScoreScreen : MonoBehaviour, IPhaseController
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TMP_InputField nameInput;
    [Space]
    [SerializeField] private RectTransform panelsTransform;
    [SerializeField] private GameObject panelObject;
    [Space]
    [SerializeField] private RectTransform scoresTransform;
    [SerializeField] private GameObject partObject;
    [SerializeField] private GameObject sumObject;

    public int group { get; set; }
    public Phase activePhase => this.GetPhaseFromCollection(new List<Phase> { Phase.ScoreScreen });

    [Space]
    [SerializeField] private ScoreData score;

    public void DisplayScoreData(ScoreData data)
    {
        score = data;
        DisplayScore(data);
    }

    private void DisplayScore(ScoreData data) // it sucks less now but still sucks
    {
        List<(string, string)> displayStrings = data.GetDisplayStrings();
        List<(string, string)> fullScore = data.GetFullScore();

        int halfCount = (displayStrings.Count / 2); // if 4:
        List<(string, string)> stats = displayStrings.GetRange(0, halfCount);
        List<(string, string)> scores = displayStrings.GetRange(halfCount, halfCount);

        DisplayParts(stats, panelObject, panelsTransform, -120f, -180f);
        float linePos = DisplayParts(scores, partObject, scoresTransform, 200f, -130f) - 160f;
        DisplayParts(fullScore, sumObject, scoresTransform, linePos, 0f);
    }

    private float DisplayParts(List<(string label, string value)> displayStrings, GameObject partObject, Transform parent, float startPos, float spacing)
    {
        foreach (var display in displayStrings)
        {
            GameObject newPart = Instantiate(partObject, parent);

            newPart.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, startPos);
            startPos += spacing;

            TextMeshProUGUI label = newPart.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>(); //this sucks
            label.text = display.label;

            TextMeshProUGUI value = newPart.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>(); //this sucks
            value.text = display.value;
        }

        return startPos - spacing;
    }

    public void TryAddInputToLeaderboard(GameObject boss)
    {
        LeaderboardData board = LeaderboardUtil.LoadLeaderboard(boss.GetComponent<Boss>().characterName);
        board.TryAdd(new LeaderboardEntryData(nameInput.text, score));
    }

    // Update is called once per frame
    void Update()
    {
        panelsTransform.anchoredPosition = new Vector2(0f, 975f + (-500f * (1 - Math.Max(0f, scrollbar.value))));
    }

    public void OnStart()
    {

    }

    public void OnPhaseEnter()
    {
        DisplayScoreData(ScoreUtil.GetPlayerScoreData(group));
    }

    public void OnPhaseUpdate()
    {

    }

    public void OnPhaseExit()
    {

    }
}
