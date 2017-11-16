using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

	private PlayerController playerController;

	void Start()
	{
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
	}

	void OnTriggerEnter(Collider other)
	{
		string targetLeg = other.gameObject.GetComponent<Mover> ().TargetLeg;
		playerController.TrackLeg (targetLeg);
		Destroy (other.gameObject);
	}
}
