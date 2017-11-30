using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyController : MonoBehaviour
{

    public UIController uIController;
    public PlayerController playerController;
    public float fireRate;
    public float enemyMovingRate;
    public GameObject[] hazards;
    public int hazardsPerWave;
    public float playerRestTime;
    public float enemySpeed;
    public float minPlayerActiveTime;
    public float maxPlayerActiveTime;
    public const string LEG_RIGHT = "LEG_RIGHT";
    public const string LEG_LEFT = "LEG_LEFT";
    public const string TILT_RIGHT = "TILT_RIGHT";
    public const string TILT_LEFT = "TILT_LEFT";

    private string Target { get; set; }
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
    void Start()
    {
        waitingTimer = 0f;
        movingTimer = 0f;
        animator = GetComponentInChildren<Animator>();
        enemyRigidBody = GetComponent<Rigidbody>();
        isMoving = false;
        hasDirection = true;
        hasWaitingTime = false;
        currentWaitingTime = 0f;
        currentDirection = Vector3.left;

        StartCoroutine(SpawnHazards());
    }

    void FixedUpdate()
    {
        if (!hasDirection)
            PickDirection();
        if (!hasWaitingTime)
            SetWaitingTime();
        if (isMoving)
            Move(currentDirection);
        else
            StayStill();
    }

    IEnumerator SpawnHazards()
    {
        yield return new WaitForSeconds(enemyMovingRate);
        while (true)
        {
            for (int i = 0; i < hazardsPerWave; i++)
            {
                if (!isMoving && Target != null /*&& playerController.IsPlayerTracked()*/)
                    Fire();
                yield return new WaitForSeconds(fireRate);
            }
            animator.SetTrigger("NewWave");
            yield return new WaitForSeconds(playerRestTime);
        }
    }

    void Fire()
    {
        //Get random hazard
        GameObject hazard = hazards[UnityEngine.Random.Range(0, hazards.Length)];

        //Define properties for hazard
        Vector3 spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f, gameObject.transform.position.z - 0.2f);
        Quaternion spawnRotation = Quaternion.identity;

        //Get instantiated object and set TargetLeg variable
        GameObject instance = Instantiate(hazard, spawnPosition, spawnRotation);
        Mover hazardMover = instance.GetComponent<Mover>();

        BodyTarget bodyTarget = new BodyTarget(Target, instance);
        hazardMover.BodyTarget = bodyTarget;

        //Animate
        animator.SetTrigger("Fire");

        //Update UI
        uIController.UpdateTargetText(bodyTarget);
        uIController.UpdateBodyImage(bodyTarget);
    }

    void Move(Vector3 direction)
    {
        isMoving = true;
        movement = direction * enemySpeed * Time.deltaTime;
        enemyRigidBody.MovePosition(transform.position + movement);
        Animate(isMoving);
        CheckIfShouldStop();
    }

    void CheckIfShouldStop()
    {
        movingTimer += Time.deltaTime;
        if (movingTimer >= enemyMovingRate)
        {
            isMoving = false;
        }
    }

    void StayStill()
    {
        waitingTimer += Time.deltaTime;
        isMoving = false;
        Animate(isMoving);

        if (waitingTimer >= currentWaitingTime)
            Restart();
    }

    void Restart()
    {
        isMoving = true;
        hasDirection = false;
        hasWaitingTime = false;
        waitingTimer = 0f;
        movingTimer = 0f;
        Animate(isMoving);
    }

    void Animate(bool value)
    {
        animator.SetBool("isMoving", value);
    }

    void PickDirection()
    {
        Vector3 newDirection = currentDirection == Vector3.left ? Vector3.right : Vector3.left;
        currentDirection = newDirection;
        hasDirection = true;

        //Equal probability to shoot hazards for legs or upper body
        //Each wave of hazards will have same target
        System.Random random = new System.Random();
        Target = random.NextDouble() > 0.5 ? GetTargetLeg() : GetTargetTilt();
    }

    void SetWaitingTime()
    {
        hasWaitingTime = true;
        currentWaitingTime = UnityEngine.Random.Range(minPlayerActiveTime, maxPlayerActiveTime);
    }

    public string GetTargetLeg()
    {
        return currentDirection == Vector3.right ? LEG_RIGHT : LEG_LEFT;
    }
    private string GetTargetTilt()
    {
        return currentDirection == Vector3.right ? TILT_LEFT : TILT_RIGHT;
    }
}
