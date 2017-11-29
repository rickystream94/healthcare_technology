using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public int userIndex;
    public double thresholdTiltingAngle = 20.0;
    public UIController uIController;
    public GameController gameController;

    private KinectManager kinectManager;
    private const KinectInterop.JointType rightKneeJoint = KinectInterop.JointType.KneeRight;
    private const KinectInterop.JointType leftKneeJoint = KinectInterop.JointType.KneeLeft;
    private const KinectInterop.JointType spineBaseJoint = KinectInterop.JointType.SpineBase;
    private const KinectInterop.JointType neckJoint = KinectInterop.JointType.Neck;
    private bool damaged;

    // Use this for initialization
    void Start()
    {
        kinectManager = KinectManager.Instance;
        damaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            uIController.FlashHitImage();
        }
        else
        {
            uIController.ResetFlashHitImage();
        }
        damaged = false;
    }

    internal bool IsPlayerTracked()
    {
        KinectInterop.BodyData bodyData = kinectManager.GetUserBodyData(kinectManager.GetUserIdByIndex(userIndex));
        return !(bodyData.bIsTracked == 0);
    }

    public void TrackLeg(string targetLeg)
    {
        //Check if body is tracked, otherwise skip
        if (!IsPlayerTracked())
            return;

        KinectInterop.BodyData bodyData = kinectManager.GetUserBodyData(kinectManager.GetUserIdByIndex(userIndex));

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
            case EnemyController.LEG_RIGHT:
                targetLegY = rightKneeY;
                targetJointData = rightKneeJointData;
                break;
            case EnemyController.LEG_LEFT:
                targetLegY = leftKneeY;
                targetJointData = leftKneeJointData;
                break;
            default:
                break;
        }

        if (targetLegY != -100f)
        {
            double distance = LenghtBetweenTwoJoints(targetJointData, spineBaseJointData);
            bool isAvoided = targetLegY >= spineBaseY - (distance / 2);
            if (!isAvoided)
                Hit();
            else
                gameController.IncreaseScore();
            gameController.AddHazard(isAvoided);
        }
    }

    public void TrackTilting(string targetDirection)
    {
        //Check if body is tracked, otherwise skip
        if (!IsPlayerTracked())
            return;

        KinectInterop.BodyData bodyData = kinectManager.GetUserBodyData(kinectManager.GetUserIdByIndex(userIndex));

        KinectInterop.JointData neckJointData = bodyData.joint[(int)neckJoint];
        KinectInterop.JointData spineBaseJointData = bodyData.joint[(int)spineBaseJoint];
        double m = AnglesHelper.AngularCoefficientBetweenTwoJoints(neckJointData, spineBaseJointData);
        double tiltingAngle = AnglesHelper.GetTiltingAngle(m);
        //if tiltingAngle is negative, we're inclining LEFT, otherwise RIGHT
        //Debug.Log("INCLINED: " + tiltingAngle + "°");

        bool isAvoided;
        double minTiltingAngle = 20.0;
        if (targetDirection.Contains("LEFT"))
        {
            minTiltingAngle = -thresholdTiltingAngle;
            isAvoided = tiltingAngle < minTiltingAngle;
        }
        else
        {
            minTiltingAngle = thresholdTiltingAngle;
            isAvoided = tiltingAngle > minTiltingAngle;
        }
        if (!isAvoided)
            Hit();
        else
            gameController.IncreaseScore();
        gameController.AddHazard(isAvoided);
    }

    private void Hit()
    {
        damaged = true;
    }

    public static double LenghtBetweenTwoJoints(KinectInterop.JointData j1, KinectInterop.JointData j2)
    {
        return Math.Sqrt(Math.Pow(j1.position.x - j2.position.x, 2) + Math.Pow(j1.position.y - j2.position.y, 2) + Math.Pow(j1.position.z - j2.position.z, 2));
    }
}
