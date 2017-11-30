using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ClockController : MonoBehaviour
    {
        public Text timerText;
        public PlayerController playerController;
        public GameController gameController;

        private float timeLeft;
        private bool timerOn;
        private int minutes;
        private int seconds;

        void Awake()
        {
            timerOn = false;
            timeLeft = GetInitialTime();
        }

        void Update()
        {
            if (timerOn && timeLeft > 0f /*&& playerController.IsPlayerTracked()*/) //TODO: remove comment to play with kinect
            {
                //  Update countdown clock
                timeLeft -= Time.deltaTime;
                minutes = GetLeftMinutes();
                seconds = GetLeftSeconds();

                //  Show current clock
                if (timeLeft > 0f)
                {
                    timerText.text = "Time : " + minutes + ":" + seconds.ToString("00");
                }
                else
                {
                    //  The countdown clock has finished and it triggers GameController methods
                    timerText.text = "Time : 0:00";
                    timerOn = false;
                    gameController.CheckLevelScore();
                }
            }
        }

        private float GetInitialTime()
        {
            return minutes * 60f + seconds;
        }

        private int GetLeftMinutes()
        {
            return Mathf.FloorToInt(timeLeft / 60f);
        }

        private int GetLeftSeconds()
        {
            return Mathf.FloorToInt(timeLeft % 60f);
        }

        public void RestartTimer(int newMinutes)
        {
            minutes = newMinutes;
            seconds = 0;
            timeLeft = GetInitialTime();
            timerOn = true;
        }
    }
}
