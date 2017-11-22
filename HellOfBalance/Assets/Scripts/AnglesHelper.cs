using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class AnglesHelper
    {
        public static double AngularCoefficientBetweenTwoJoints(KinectInterop.JointData j1, KinectInterop.JointData j2)
        {
            return (double)(j1.position.y - j2.position.y) / (j1.position.x - j2.position.x);
        }

        public static double GetTiltingAngle(double m)
        {
            double lineAngleRadians = Math.Atan(m);
            double lineAngleDegrees = RadianToDegree(lineAngleRadians);
            //lineAngleDegrees will be either positive or negative
            if (m > 0)
                return 90 - lineAngleDegrees;
            else
                return -(lineAngleDegrees + 90);
        }

        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
