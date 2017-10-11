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
	public float MULTIPLIER;

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
		movement = new Vector3(h*MULTIPLIER, 0, v*MULTIPLIER);
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
