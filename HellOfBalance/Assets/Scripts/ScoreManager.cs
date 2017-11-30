using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ScoreManager
    {
        public int basePoints = 10;
        public int Score { get; set; }

        public int TotalHazardsShot { get; set; }
        public int TotalAvoidedHazards { get; set; }
        public int CurrentLevelHazardsShot { get; set; }
        public int CurrentLevelAvoidedHazards { get; set; }

        private float minSuccessRatio = 0.75f;
        private List<LevelResult> levelResults;
        private Dictionary<int, int> failedAttemptsPerLevel;

        public ScoreManager()
        {
            Score = 0;
            TotalHazardsShot = 0;
            TotalAvoidedHazards = 0;
            CurrentLevelAvoidedHazards = 0;
            CurrentLevelHazardsShot = 0;
            levelResults = new List<LevelResult>();
            failedAttemptsPerLevel = new Dictionary<int, int>();
        }

        public void AddPoints()
        {
            Score += basePoints;
        }

        public void AddHazard(bool avoided)
        {
            CurrentLevelHazardsShot++;
            TotalHazardsShot++;
            CurrentLevelAvoidedHazards += avoided ? 1 : 0;
            TotalAvoidedHazards += avoided ? 1 : 0;
        }

        internal bool HasPassedLevel(out float ratioOfSuccess)
        {
            ratioOfSuccess = CurrentLevelAvoidedHazards * 1.0f / CurrentLevelHazardsShot;
            return ratioOfSuccess >= minSuccessRatio;
        }

        internal void AddLevelResult(LevelResult levelResult)
        {
            levelResults.Add(levelResult);
        }

        internal void AddFailedAttempt(int level, int failedAttempts)
        {
            failedAttemptsPerLevel.Add(level, failedAttempts);
        }

        internal void LevelUp()
        {
            CurrentLevelHazardsShot = 0;
            CurrentLevelAvoidedHazards = 0;
        }
    }
}
