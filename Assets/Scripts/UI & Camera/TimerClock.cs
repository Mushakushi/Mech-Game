using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TimerClock : MonoBehaviour, IPhaseController
{
    private TextMeshProUGUI textbox;
    [SerializeField] private DateTime startTime = DateTime.MinValue;
    private TimeSpan endTime = TimeSpan.Zero;

    public int group { get; set; }

    public Phase activePhase => this.GetPhaseFromCollection(new Phase[] { Phase.Intro, Phase.Player_Win });

    // Start is called before the first frame update
    void Awake()
    {
        textbox = GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer()
    {
        startTime = DateTime.Now;
    }

    public TimeSpan StopTimer()
    {
        endTime = (DateTime.Now - startTime);
        return endTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (endTime == TimeSpan.Zero)
            textbox.text = (DateTime.Now - startTime).ToString(@"mm\:ss\.f");
    }

    public void OnStart()
    {

    }

    public void OnPhaseEnter()
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Intro:
                StartTimer();
                break;
            case Phase.Player_Win:
                new ScoreData(levelCompleteTime: (float) StopTimer().TotalMilliseconds).AddToPlayerScore(group);
                break;
        }
    }

    public void OnPhaseUpdate()
    {
    }

    public void OnPhaseExit()
    {
    }
}
