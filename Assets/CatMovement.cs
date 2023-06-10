using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    // Speed of horizontal movement
    public float walkingSpeed = 1f;
    public float runningSpeed = 3.4f;

    private Rigidbody2D rigidBody;

    Animator animator;

    GameObject player;

    PlayerHealth playerHealth;

    public int wanderTimeInterval = 900;
    private int wanderTimeToNextState;

    const float wanderRadius = 10f;

    [SerializeField]
    Vector2 initialPosition = Vector2.zero;

    enum WanderState
    {
        Standing,
        WalkingLeft,
        WalkingRight
    };
    WanderState wanderState = WanderState.Standing;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        initialPosition = transform.localPosition;

        wanderTimeToNextState = wanderTimeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isBeingPetted"))
        {
            transform.localScale = new Vector3
            (
                (player.transform.position.x - transform.position.x > 0 ? 1f : -1f) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );

            playerHealth.HealByCat();
            return;
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
        return wander();
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

    public void StartPetting()
    {
        animator.SetBool("isBeingPetted", true);
        animator.SetBool("isStanding", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    public void StopPetting()
    {
        animator.SetBool("isBeingPetted", false);
    }
}
