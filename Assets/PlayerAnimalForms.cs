using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimalForms : MonoBehaviour
{
    private SunOrbiting sunOrbiting;

    private Animator animator;

    public bool isHuman = true;
    public bool isBird = false;
    public bool isFish = false;

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
        bool wasHuman = isHuman;
        bool wasBird = isBird;
        bool wasFish = isFish;

        isHuman = (sunOrbiting.days % 3 == 1 && sunOrbiting.hours >= 6)
            || (sunOrbiting.days % 3 == 2 && sunOrbiting.hours < 6);
        isBird = (sunOrbiting.days % 3 == 2 && sunOrbiting.hours >= 6)
            || (sunOrbiting.days % 3 == 0 && sunOrbiting.hours < 6);
        isFish = (sunOrbiting.days % 3 == 0 && sunOrbiting.hours >= 6)
            || (sunOrbiting.days % 3 == 1 && sunOrbiting.hours < 6);

        animator.SetBool("isHuman", isHuman);
        animator.SetBool("isBird", isBird);
        animator.SetBool("isFish", isFish);

        bool isTransforming = (isHuman != wasHuman || isBird != wasBird || isFish != wasFish);
        animator.SetBool("isTransforming", isTransforming);
    }
}
