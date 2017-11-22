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
        //string targetLeg = other.gameObject.GetComponent<Mover> ().TargetLeg;
        string targetTilt = other.gameObject.GetComponent<Mover>().TargetTilt;
        //playerController.TrackLeg (targetLeg);
        playerController.TrackTilting(targetTilt);
		Destroy (other.gameObject);
	}
}
