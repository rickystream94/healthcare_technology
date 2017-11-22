using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ScoreManager
    {
        public int POINTS_PER_MISSED_HAZARD = 10;
        private int score;

        public ScoreManager()
        {
            score = 0;
        }

        public void AddPoints(int points)
        {
            score += points;
        }
    }
}
