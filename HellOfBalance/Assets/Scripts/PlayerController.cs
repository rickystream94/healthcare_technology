using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int userIndex;

	private KinectManager kinectManager;
	private const KinectInterop.JointType rightKneeJoint = KinectInterop.JointType.KneeRight;
	private const KinectInterop.JointType leftKneeJoint = KinectInterop.JointType.KneeLeft;
	private const KinectInterop.JointType spineBaseJoint = KinectInterop.JointType.SpineBase;

	// Use this for initialization
	void Start () {
		kinectManager = KinectManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TrackLeg(string targetLeg)
	{
		KinectInterop.BodyData bodyData = kinectManager.GetUserBodyData (kinectManager.GetUserIdByIndex (userIndex));

		//Check if body is tracked, otherwise skip
		if (bodyData.bIsTracked == 0)
			return;

		//Get joints data
		KinectInterop.JointData rightJointData = bodyData.joint [(int)rightKneeJoint];
		KinectInterop.JointData leftJointData = bodyData.joint [(int)leftKneeJoint];
		KinectInterop.JointData spineBaseJointData = bodyData.joint [(int)spineBaseJoint];
		float rightKneeY = rightJointData.position.y;
		float leftKneeY = leftJointData.position.y;
		float spineBaseY = spineBaseJointData.position.y;
		float targetLegY = -100f; //Default value (impossible to reach)
        KinectInterop.JointData targetJointData = rightJointData; //Init default but it will be overwritten (cannot set to null)
		switch (targetLeg) {
		case EnemyController.RIGHT_LEG_NAME:
			targetLegY = rightKneeY;
			targetJointData = rightJointData;
			break;
		case EnemyController.LEFT_LEG_NAME:
			targetLegY = leftKneeY;
			targetJointData = leftJointData;
			break;
		default:
			break;
		}

		if (targetLegY != -100f) {
			double distance = LenghtBetweenTwoJoints (targetJointData, spineBaseJointData);
			if (targetLegY >= spineBaseY - (distance / 2))
				Debug.Log ("AVOIDED");
			else
				Debug.Log ("HIT");
		}
	}

	public static double LenghtBetweenTwoJoints(KinectInterop.JointData j1,KinectInterop.JointData j2) {
		return Math.Sqrt (Math.Pow (j1.position.x - j2.position.x, 2) + Math.Pow (j1.position.y - j2.position.y, 2) + Math.Pow (j1.position.z - j2.position.z, 2));
	}
}
