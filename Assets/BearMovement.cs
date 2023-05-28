using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : MonoBehaviour
{
    // Speed of horizontal movement
    private float speed = 1f;

    private Rigidbody2D rigidBody;

    Animator animator;

    const int waddleTimeInterval = 900;
    int waddleTimeToNextState = waddleTimeInterval;
    enum WaddleState
    {
        Standing,
        WalkingLeft,
        WalkingRight
    };
    WaddleState waddleState = WaddleState.Standing;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle horizontal movement
        float horizontalMove = getHorizontalMove();
        rigidBody.velocity = new Vector2(horizontalMove * speed, rigidBody.velocity.y);
        // Check if bear is walking/running left or right and accordingly flip the bear horizontally
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
        // Set animator parameter to indicate if bear is currently walking
        animator.SetBool("isWalking", !Mathf.Approximately(horizontalMove, 0f));
    }

    float getHorizontalMove()
    {
        return waddle();
    }

    float waddle()
    {
        if (waddleTimeToNextState <= 0)
        {
            int newStateIndex = Random.Range(0, 3);
            waddleState = (WaddleState)newStateIndex;
            waddleTimeToNextState = waddleTimeInterval;
        }
        waddleTimeToNextState--;

        if (waddleState == WaddleState.WalkingRight)
        {
            return 1f;
        }
        else if (waddleState == WaddleState.WalkingLeft)
        {
            return -1f;
        }

        return 0f;
    }
}
