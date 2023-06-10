using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleMovement : MonoBehaviour
{
    public float wanderSpeed = 1f;
    public float chaseSpeed = 3f;

    private Rigidbody2D rigidBody;

    Animator animator;

    GameObject player;

    PlayerHealth playerHealth;

    public int wanderTimeInterval = 500;
    private int wanderTimeToNextState;

    const float wanderRadius = 20f;

    [SerializeField]
    Vector2 initialPosition = Vector2.zero;

    enum WanderState
    {
        WalkingLeft,
        WalkingRight
    };
    WanderState wanderState = WanderState.WalkingRight;

    float fieldOfView = 7f;

    const float hitDistance = 1.4f;

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
        Vector3 velocity = getVelocity();

        rigidBody.velocity = new Vector2(velocity.x, velocity.y);

        if (Mathf.Approximately(rigidBody.velocity.y, 0f))
        {
            transform.localScale = new Vector3
            (
                (rigidBody.velocity.x > 0 ? 1f : -1f) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
            transform.SetLocalPositionAndRotation(
                transform.localPosition,
                Quaternion.identity
            );
        }
        else
        {
            float rotDeg = Mathf.Rad2Deg * (Mathf.Atan2(rigidBody.velocity.y, rigidBody.velocity.x));

            if (rotDeg > -90f && rotDeg < 90f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (rotDeg >= 90f)
                {
                    rotDeg -= 180f;
                }
                else
                {
                    rotDeg += 180f;
                }
            }

            transform.SetLocalPositionAndRotation(
                transform.localPosition,
                Quaternion.Euler(
                    0f,
                    0f,
                    rotDeg
                )
            );
            print(rotDeg);
        }

        tryHitPlayer();
    }

    bool tryHitPlayer()
    {
        Vector3 plPos = player.transform.position;
        Vector3 eaPos = transform.position;
        if ((plPos.x - eaPos.x) * (plPos.x - eaPos.x) + (plPos.y - eaPos.y) * (plPos.y - eaPos.y) < hitDistance * hitDistance)
        {
            playerHealth.HitByEagle();
            return true;
        }
        return false;
    }

    Vector3 getVelocity()
    {
        Vector3 move = getMove();
        if (move.magnitude > 1f)
        {
            return move * chaseSpeed;
        }
        return move * wanderSpeed;
    }

    Vector3 getMove()
    {
        if (isPlayerInFieldOfView())
        {
            return flyTowardsPlayer();
        }
        return wander();
    }

    bool isPlayerInFieldOfView()
    {
        Vector3 plPos = player.transform.position;
        Vector3 eaPos = transform.position;
        return (plPos.x - eaPos.x) * (plPos.x - eaPos.x) + (plPos.y - eaPos.y) * (plPos.y - eaPos.y) < fieldOfView * fieldOfView;
    }

    Vector3 flyTowardsPlayer()
    {
        Vector3 vecToPlayer = player.transform.position - transform.position;
        vecToPlayer.Normalize();
        vecToPlayer *= 2f;
        return vecToPlayer;
    }

    Vector3 wander()
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
                int newStateIndex = Random.Range(0, 2);
                wanderState = (WanderState)newStateIndex;
            }
            wanderTimeToNextState = wanderTimeInterval;
        }
        wanderTimeToNextState--;

        if (wanderState == WanderState.WalkingRight)
        {
            return new Vector3(1f, 0f, 0f);
        }
        else
        {
            return new Vector3(-1f, 0f, 0f);
        }
    }
}
