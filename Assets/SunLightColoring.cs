using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunLightColoring : MonoBehaviour
{
    public Color yellowColor;
    public Color redColor;
    public Color blueColor;

    private SunOrbiting sunOrbiting;

    private Light2D lightComp;
    // Start is called before the first frame update
    void Start()
    {
        lightComp = GetComponent<Light2D>();
        GameObject sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sunOrbiting.days % 3 == 0)
        {
            lightComp.color = blueColor;
        }
        else if (sunOrbiting.days % 3 == 1)
        {
            lightComp.color = yellowColor;
        }
        else
        {
            lightComp.color = redColor;
        }
    }
}
