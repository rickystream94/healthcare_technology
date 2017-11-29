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
        public int minutes = 0;
        public int seconds = 0;
        public PlayerController playerController;
        private float timeLeft;

        void Awake()
        {
            timeLeft = GetInitialTime();
        }

        void Update()
        {
            if (timeLeft > 0f && playerController.IsPlayerTracked())
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
                    //  The countdown clock has finished
                    timerText.text = "Time : 0:00";
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

    }
}
