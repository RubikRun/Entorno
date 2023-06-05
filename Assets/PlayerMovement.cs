using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Speed of horizontal movement
    public float speed = 6f;
    public float jumpPower = 6f;

    public float birdHorizontalSpeed = 8f;
    public float birdVerticalSpeed = 0.2f;
    public float birdMaxVerticalVelocity = 8f;

    public float birdGravityScale = 0.3f;

    public float fishJumpPower = 2f;
    public int fishTimeBetweenJumps = 200;
    private int fishTimeSinceLastJump = 9999999;

    private Rigidbody2D rigidBody;
    private SunOrbiting sunOrbiting;

    Animator animator;

    GameObject water;
    public bool isInWater = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();

        water = GameObject.Find("Water");
    }

    // Update is called once per frame
    void Update()
    {
        bool isTransforming = animator.GetBool("isTransforming");

        if (animator.GetBool("isHuman"))
        {
            if (isTransforming)
            {
                InitHumanForm();
            }
            UpdateAsHuman();
        }
        else if (animator.GetBool("isBird"))
        {
            if (isTransforming)
            {
                InitBirdForm();
            }
            UpdateAsBird();
        }
        else
        {
            if (isTransforming)
            {
                InitFishForm();
            }
            UpdateAsFish();
        }
    }

    void InitHumanForm()
    {
        rigidBody.gravityScale = 1f;
        transform.SetLocalPositionAndRotation(
            transform.localPosition,
            Quaternion.identity
        );
    }

    void InitBirdForm()
    {
        rigidBody.gravityScale = birdGravityScale;
    }

    void InitFishForm()
    {
        UpdateIsInWater();
        if (isInWater)
        {
            InitBirdForm();
        }
        else
        {
            rigidBody.gravityScale = 1f;
            transform.SetLocalPositionAndRotation(
                transform.localPosition,
                Quaternion.Euler(
                    0f,
                    0f,
                    90f
                )
            );
        }
    }

    void UpdateAsHuman()
    {
        // Handle horizontal movement
        float horizontalMove = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizontalMove * speed, rigidBody.velocity.y);
        // Check if player is running left or right and accordingly flip the player horizontally
        if (horizontalMove > 0)
        {
            transform.localScale = new Vector3
            (
                Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        else if (horizontalMove < 0)
        {
            transform.localScale = new Vector3
            (
                -Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
        // Set animator parameter to indicate if player is currently running
        animator.SetBool("isRunning", !Mathf.Approximately(horizontalMove, 0f));
        // Handle jumping
        if (Input.GetButtonDown("Jump"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        animator.SetBool("isFalling", Mathf.Abs(rigidBody.velocity.y) > 10f);
        animator.SetBool("isJumpFlying", Mathf.Abs(rigidBody.velocity.y) <= 10f && !Mathf.Approximately(rigidBody.velocity.y, 0f));
    }

    void UpdateAsBird()
    {
        // Handle horizontal movement
        float horizontalMove = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizontalMove * birdHorizontalSpeed, rigidBody.velocity.y);
        // Handle vertical movement
        float verticalMove = Input.GetAxis("Vertical");
        rigidBody.velocity = new Vector2(
            rigidBody.velocity.x,
            Mathf.Clamp(
                rigidBody.velocity.y + verticalMove * birdVerticalSpeed
                    * fLimitFlying(rigidBody.velocity.y / birdMaxVerticalVelocity),
                -birdMaxVerticalVelocity,
                birdMaxVerticalVelocity
            )
        );

        if (Mathf.Approximately(rigidBody.velocity.x, 0f) && Mathf.Approximately(rigidBody.velocity.y, 0f))
        {
            transform.SetLocalPositionAndRotation(
                transform.localPosition,
                Quaternion.identity
            );
        }
        else
        {
            transform.SetLocalPositionAndRotation(
                transform.localPosition,
                Quaternion.Euler(
                    0f,
                    0f,
                    Mathf.Rad2Deg * (Mathf.Atan2(rigidBody.velocity.y, rigidBody.velocity.x) - Mathf.PI / 2f)
                )
            );
        }
    }

    void UpdateAsFish()
    {
        bool wasInWater = isInWater;
        UpdateIsInWater();
        if (wasInWater != isInWater)
        {
            InitFishForm();
        }

        if (isInWater)
        {
            UpdateAsBird();
        }
        else
        {
            UpdateAsFishOutOfWater();
        }
    }

    void UpdateAsFishOutOfWater()
    {
        // Handle fish dry jumping
        float horizontalMove = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && Mathf.Abs(horizontalMove) >= 1f && fishTimeSinceLastJump > fishTimeBetweenJumps)
        {
            rigidBody.velocity = new Vector2(fishJumpPower * horizontalMove, fishJumpPower);
            fishTimeSinceLastJump = 0;
        }
        fishTimeSinceLastJump++;
    }

    void UpdateIsInWater()
    {
        isInWater = false;
        foreach (Transform waterChildTr in water.transform)
        {
            GameObject waterChild = waterChildTr.gameObject;
            SpriteRenderer waterChildSR = waterChild.GetComponent<SpriteRenderer>();
            if (waterChildSR.bounds.Contains(transform.position))
            {
                isInWater = true;
                break;
            }
        }

        animator.SetBool("inWater", isInWater);
    }

    private float fLimitFlying(float x)
    {
        return 1f - Mathf.Clamp01(Mathf.Pow(x, 11f));
    }
}
