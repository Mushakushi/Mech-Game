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
    private TimeSpan pauseSpan = TimeSpan.Zero;
    private DateTime pauseStart;

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
        endTime = (DateTime.Now - startTime) - pauseSpan;
        return endTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            if (pauseStart == DateTime.MinValue)
                pauseStart = DateTime.Now;
        }
        else
        {
            if (pauseStart != DateTime.MinValue)
            {
                pauseSpan += DateTime.Now - pauseStart;
                pauseStart = DateTime.MinValue;
            }
            if (endTime == TimeSpan.Zero)
                textbox.text = ((DateTime.Now - startTime) - pauseSpan).ToString(@"mm\:ss\.f");
        }
    }

    public void OnStart()
    {
        StartTimer();
    }

    public void OnPhaseEnter()
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Player_Win: // DO NOT CHANGE!!!
                new ScoreData(levelCompleteTime: (float) StopTimer().TotalMilliseconds / 1000f).AddToPlayerScore(group);
                break;
        }
    }

    public void OnPhaseUpdate()
    {
    }

    public void OnPhaseExit()
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Intro:
                StartTimer();
                break;
        }
    }
}
