using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float FIRE_RATE;
	public GameObject[] throwables;

	private float timer;
	private Animator animator;

	// Use this for initialization
	void Start () {
		timer = 0f;
		animator = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= FIRE_RATE)
		{
			animator.SetTrigger ("Fire");
			Quaternion rotation = Quaternion.identity;
			GameObject throwable = throwables[Random.Range(0,throwables.Length)];
			Instantiate (throwable,new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+0.05f,gameObject.transform.position.z-0.1f),rotation);
			timer = 0f;
		}
			
	}
}
