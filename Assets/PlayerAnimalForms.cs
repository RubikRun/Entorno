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
        animator.SetBool("isHuman",
            (sunOrbiting.days % 3 == 1 && sunOrbiting.hours >= 6)
            ||
            (sunOrbiting.days % 3 == 2 && sunOrbiting.hours < 6)
        );
        animator.SetBool("isBird",
            (sunOrbiting.days % 3 == 2 && sunOrbiting.hours >= 6)
            ||
            (sunOrbiting.days % 3 == 0 && sunOrbiting.hours < 6)
        );
        animator.SetBool("isFish",
            (sunOrbiting.days % 3 == 0 && sunOrbiting.hours >= 6)
            ||
            (sunOrbiting.days % 3 == 1 && sunOrbiting.hours < 6)
        );
    }
}
