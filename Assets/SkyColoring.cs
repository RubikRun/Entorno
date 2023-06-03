using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyColoring : MonoBehaviour
{
    public Color yellowColor;
    public Color redColor;
    public Color blueColor;

    public float beginHourOfChange = 6f;
    public float endHourOfChange = 7f;

    private SunOrbiting sunOrbiting;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((float)sunOrbiting.hours >= beginHourOfChange && (float)sunOrbiting.hours <= endHourOfChange)
        {
            float t = ((float)sunOrbiting.hours - beginHourOfChange) / (endHourOfChange - beginHourOfChange);
            if (sunOrbiting.days % 3 == 0)
            {
                spriteRenderer.color = t * blueColor + (1f - t) * redColor;
            }
            else if (sunOrbiting.days % 3 == 1)
            {
                spriteRenderer.color = t * yellowColor + (1f - t) * blueColor;
            }
            else
            {
                spriteRenderer.color = t * redColor + (1f - t) * yellowColor;
            }
        }
    }
}
