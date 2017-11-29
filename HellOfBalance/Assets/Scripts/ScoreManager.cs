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

        public int TotalHazardsShot { get; set; }
        public int AvoidedHazards { get; set; }

        public ScoreManager()
        {
            CurrentScore = 0;
            TotalHazardsShot = 0;
            AvoidedHazards = 0;
        }

        public void AddPoints()
        {
            CurrentScore += basePoints;
        }

        public void AddHazard(bool avoided)
        {
            TotalHazardsShot++;
            AvoidedHazards += avoided ? 1 : 0;
        }
    }
}
