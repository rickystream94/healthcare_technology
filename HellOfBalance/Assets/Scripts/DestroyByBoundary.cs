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
        string target = other.gameObject.GetComponent<Mover> ().BodyTarget.Target;
        if(target.StartsWith("LEG"))
            playerController.TrackLeg (target);
        else if(target.StartsWith("TILT"))
            playerController.TrackTilting(target);
		Destroy (other.gameObject);
	}
}
