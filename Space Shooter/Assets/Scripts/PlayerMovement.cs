using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerMovement : MonoBehaviour {

	public Boundary boundary;
	public float speed;
	public float tilt;
	public DetectJoints bodyManager;
	public float AXIS_MULTIPLIER;
	public float VERTICAL_OFFSET;

	private Rigidbody rigidBody;
	private Vector3 movement;
	private Camera mainCamera;
	private float vertExtent;
	private float horzExtent;

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		mainCamera = Camera.main.GetComponent<Camera>();
	}

	void FixedUpdate()
	{
		//The tracking is ok but there's need of scaling factor
		float h = bodyManager.GetPosX (); //Input.GetAxis ("Horizontal");
		float v = bodyManager.GetPosY(); //Input.GetAxis ("Vertical");
		double wingSpan = bodyManager.GetWingspan();
		float normalizedH = wingSpan != 0 ? h / (float)wingSpan : h;
		movement = new Vector3(normalizedH*AXIS_MULTIPLIER, 0, v*AXIS_MULTIPLIER-VERTICAL_OFFSET);
		rigidBody.velocity = movement * speed;
		rigidBody.position = new Vector3 (
			Mathf.Clamp(rigidBody.position.x,boundary.xMin,boundary.xMax),
			0f,
			Mathf.Clamp(rigidBody.position.z,boundary.zMin,boundary.zMax));
		rigidBody.rotation = Quaternion.Euler (0, 0, rigidBody.velocity.x * -tilt);
	}
	

	void Update () {
		
	}
}
