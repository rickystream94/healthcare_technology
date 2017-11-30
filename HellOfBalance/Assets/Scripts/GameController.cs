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
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void IncreaseScore()
    {
        scoreManager.AddPoints();
    }

    internal void CheckLevelScore()
    {
        float ratioOfSuccess;
        bool levelPassed = scoreManager.HasPassedLevel(out ratioOfSuccess);
        if (levelPassed)
        {
            //If the level is passed, we save the score and level up
            scoreManager.AddLevelResult(new LevelResult(levelManager.Level, levelManager.FailedAttempts, ratioOfSuccess, levelManager.GetPlayMinutesPerLevel(levelManager.Level)));
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
                //TODO: implement gameover
            }
        }
    }

    private void RetryLevel()
    {
        levelManager.FailedAttempts++;
        int remainingAttempts = levelManager.GetMaxAttemptsPerLevel(levelManager.Level) - levelManager.FailedAttempts;
        hudCanvasAnimator.SetTrigger("LevelTrigger");
        uIController.UpdateLevelText(string.Format("That's not enough, retry!\n\rYou have {0} attempts left!", remainingAttempts));
        clockController.RestartTimer(levelManager.GetPlayMinutesPerLevel(levelManager.Level));
    }

    public void AddHazard(bool avoided)
    {
        scoreManager.AddHazard(avoided);
        //Updates the score text
        uIController.UpdateScoreText(scoreManager.Score, scoreManager.TotalHazardsShot, scoreManager.TotalAvoidedHazards);
    }

    public void LevelUp()
    {
        levelManager.LevelUp();
        scoreManager.LevelUp();

        //Always check if we reached the max level
        if (levelManager.IsMaxLevelReached())
        {
            //TODO: stop playing, calculate the final score and show summary screen
        }
        else
        {
            hudCanvasAnimator.SetTrigger("LevelTrigger");
            uIController.UpdateLevelText("Level Completed!\n\rNow playing Level " + levelManager.Level);
            clockController.RestartTimer(levelManager.GetPlayMinutesPerLevel(levelManager.Level));
        }
    }
}
