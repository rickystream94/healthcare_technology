using Assets.Scripts;
using System.Collections;
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
    public const string RIGHT_LEG_NAME = "LEG_RIGHT";
    public const string LEFT_LEG_NAME = "LEG_LEFT";
    public const string RIGHT_TILT_NAME = "TILT_RIGHT";
    public const string LEFT_TILT_NAME = "TILT_LEFT";

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
    private ColorManager colorManager;

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
        colorManager = ColorManager.Instance;

		StartCoroutine (SpawnHazards ());
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

	IEnumerator SpawnHazards ()
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

	void Fire()
	{
        //Get random hazard
        GameObject hazard = hazards[Random.Range(0, hazards.Length)];

        //Define properties for hazard
        Vector3 spawnPosition = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z - 0.2f);
		Quaternion spawnRotation = Quaternion.identity;

		//Get instantiated object and set TargetLeg variable
		GameObject instance = Instantiate(hazard, spawnPosition, spawnRotation);
		Mover hazardMover = instance.GetComponent<Mover> ();
		SetRendererProperties (instance);
		hazardMover.TargetLeg = GetTargetLeg();

		//Animate
		animator.SetTrigger ("Fire");
	}

	//Used to set the border of the gameObject based on the targeted leg
	void SetRendererProperties(GameObject instance)
	{
		Renderer renderer = instance.GetComponent<Renderer> ();
        Color color;
        colorManager.GetValueFromKey(GetTargetLeg(), out color);
		renderer.material.SetColor ("_OutlineColor", color);
		renderer.material.SetFloat ("_Outline", 0.005f);
	}

	void Move(Vector3 direction)
	{
		isMoving = true;
		movement = direction * speed * Time.deltaTime;
		enemyRigidBody.MovePosition (transform.position + movement);
		Animate(isMoving);
		CheckIfShouldStop ();
	}

	void CheckIfShouldStop()
	{
		movingTimer += Time.deltaTime;
		if (movingTimer >= moveRate) {
			isMoving = false;
		}
	}

	void StayStill()
	{
		waitingTimer += Time.deltaTime;
		isMoving = false;
		Animate (isMoving);

		if (waitingTimer >= currentWaitingTime)
			Restart ();
	}

	void Restart()
	{
		isMoving = true;
		hasDirection = false;
		hasWaitingTime = false;
		waitingTimer = 0f;
		movingTimer = 0f;
		Animate (isMoving);
	}

	void Animate(bool value)
	{
		animator.SetBool ("isMoving", value);
	}

	void PickDirection()
	{		
		Vector3 newDirection = currentDirection == Vector3.left ? Vector3.right : Vector3.left;
		currentDirection = newDirection;
		hasDirection = true;
	}

	void SetWaitingTime()
	{
		hasWaitingTime = true;
		currentWaitingTime = Random.Range (minWaitTime, maxWaitTime);
	}

	public string GetTargetLeg()
	{
		return currentDirection == Vector3.right ? RIGHT_LEG_NAME : LEFT_LEG_NAME;
	}
}
