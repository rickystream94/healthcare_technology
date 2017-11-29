using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public UIController uIController;
    private ScoreManager scoreManager;

    // Use this for initialization
    void Start()
    {
        scoreManager = new ScoreManager();
    }

    // Update is called once per frame
    void Update()
    {
        //Updates the score text
        uIController.UpdateScoreText(scoreManager.CurrentScore, scoreManager.TotalHazardsShot, scoreManager.AvoidedHazards);
    }

    public void IncreaseScore()
    {
        scoreManager.AddPoints();
    }

    public void AddHazard(bool avoided)
    {
        scoreManager.AddHazard(avoided);
    }
}
