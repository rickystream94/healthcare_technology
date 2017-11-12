using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public float speed;

	private Rigidbody rigidbody;
	private float timer;
	private float delay = 0.5f;
	private float rotationSpeed;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		timer = 0f;
		rotationSpeed = 4f;
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= delay)
		{
			rigidbody.velocity = transform.forward * -speed;
			transform.Rotate (0, 0, Random.Range (0, 90) * Time.deltaTime * rotationSpeed);
		}
	}
}
