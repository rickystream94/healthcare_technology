using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts;

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
        double m = AnglesHelper.AngularCoefficientBetweenTwoJoints(neckJointData, spineBaseJointData);
        double tiltingAngle = AnglesHelper.GetTiltingAngle(m);
        //if tiltingAngle is negative, we're inclining LEFT, otherwise RIGHT
        Debug.Log("INCLINED: " + tiltingAngle + "°");

        double minimumTiltingAngle;
        bool isAvoided;
        if(targetDirection.Contains("LEFT"))
        {
            minimumTiltingAngle = -20.0;
            isAvoided = tiltingAngle < minimumTiltingAngle;
        } else
        {
            minimumTiltingAngle = 20.0;
            isAvoided = tiltingAngle > minimumTiltingAngle;
        }
        if (isAvoided)
            Debug.Log("AVOIDED");
        else
            Debug.Log("HIT");
    }

    public static double LenghtBetweenTwoJoints(KinectInterop.JointData j1, KinectInterop.JointData j2)
    {
        return Math.Sqrt(Math.Pow(j1.position.x - j2.position.x, 2) + Math.Pow(j1.position.y - j2.position.y, 2) + Math.Pow(j1.position.z - j2.position.z, 2));
    }
}
