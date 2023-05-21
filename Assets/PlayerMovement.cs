using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Speed of horizontal movement
    private float speed = 6f;
    private float jumpPower = 6f;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle horizontal movement
        float horizontalMove = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(horizontalMove * speed, rigidBody.velocity.y);
        // Handle jumping
        if (Input.GetButtonDown("Jump"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpPower);
        }
    }
}
