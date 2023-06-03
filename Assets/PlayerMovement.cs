using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Speed of horizontal movement
    public float speed = 6f;
    public float jumpPower = 6f;

    public float birdHorizontalSpeed = 5f;
    public float birdVerticalSpeed = 0.3f;
    public float birdMaxVerticalVelocity = 8f;

    public float birdGravityScale = 0.1f;

    private Rigidbody2D rigidBody;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
            rigidBody.velocity.y + verticalMove * birdVerticalSpeed * fLimitFlying(rigidBody.velocity.y / birdMaxVerticalVelocity)
        );
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
