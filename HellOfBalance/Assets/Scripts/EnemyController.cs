using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float fireRate;
	public float moveRate;
	public GameObject[] hazards;
	public int hazardCount;
	public float waveWait;
	public float speed;
	public float minWaitTime;
	public float maxWaitTime;

	private float waitingTimer;
	private float movingTimer;
	private Animator animator;
	private Rigidbody enemyRigidBody;
	private Vector3 movement;
	private bool isMoving;
	private bool hasDirection;
	private bool hasWaitingTime;
	private Vector3 currentDirection;
	private float currentWaitingTime;

	// Use this for initialization
	void Start () {
		waitingTimer = 0f;
		movingTimer = 0f;
		animator = GetComponentInChildren<Animator> ();
		enemyRigidBody = GetComponent<Rigidbody> ();
		isMoving = false;
		hasDirection = true;
		hasWaitingTime = false;
		currentWaitingTime = 0f;
		currentDirection = Vector3.left;
		StartCoroutine (SpawnWaves ());
	}

	void FixedUpdate()
	{
		if (!hasDirection)
			PickDirection ();
		if (!hasWaitingTime)
			SetWaitingTime ();
		if (isMoving)
			Move (currentDirection);
		else
			StayStill ();
	}

	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds(moveRate);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				if(!isMoving)
					Fire ();
				yield return new WaitForSeconds(fireRate);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	private void Fire()
	{
		//Define properties for hazard
		GameObject hazard = hazards[Random.Range(0, hazards.Length)];
		Vector3 spawnPosition = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z - 0.2f);
		Quaternion spawnRotation = Quaternion.identity;

		//Get instantiated object and set TargetLeg variable
		GameObject instance = Instantiate(hazard, spawnPosition, spawnRotation);
		Mover hazardMover = instance.GetComponent<Mover> ();
		hazardMover.TargetLeg = GetTargetLeg();

		//Animate
		animator.SetTrigger ("Fire");
	}

	private void Move(Vector3 direction)
	{
		isMoving = true;
		movement = direction * speed * Time.deltaTime;
		enemyRigidBody.MovePosition (transform.position + movement);
		Animate(isMoving);
		CheckIfShouldStop ();
	}

	private void CheckIfShouldStop()
	{
		movingTimer += Time.deltaTime;
		if (movingTimer >= moveRate) {
			isMoving = false;
		}
	}

	private void StayStill()
	{
		waitingTimer += Time.deltaTime;
		isMoving = false;
		Animate (isMoving);

		if (waitingTimer >= currentWaitingTime)
			Restart ();
	}

	private void Restart()
	{
		isMoving = true;
		hasDirection = false;
		hasWaitingTime = false;
		waitingTimer = 0f;
		movingTimer = 0f;
		Animate (isMoving);
	}

	private void Animate(bool value)
	{
		animator.SetBool ("isMoving", value);
	}

	private void PickDirection()
	{		
		Vector3 newDirection = currentDirection == Vector3.left ? Vector3.right : Vector3.left;
		currentDirection = newDirection;
		hasDirection = true;
	}

	private void SetWaitingTime()
	{
		hasWaitingTime = true;
		currentWaitingTime = Random.Range (minWaitTime, maxWaitTime);
	}

	public string GetTargetLeg()
	{
		return currentDirection == Vector3.right ? "RIGHT" : "LEFT";
	}
}
