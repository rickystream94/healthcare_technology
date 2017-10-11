using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public float speed;

	private Rigidbody rigidBody;

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.velocity = transform.forward * speed;
	}
}
