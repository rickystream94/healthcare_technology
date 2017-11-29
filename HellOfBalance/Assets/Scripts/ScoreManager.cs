using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ScoreManager
    {
        public int basePoints = 10;
        public int CurrentScore { get; set; }

        public ScoreManager()
        {
            CurrentScore = 0;
        }

        public void AddPoints()
        {
            CurrentScore += basePoints;
        }
    }
}
