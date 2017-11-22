using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	private Rigidbody rigidBody;
	private float tumble;
	private float speed;

	public string TargetLeg { get; set; }
    public string TargetTilt { get; set; }

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		speed = 0.5f;
		tumble = 4f;
		rigidBody.angularVelocity = Random.insideUnitSphere * tumble;
	}

	void Update()
	{
		rigidBody.velocity = Vector3.forward * -speed;
	}
}
