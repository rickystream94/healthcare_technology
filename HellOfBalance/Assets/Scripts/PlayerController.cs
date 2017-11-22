using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int userIndex;

    private KinectManager kinectManager;
    private const KinectInterop.JointType rightKneeJoint = KinectInterop.JointType.KneeRight;
    private const KinectInterop.JointType leftKneeJoint = KinectInterop.JointType.KneeLeft;
    private const KinectInterop.JointType spineBaseJoint = KinectInterop.JointType.SpineBase;
    private const KinectInterop.JointType neckJoint = KinectInterop.JointType.Neck;

    // Use this for initialization
    void Start()
    {
        kinectManager = KinectManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrackLeg(string targetLeg)
    {
        KinectInterop.BodyData bodyData = kinectManager.GetUserBodyData(kinectManager.GetUserIdByIndex(userIndex));

        //Check if body is tracked, otherwise skip
        if (bodyData.bIsTracked == 0)
            return;

        //Get joints data
        KinectInterop.JointData rightKneeJointData = bodyData.joint[(int)rightKneeJoint];
        KinectInterop.JointData leftKneeJointData = bodyData.joint[(int)leftKneeJoint];
        KinectInterop.JointData spineBaseJointData = bodyData.joint[(int)spineBaseJoint];
        float rightKneeY = rightKneeJointData.position.y;
        float leftKneeY = leftKneeJointData.position.y;
        float spineBaseY = spineBaseJointData.position.y;
        float targetLegY = -100f; //Default value (impossible to reach, used just for initialization)
        KinectInterop.JointData targetJointData = rightKneeJointData; //Init default but it will be overwritten (cannot set to null)
        switch (targetLeg)
        {
            case EnemyController.RIGHT_LEG_NAME:
                targetLegY = rightKneeY;
                targetJointData = rightKneeJointData;
                break;
            case EnemyController.LEFT_LEG_NAME:
                targetLegY = leftKneeY;
                targetJointData = leftKneeJointData;
                break;
            default:
                break;
        }

        if (targetLegY != -100f)
        {
            double distance = LenghtBetweenTwoJoints(targetJointData, spineBaseJointData);
            if (targetLegY >= spineBaseY - (distance / 2))
                Debug.Log("MISSED");
            else
                Debug.Log("HIT");
        }
    }

    public void TrackTilting(string targetDirection)
    {
        KinectInterop.BodyData bodyData = kinectManager.GetUserBodyData(kinectManager.GetUserIdByIndex(userIndex));

        //Check if body is tracked, otherwise skip
        if (bodyData.bIsTracked == 0)
            return;

        KinectInterop.JointData neckJointData = bodyData.joint[(int)neckJoint];
        KinectInterop.JointData spineBaseJointData = bodyData.joint[(int)spineBaseJoint];
        double m = AngularCoefficientBetweenTwoJoints(neckJointData, spineBaseJointData);
        double tiltingAngle = GetTiltingAngle(m, targetDirection);
        //if tiltingAngle is negative, we're inclining LEFT, otherwise RIGHT
        Debug.Log("INCLINED: " + tiltingAngle + "°");
    }

    public static double LenghtBetweenTwoJoints(KinectInterop.JointData j1, KinectInterop.JointData j2)
    {
        return Math.Sqrt(Math.Pow(j1.position.x - j2.position.x, 2) + Math.Pow(j1.position.y - j2.position.y, 2) + Math.Pow(j1.position.z - j2.position.z, 2));
    }

    public static double AngularCoefficientBetweenTwoJoints(KinectInterop.JointData j1, KinectInterop.JointData j2)
    {
        return (double)(j1.position.y - j2.position.y) / (j1.position.x - j2.position.x);
    }

    public static double GetTiltingAngle(double m, string direction)
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
