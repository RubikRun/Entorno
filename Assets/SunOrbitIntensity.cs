using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SunOrbitIntensity : MonoBehaviour
{
    SunOrbiting sunOrbiting;
    Light2D lightComp;

    float initialIntensity = -1f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();
        lightComp = GetComponent<Light2D>();
        initialIntensity = lightComp.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIntensity();
    }

    private void UpdateIntensity()
    {
        lightComp.intensity = CalcIntensity() * initialIntensity;
    }

    private float CalcIntensity()
    {
        const float sunriseEndAngle = 2f * Mathf.PI / 18f;
        const float morningEndAngle = 7f * Mathf.PI / 18f;
        const float sunriseEndIntensity = 0.5f;

        float angle = sunOrbiting.sunAngle;

        if (angle < 0f || angle > Mathf.PI)
        {
            return 0f;
        }
        if (angle < sunriseEndAngle)
        {
            return sunriseEndIntensity * fEase(angle / sunriseEndAngle);
        }
        if (angle < morningEndAngle)
        {
            return sunriseEndIntensity + (1f - sunriseEndIntensity) * fEase((angle - sunriseEndAngle) / (morningEndAngle - sunriseEndAngle));
        }
        if (angle < Mathf.PI - morningEndAngle)
        {
            return 1f;
        }
        if (angle < Mathf.PI - sunriseEndAngle)
        {
            return 1f - (1f - sunriseEndIntensity) * fEase((angle - Mathf.PI + morningEndAngle) / (morningEndAngle - sunriseEndAngle));
        }
        if (angle <= Mathf.PI)
        {
            return sunriseEndIntensity - sunriseEndIntensity * fEase((angle - Mathf.PI + sunriseEndAngle) / sunriseEndAngle);
        }

        return -1f;
    }

    private float fEase(float x, float alpha = 1.4f)
    {
        return Mathf.Pow(x, alpha) / (Mathf.Pow(x, alpha) + Mathf.Pow((1f - x), alpha));
    }
}
