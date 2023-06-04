using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimalForms : MonoBehaviour
{
    private SunOrbiting sunOrbiting;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GameObject sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isHuman = (sunOrbiting.days % 3 == 1 && sunOrbiting.hours >= 6)
            || (sunOrbiting.days % 3 == 2 && sunOrbiting.hours < 6);
        bool isBird = (sunOrbiting.days % 3 == 2 && sunOrbiting.hours >= 6)
            || (sunOrbiting.days % 3 == 0 && sunOrbiting.hours < 6);
        bool isFish = (sunOrbiting.days % 3 == 0 && sunOrbiting.hours >= 6)
            || (sunOrbiting.days % 3 == 1 && sunOrbiting.hours < 6);

        bool wasHuman = animator.GetBool("isHuman");
        bool wasBird = animator.GetBool("isBird");
        bool wasFish = animator.GetBool("isFish");

        animator.SetBool("isHuman", isHuman);
        animator.SetBool("isBird", isBird);
        animator.SetBool("isFish", isFish);

        bool isTransforming = (isHuman != wasHuman || isBird != wasBird || isFish != wasFish);
        animator.SetBool("isTransforming", isTransforming);
    }
}
