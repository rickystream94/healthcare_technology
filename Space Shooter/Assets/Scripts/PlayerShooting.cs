using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

	public GameObject bolt;
	private AudioSource audio;
	public DetectJoints bodyManager;

	Rigidbody rigidBody;
	public float fireRate;
	float timer;

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
		audio = GetComponent<AudioSource> ();
		//fireRate = .1f;
		timer = 0f;
	}

	void Update () {
		timer += Time.deltaTime;
		if(bodyManager.IsShooting() && timer>=fireRate) {
		//if (Input.GetKey (KeyCode.Space) && timer>=fireRate) {
			timer = 0f;
			Instantiate (bolt, rigidBody.transform.position,rigidBody.transform.rotation);
			audio.Play ();
		}
	}
}
