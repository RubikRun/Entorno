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

    Animator animator;

    GameObject water;
    public bool isInWater = false;

    GameObject cats;
    public float catPetRadius = 1f;

    GameObject myCat = null;

    private bool isOnGround = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject sun = GameObject.Find("Sun");

        water = GameObject.Find("Water");
        cats = GameObject.Find("Cats");
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
        UpdateIsInWater();
        // Handle the petting of cats
        if (Input.GetKey(KeyCode.P))
        {
            if (myCat == null)
            {
                GameObject cat = GetClosestCat();
                Vector3 catPos = cat.transform.position;
                float catDist = Mathf.Abs(catPos.x - transform.position.x);
                if (catDist < catPetRadius)
                {
                    animator.SetBool("isPetting", true);
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isJumping", false);
                    animator.SetBool("isFalling", false);
                    animator.SetBool("isJumpFlying", false);

                    CatMovement catMovement = cat.GetComponent<CatMovement>();
                    catMovement.StartPetting();

                    transform.localScale = new Vector3
                    (
                        (catPos.x - transform.position.x > 0 ? 1f : -1f) * Mathf.Abs(transform.localScale.x),
                        transform.localScale.y,
                        transform.localScale.z
                    );

                    myCat = cat;
                }
            }
            return;
        }
        else if (myCat != null)
        {
            animator.SetBool("isPetting", false);

            CatMovement myCatMovement = myCat.GetComponent<CatMovement>();
            myCatMovement.StopPetting();
            myCat = null;
        }
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
        if (Input.GetButtonDown("Jump") && isOnGround)
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

    GameObject GetClosestCat()
    {
        Vector3 playerPos = transform.position;

        GameObject closestCat = null;
        float closestDist = float.PositiveInfinity;

        foreach (Transform catTr in cats.transform)
        {
            Vector3 catPos = catTr.position;
            float catDist = Mathf.Abs(catPos.x - playerPos.x);
            if (catDist < closestDist)
            {
                closestCat = catTr.gameObject;
                closestDist = catDist;
            }
        }

        return closestCat;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    private float fLimitFlying(float x)
    {
        return 1f - Mathf.Clamp01(Mathf.Pow(x, 11f));
    }
}
