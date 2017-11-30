using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class LevelResult
    {
        public int LevelNum { get; set; }
        public int FailedAttempts { get; set; }
        public float RatioOfSuccess { get; set; }
        public int MinutesPlayed { get; set; }

        public LevelResult(int levelNumber, int failedAttempts, float ratioOfSuccess, int minutesPlayed)
        {
            this.LevelNum = levelNumber;
            this.FailedAttempts = failedAttempts;
            this.RatioOfSuccess = ratioOfSuccess;
            this.MinutesPlayed = minutesPlayed;
        }
    }
}
