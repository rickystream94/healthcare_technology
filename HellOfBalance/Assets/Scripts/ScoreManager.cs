using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ScoreManager
    {
        public int basePoints = 10;
        public int Score { get; private set; }

        public int TotalHazardsShot { get; private set; }
        public int TotalAvoidedHazards { get; private set; }
        public int CurrentLevelHazardsShot { get; private set; }
        public int CurrentLevelAvoidedHazards { get; private set; }

        private float minSuccessRatio = 0.75f;
        private int totalAttempts;
        private List<LevelResult> levelResults;
        private Dictionary<int, int> failedAttemptsPerLevel;

        public ScoreManager()
        {
            Score = 0;
            TotalHazardsShot = 0;
            TotalAvoidedHazards = 0;
            CurrentLevelAvoidedHazards = 0;
            CurrentLevelHazardsShot = 0;
            totalAttempts = 0;
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
            CurrentLevelAvoidedHazards += avoided ? 1 : 0;
        }

        internal bool HasPassedLevel()
        {
            totalAttempts++;
            float ratioOfSuccess = CurrentLevelAvoidedHazards * 1.0f / CurrentLevelHazardsShot;
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
            TotalHazardsShot += CurrentLevelHazardsShot;
            TotalAvoidedHazards += CurrentLevelAvoidedHazards;
            ResetCounters();
        }

        public void ResetCounters()
        {
            CurrentLevelHazardsShot = 0;
            CurrentLevelAvoidedHazards = 0;
        }

        internal float CalculateFinalRatioOfSuccess()
        {
            return TotalAvoidedHazards * 1.0f / TotalHazardsShot;
        }

        internal float CalculatePenaltyRatio()
        {
            float weightedFailedAttempts = 0f;
            levelResults.ForEach(x => weightedFailedAttempts += x.FailedAttempts * x.PenaltyWeight);
            float weightSum = 0f;
            levelResults.ForEach(x => weightSum += x.PenaltyWeight);

            //Calculate failed attempts weighted average
            float failedAttemptsWeightedAvg = weightedFailedAttempts / weightSum;

            //Calculate and return penalty ratio
            return failedAttemptsWeightedAvg / totalAttempts;
        }
    }
}
