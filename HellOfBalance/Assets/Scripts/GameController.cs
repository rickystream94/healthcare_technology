using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public UIController uIController;
    public ClockController clockController;
    public EnemyController enemyController;
    public bool IsGamePlaying { get; private set; }
    public int TotalPlayedMinutes { get; private set; }

    private ScoreManager scoreManager;
    private LevelManager levelManager;
    private Animator hudCanvasAnimator;

    // Use this for initialization
    void Start()
    {
        scoreManager = new ScoreManager();
        levelManager = new LevelManager();
        hudCanvasAnimator = GameObject.Find("HUDCanvas").GetComponent<Animator>();
        hudCanvasAnimator.SetTrigger("LevelTrigger");
        uIController.UpdateLevelText("Ready to play Level " + levelManager.Level);
        uIController.UpdateScoreText(scoreManager.Score, scoreManager.TotalHazardsShot, scoreManager.TotalAvoidedHazards);
        clockController.RestartTimer(levelManager.GetPlayMinutesPerLevel(levelManager.Level));
        enemyController.PlayerActiveSeconds = levelManager.GetActiveSecondsPerlevel(levelManager.Level);
        IsGamePlaying = true;
        TotalPlayedMinutes = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void IncreaseScore()
    {
        scoreManager.AddPoints();
        //Updates the score text
        uIController.UpdateScoreText(scoreManager.Score, scoreManager.CurrentLevelHazardsShot, scoreManager.CurrentLevelAvoidedHazards);
    }

    internal void CheckLevelScore()
    {
        bool levelPassed = scoreManager.HasPassedLevel();
        TotalPlayedMinutes += levelManager.GetPlayMinutesPerLevel(levelManager.Level);
        if (levelPassed)
        {
            //If the level is passed, we save the score and level up
            scoreManager.AddLevelResult(new LevelResult(levelManager.FailedAttempts, levelManager.GetPenaltyWeightPerLevel(levelManager.Level)));
            LevelUp();
        }
        else
        {
            if (levelManager.HasMoreAttempts())
            {
                //If we have more attempts, let's replay the same level (reset the timer)
                RetryLevel();
            }
            else
            {
                //We have to stop playing (game-over)
                IsGamePlaying = false;
                hudCanvasAnimator.SetTrigger("GameOver");
            }
        }
    }

    private void RetryLevel()
    {
        levelManager.FailedAttempts++;
        scoreManager.ResetCounters();
        int remainingAttempts = levelManager.GetMaxAttemptsPerLevel(levelManager.Level) - levelManager.FailedAttempts;
        hudCanvasAnimator.SetTrigger("LevelTrigger");
        uIController.UpdateLevelText(string.Format("That's not enough, retry!\n\rYou have {0} attempts left!", remainingAttempts));
        clockController.RestartTimer(levelManager.GetPlayMinutesPerLevel(levelManager.Level));
    }

    public void AddHazard(bool avoided)
    {
        scoreManager.AddHazard(avoided);
        //Updates the score text
        uIController.UpdateScoreText(scoreManager.Score, scoreManager.CurrentLevelHazardsShot, scoreManager.CurrentLevelAvoidedHazards);
    }

    public void LevelUp()
    {
        levelManager.LevelUp();
        scoreManager.LevelUp();
        enemyController.PlayerActiveSeconds = levelManager.GetActiveSecondsPerlevel(levelManager.Level);

        //Always check if we reached the max level
        if (levelManager.IsMaxLevelReached())
        {
            IsGamePlaying = false;
            hudCanvasAnimator.SetTrigger("Win");
            CalculateSummaryInfo();
        }
        else
        {
            hudCanvasAnimator.SetTrigger("LevelTrigger");
            uIController.UpdateLevelText("Level Completed!\n\rNow playing Level " + levelManager.Level);
            clockController.RestartTimer(levelManager.GetPlayMinutesPerLevel(levelManager.Level));
        }
    }

    private void CalculateSummaryInfo()
    {
        //Get summary information
        float totalRatioOfSuccess = (float)Math.Round((decimal)(scoreManager.CalculateFinalRatioOfSuccess()), 2);
        float penaltyRatio = (float)Math.Round((decimal)(scoreManager.CalculatePenaltyRatio()), 2);
        float totalRatioOfSuccessPerc = totalRatioOfSuccess * 100;
        float penaltyRatioPerc = penaltyRatio * 100;
        float finalScorePerc = (float)Math.Round((decimal)(totalRatioOfSuccess - totalRatioOfSuccess * penaltyRatio) * 100, 2);
        uIController.ShowSummaryText(scoreManager.Score, TotalPlayedMinutes, totalRatioOfSuccessPerc, penaltyRatioPerc, finalScorePerc);
    }
}
