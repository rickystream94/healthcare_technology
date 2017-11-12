using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

	public GameObject explosion;

	void OnTriggerEnter(Collider other)
	{
		Instantiate (explosion, other.transform.position,other.transform.rotation);
		Destroy (other.gameObject);
	}
}
