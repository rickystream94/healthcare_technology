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
        public Text scoreText;
        public Text levelText;
        public Text gameOverText;
        public Text summaryText;
        public Image hitImage;
        public Image bodyImage;
        public Image shader;
        public Sprite legLeftSprite;
        public Sprite rightLegSprite;
        public Sprite tiltLeftSprite;
        public Sprite tiltRightSprite;
        public float flashSpeed = 10f;
        public Color flashColour = new Color(1f, 0f, 0f, 1f);

        private string scoreFormat = "Score: {0}\n\rTotal Hazards: {1}\n\rAvoided Hazards: {2}";
        private string summaryFormat = "Total Points: {0}\n\rTotal Played Minutes: {1}\n\rSuccess Score: {2}%\n\rPenalty Score: {3}%\n\rFinal Score: {4}%";

        private void Start()
        {
            scoreText.text = string.Format(scoreFormat, 0, 0, 0);
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

        internal void UpdateLevelText(string text)
        {
            levelText.text = text;
        }

        internal void UpdateScoreText(int points, int totalHazards, int avoidedHazards)
        {
            scoreText.text = string.Format(scoreFormat, points, totalHazards, avoidedHazards);
        }

        internal void UpdateBodyImage(BodyTarget bodyTarget)
        {
            Color color = bodyTarget.Color;
            bodyImage.color = color;
            switch (bodyTarget.Target)
            {
                case EnemyController.LEG_LEFT:
                    bodyImage.sprite = legLeftSprite;
                    break;
                case EnemyController.LEG_RIGHT:
                    bodyImage.sprite = rightLegSprite;
                    break;
                case EnemyController.TILT_LEFT:
                    bodyImage.sprite = tiltLeftSprite;
                    break;
                case EnemyController.TILT_RIGHT:
                    bodyImage.sprite = tiltRightSprite;
                    break;
                default:
                    bodyImage.sprite = null;
                    break;
            }
            bodyImage.SetNativeSize();
        }

        internal void ShowSummaryText(int score, int totalPlayedMinutes, float totalRatioOfSuccessPerc, float penaltyRatioPerc, float finalScorePerc)
        {
            string summaryInfo = string.Format(summaryFormat, score, totalPlayedMinutes, totalRatioOfSuccessPerc, penaltyRatioPerc, finalScorePerc);
            summaryText.text = summaryInfo;
        }
    }
}
