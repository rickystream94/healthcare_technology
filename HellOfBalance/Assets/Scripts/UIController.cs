using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIController : MonoBehaviour
    {
        public Text targetText;
        public Text infoText;
        public Text scoreText;
        public Image hitImage;
        public float flashSpeed = 5f;
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

        private void Start()
        {
            scoreText.text = "Score: 0";
        }

        private void Update()
        {

        }

        public void FlashHitImage()
        {
            hitImage.color = flashColour;
        }

        public void ResetFlashHitImage()
        {
            hitImage.color = Color.Lerp(hitImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        internal void UpdateTargetText(BodyTarget bodyTarget)
        {
            string target = bodyTarget.Target;
            if (target.Contains("LEG"))
            {
                string whichLeg = target.Split('_')[1];
                targetText.text = "Raise your " + whichLeg + " leg!";
            }
            else if (target.Contains("TILT"))
            {
                string whichDirection = target.Split('_')[1];
                targetText.text = "Tilt your body " + whichDirection + "!";
            }
            targetText.color = bodyTarget.Color;
        }

        internal void UpdateInfoText()
        {

        }

        internal void UpdateScoreText(int points)
        {
            scoreText.text = "Score: " + points;
        }
    }
}
