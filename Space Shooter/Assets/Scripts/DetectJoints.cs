using UnityEngine;
using System.Collections;
using Windows.Kinect;
using System;

public class DetectJoints : MonoBehaviour 
{
	private KinectSensor _Sensor;
	private BodyFrameReader _Reader;
	private Body[] _Data = null;
	public JointType trackedJoint;
	private float _posX = 0;
	private float _posY = 0;
	private bool _isShooting = false;
	private bool _isCalibrated = false;
	private double wingSpan;

	public Body[] GetData()
	{
		return _Data;
	}


	void Start () 
	{
		Debug.Log ("Kinect ready");
		_Sensor = KinectSensor.GetDefault();

		if (_Sensor != null)
		{
			_Reader = _Sensor.BodyFrameSource.OpenReader();

			if (!_Sensor.IsOpen)
			{
				_Sensor.Open();
			}
		}
	}

	void Update () 
	{
		if (_Reader != null)
		{
			var frame = _Reader.AcquireLatestFrame();
			if (frame != null)
			{
				if (_Data == null)
				{
					_Data = new Body[_Sensor.BodyFrameSource.BodyCount];
				}

				frame.GetAndRefreshBodyData(_Data);

				if (_Data [0].IsTracked && !_isCalibrated) {
					wingSpan = CalculateWingspan (_Data [0]);
					Debug.Log ("Got wingspan: " + wingSpan);
					_isCalibrated = true;
					Debug.Log ("Is Calibrated: " + _isCalibrated);
				}
				TrackJoint ();

				frame.Dispose();
				frame = null;
			}
		}    
	}

	void OnApplicationQuit()
	{
		if (_Reader != null)
		{
			_Reader.Dispose();
			_Reader = null;
		}

		if (_Sensor != null)
		{
			if (_Sensor.IsOpen)
			{
				_Sensor.Close();
			}

			_Sensor = null;
		}
	}

	void TrackJoint() {
		Body body = _Data [0];
		_posX = body.Joints [trackedJoint].Position.X;
		_posY = body.Joints [trackedJoint].Position.Y;

		HandState trackedHandState; //Change hand state according to which hand is being tracked (select from editor)
		if (trackedJoint == JointType.HandLeft)
			trackedHandState = body.HandLeftState;
		else
			trackedHandState = body.HandRightState;
		switch (trackedHandState) {
		case HandState.Closed:
			_isShooting = true;
			break;
		default:
			_isShooting = false;
			break;
		}
		if(_isShooting)
			Debug.Log ("Is shooting!");
	}

	public float GetPosX()
	{
		return this._posX;
	}

	public float GetPosY()
	{
		return this._posY;
	}

	public double GetWingspan()
	{
		return this.wingSpan;
	}

	public bool IsCalibrated()
	{
		return this._isCalibrated;
	}

	public bool IsShooting()
	{
		return this._isShooting;
	}

	public double CalculateWingspan(Body body)
	{
		Windows.Kinect.Joint handRight = body.Joints [JointType.HandRight];
		Windows.Kinect.Joint wristRight = body.Joints [JointType.WristRight];
		Windows.Kinect.Joint elbowRight = body.Joints [JointType.ElbowRight];
		Windows.Kinect.Joint shoulderRight = body.Joints [JointType.ShoulderRight];
		Windows.Kinect.Joint spineShoulder = body.Joints [JointType.SpineShoulder];
		Windows.Kinect.Joint handLeft = body.Joints [JointType.HandLeft];
		Windows.Kinect.Joint wristLeft = body.Joints [JointType.WristLeft];
		Windows.Kinect.Joint elbowLeft = body.Joints [JointType.ElbowLeft];
		Windows.Kinect.Joint shoulderLeft = body.Joints [JointType.ShoulderLeft];
		return Length (handRight, wristRight, elbowRight, shoulderRight, spineShoulder, shoulderLeft, elbowLeft, wristLeft, handLeft);
	}

	public static double Length(params Windows.Kinect.Joint[] joints)
	{
		double length = 0;

		for (int index = 0; index < joints.Length - 1; index++)
		{
			length += LenghtBetweenTwoJoints(joints[index], joints[index + 1]);
		}

		return length;
	}

	public static double LenghtBetweenTwoJoints(Windows.Kinect.Joint j1,Windows.Kinect.Joint j2) {
		return Math.Sqrt (Math.Pow (j1.Position.X - j2.Position.X, 2) + Math.Pow (j1.Position.Y - j2.Position.Y, 2) + Math.Pow (j1.Position.Z - j2.Position.Z, 2));
	}
}
