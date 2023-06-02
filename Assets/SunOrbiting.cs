using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbiting : MonoBehaviour
{
    public float tick = 8f; // Increasing the tick, increases second rate
    public float mins = 0f;
    public int hours = 6;
    public int days = 1;

    public float nightFasterFactor = 8f;

    public float groundLevel = -3f;

    public float orbitRadius = 200f;

    public float sunAngle = 0f;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CalcTime();
        PlaceSun();
    }

    public void CalcTime()
    {
        if (hours >= 6 && hours <= 18)
        {
            mins += Time.fixedDeltaTime * tick;
        }
        else
        {
            mins += Time.fixedDeltaTime * tick * nightFasterFactor;
        }

        if (mins >= 60f)
        {
            mins = 0f;
            hours += 1;
        }

        if (hours >= 24)
        {
            hours = 0;
            days += 1;
        }
    }

    public void PlaceSun()
    {
        Vector3 sunPosition = CalcSunPosition();
        transform.position = sunPosition;
    }

    public Vector3 CalcSunPosition()
    {
        CalcSunAngle();
        Vector3 playerPos = player.transform.position;

        Vector3 sunPosition = new Vector3(
            playerPos.x + Mathf.Cos(sunAngle) * orbitRadius,
            playerPos.y + Mathf.Sin(sunAngle) * orbitRadius,
            0f
        );

        return sunPosition;
    }

    public void CalcSunAngle()
    {
        sunAngle = 2f * Mathf.PI * (hours * 60f + mins) / (60f * 24f) - Mathf.PI / 2f;
    }
}
