using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class LevelResult
    {
        public int FailedAttempts { get; private set; }
        public float PenaltyWeight { get; private set; }

        public LevelResult(int failedAttempts, float penaltyWeight)
        {
            this.FailedAttempts = failedAttempts;
            this.PenaltyWeight = penaltyWeight;
        }
    }
}
