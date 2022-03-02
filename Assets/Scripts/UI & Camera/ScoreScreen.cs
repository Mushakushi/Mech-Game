using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [Space]
    [SerializeField] private RectTransform panelsTransform;
    [SerializeField] private GameObject panelObject;
    [Space]
    [SerializeField] private RectTransform scoresTransform;
    [SerializeField] private GameObject partObject;
    [Space]
#if UNITY_EDITOR
    [SerializeField] private ScoreData score;
#endif

    void Awake()
    {
    }

    private void Start()
    {
        DisplayScoreData(new ScoreData(78.52f, 2, 18, 5));
    }

    public void DisplayScoreData(ScoreData data)
    {
#if UNITY_EDITOR
        score = data;
#endif
        DisplayScore(data);
    }

    private void DisplayScore(ScoreData data) // it sucks less now but still sucks
    {
        List<(string, string)> displayStrings = data.GetDisplayStrings();
        int halfCount = (displayStrings.Count / 2); // if 4:
        List<(string, string)> stats = displayStrings.GetRange(0, halfCount);
        List<(string, string)> scores = displayStrings.GetRange(halfCount, halfCount);
        DisplayParts(stats, panelObject, panelsTransform, -120f, -180f);
        float linePos = DisplayParts(scores, partObject, scoresTransform, 200f, -130f) - 90f;

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

        return startPos;
    }

    // Update is called once per frame
    void Update()
    {
        panelsTransform.anchoredPosition = new Vector2(0f, 975f + (-500f * (1 - Math.Max(0f, scrollbar.value))));
    }
}
