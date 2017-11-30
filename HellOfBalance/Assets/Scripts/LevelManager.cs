using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class LevelManager
    {
        public int Level { get; set; }
        public int FailedAttempts { get; set; }

        private Dictionary<int, int> playTimePerLevel;
        private Dictionary<int, int> maxAttemptsPerLevel;
        private Dictionary<int, float> penaltyWeightPerLevel;
        private const int MAX_LEVEL = 3;

        public LevelManager()
        {
            Level = 1; //By default
            FailedAttempts = 0;

            //Populate levels info (hardcoded for simplicity)
            playTimePerLevel = new Dictionary<int, int>();
            maxAttemptsPerLevel = new Dictionary<int, int>();
            penaltyWeightPerLevel = new Dictionary<int, float>();
            playTimePerLevel.Add(1, 5);
            playTimePerLevel.Add(2, 8);
            playTimePerLevel.Add(3, 10);
            maxAttemptsPerLevel.Add(1, 5);
            maxAttemptsPerLevel.Add(2, 3);
            maxAttemptsPerLevel.Add(3, 2);
            penaltyWeightPerLevel.Add(1, 0.5f);
            penaltyWeightPerLevel.Add(2, 0.35f);
            penaltyWeightPerLevel.Add(3, 0.15f);
        }

        public void LevelUp()
        {
            Level++;
            FailedAttempts = 0;
        }

        public bool HasMoreAttempts()
        {
            int value;
            if (maxAttemptsPerLevel.TryGetValue(Level, out value))
                return FailedAttempts < value;
            else return false;
        }

        public int GetPlayMinutesPerLevel(int level)
        {
            int value;
            if (playTimePerLevel.TryGetValue(level, out value))
                return value;
            return 0;
        }

        public int GetMaxAttemptsPerLevel(int level)
        {
            int value;
            if (maxAttemptsPerLevel.TryGetValue(level, out value))
                return value;
            return 0;
        }

        public bool IsMaxLevelReached()
        {
            return Level > MAX_LEVEL;
        }
    }
}
