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

    private Rigidbody2D rigidBody;
    private SunOrbiting sunOrbiting;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("isHuman"))
        {
            UpdateAsHuman();
        }
        else if (animator.GetBool("isBird"))
        {
            UpdateAsBird();
        }
        else
        {
            UpdateAsHuman();
        }
    }

    void UpdateAsHuman()
    {
        rigidBody.gravityScale = 1f;
        transform.SetLocalPositionAndRotation(
            transform.localPosition,
            Quaternion.identity
        );

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
        animator.SetBool("isJumpFlying", Mathf.Abs(rigidBody.velocity.y) <= 10f && !compareFloats(rigidBody.velocity.y, 0f));
    }

    void UpdateAsBird()
    {
        rigidBody.gravityScale = birdGravityScale;

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

    }

    private float fLimitFlying(float x)
    {
        return 1f - Mathf.Clamp01(Mathf.Pow(x, 11f));
    }

    private bool compareFloats(float a, float b, float delta = 0.000001f)
    {
        return (Mathf.Abs(a - b) < delta);
    }
}
