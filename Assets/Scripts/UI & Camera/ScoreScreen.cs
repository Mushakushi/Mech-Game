using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

[RequireComponent(typeof(Scrollbar))]
public class ScoreScreen : MonoBehaviour
{
    private Scrollbar scrollbar;
    private RectTransform panelsTransform;
    [SerializeField] private ScoreData score;
    [SerializeField] private GameObject panels;
    [SerializeField] private GameObject panel;

    void Awake()
    {
        scrollbar = GetComponent<Scrollbar>();
        panelsTransform = panels.GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetScoreValues(new ScoreData(1000f, 10.5f, 1));
    }

    public void SetScoreValues(ScoreData data) // everything in here sucks
    {
        score = data;
        float panelPos = 0; 
        foreach (var display in data.GetDisplayStrings())
        {
            GameObject newPanel = Instantiate(panel, panels.transform);

            newPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panelPos);
            panelPos += -180f;

            TextMeshProUGUI label = newPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>(); //this sucks
            label.text = display.label;

            TextMeshProUGUI value = newPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>(); //this sucks
            value.text = display.value;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        panelsTransform.anchoredPosition = new Vector2(0f, 975f + (-500f * (1 - scrollbar.value)));
    }
}
