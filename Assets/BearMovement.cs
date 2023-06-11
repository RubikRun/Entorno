using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : MonoBehaviour
{
    // Speed of horizontal movement
    public float walkingSpeed = 1f;
    public float runningSpeed = 3.4f;

    private Rigidbody2D rigidBody;

    Animator animator;

    GameObject player;
    PlayerMovement playerMovement;

    PlayerHealth playerHealth;

    public int wanderTimeInterval = 500;
    private int wanderTimeToNextState;

    const float wanderRadius = 10f;

    [SerializeField]
    Vector2 initialPosition = Vector2.zero;

    bool seesPlayer = false;

    enum WanderState
    {
        Standing,
        WalkingLeft,
        WalkingRight
    };
    WanderState wanderState = WanderState.Standing;

    float fieldOfView = 7f;

    const float hitDistance = 1.8f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();

        initialPosition = transform.localPosition;

        wanderTimeToNextState = wanderTimeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if bear needs to start/stop seeing the player
        if (seesPlayer)
        {
            if (!isPlayerInFieldOfView())
            {
                seesPlayer = false;
            }
        }
        else
        {
            if (isPlayerInFieldOfView() && !playerMovement.IsSquare())
            {
                seesPlayer = true;
            }
        }
        // Handle horizontal movement
        float horizontalVelocity = getHorizontalVelocity();

        rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
        // Check if bear is walking/running left or right and accordingly flip the bear horizontally
        if (horizontalVelocity > 0)
        {
            transform.localScale = new Vector3
            (
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (horizontalVelocity < 0)
        {
            transform.localScale = new Vector3
            (
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        // Set animator parameters to indicate if bear is currently standing, walking or running
        animator.SetBool("isStanding", Mathf.Approximately(horizontalVelocity, 0f));
        animator.SetBool("isWalking", !Mathf.Approximately(horizontalVelocity, 0f) && Mathf.Abs(horizontalVelocity) < runningSpeed);
        animator.SetBool("isRunning", Mathf.Abs(horizontalVelocity) >= runningSpeed);

        tryHitPlayer();
    }

    bool tryHitPlayer()
    {
        if ((player.transform.position - transform.position).magnitude < hitDistance)
        {
            playerHealth.HitByBear();
            animator.SetBool("isAttacking", true);
            return true;
        }
        animator.SetBool("isAttacking", false);
        return false;
    }

    float getHorizontalVelocity()
    {
        float horizontalMove = getHorizontalMove();
        if (Mathf.Abs(horizontalMove) > 1f)
        {
            return Mathf.Sign(horizontalMove) * runningSpeed;
        }
        return horizontalMove * walkingSpeed;
    }

    float getHorizontalMove()
    {
        if (seesPlayer)
        {
            return runTowardsPlayer();
        }
        return wander();
    }

    bool isPlayerInFieldOfView()
    {
        return (player.transform.position - transform.position).magnitude < fieldOfView;
    }

    float runTowardsPlayer()
    {
        if (player.transform.position.x - transform.position.x > 0f)
        {
            return 2f;
        }
        return -2f;
    }

    float wander()
    {
        if (wanderTimeToNextState <= 0)
        {
            bool doWalkTowardsInitial = false;
            if (Mathf.Abs(initialPosition.x - transform.localPosition.x) > wanderRadius)
            {
                const float probToWalkTowardsInitial = 0.9f;
                doWalkTowardsInitial = (Random.Range(0f, 1f) < probToWalkTowardsInitial);
            }
            if (doWalkTowardsInitial)
            {
                if (initialPosition.x < transform.localPosition.x)
                {
                    wanderState = WanderState.WalkingLeft;
                }
                else
                {
                    wanderState = WanderState.WalkingRight;
                }
            }
            else
            {
                int newStateIndex = Random.Range(0, 3);
                wanderState = (WanderState)newStateIndex;
            }
            wanderTimeToNextState = wanderTimeInterval;
        }
        wanderTimeToNextState--;

        if (wanderState == WanderState.WalkingRight)
        {
            return 1f;
        }
        else if (wanderState == WanderState.WalkingLeft)
        {
            return -1f;
        }

        return 0f;
    }
}
