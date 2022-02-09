using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentScores : MonoBehaviour
{
    [SerializeField] [ReadOnly] private List<PlayerScore> playerScores = new List<PlayerScore>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerScores = ScoreUtil.GetPlayerScores();
    }
}
