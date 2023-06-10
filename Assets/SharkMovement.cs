using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    public float wanderSpeed = 1f;
    public float chaseSpeed = 2.5f;

    private Rigidbody2D rigidBody;

    Animator animator;

    GameObject player;

    PlayerHealth playerHealth;

    public int wanderTimeInterval = 800;
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

    const float hitDistance = 1.8f;

    GameObject water;
    public bool isInWater = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerHealth = player.GetComponent<PlayerHealth>();

        initialPosition = transform.localPosition;

        wanderTimeToNextState = wanderTimeInterval;

        water = GameObject.Find("Water");
    }

    // Update is called once per frame
    void Update()
    {
        bool wasInWater = isInWater;
        UpdateIsInWater();
        if (wasInWater != isInWater)
        {
            if (isInWater)
            {
                rigidBody.gravityScale = 0f;
            }
            else
            {
                rigidBody.gravityScale = 1f;
            }
        }

        Vector3 velocity = getVelocity();

        if (isInWater)
        {
            rigidBody.velocity = new Vector2(velocity.x, velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
        }

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
            playerHealth.HitByShark();
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
        if (isPlayerInFieldOfView() && isInWater)
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

        if (wanderState == WanderState.WalkingRight && !IsPositionInWater(transform.position + new Vector3(1f, 0f, 0f)))
        {
            wanderState = WanderState.WalkingLeft;
        }
        else if (wanderState == WanderState.WalkingLeft && !IsPositionInWater(transform.position + new Vector3(-1f, 0f, 0f)))
        {
            wanderState = WanderState.WalkingRight;
        }

        if (wanderState == WanderState.WalkingRight)
        {
            return new Vector3(1f, 0f, 0f);
        }
        else
        {
            return new Vector3(-1f, 0f, 0f);
        }
    }

    void UpdateIsInWater()
    {
        isInWater = IsPositionInWater(transform.position);
    }

    bool IsPositionInWater(Vector3 position)
    {
        foreach (Transform waterChildTr in water.transform)
        {
            GameObject waterChild = waterChildTr.gameObject;
            SpriteRenderer waterChildSR = waterChild.GetComponent<SpriteRenderer>();
            if (waterChildSR.bounds.Contains(position))
            {
                return true;
            }
        }

        return false;
    }
}
