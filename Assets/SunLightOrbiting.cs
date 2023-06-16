using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLightOrbiting : MonoBehaviour
{
    public float radiusFactor = 8f;

    SunOrbiting sunOrbiting;

    GameObject player;
    GameObject sun;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sun = GameObject.Find("Sun");
        sunOrbiting = sun.GetComponent<SunOrbiting>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vecPlayerToSun = sun.transform.position - player.transform.position;
        Vector3 sunLightPosition = player.transform.position + vecPlayerToSun * radiusFactor;
        transform.position = sunLightPosition;
    }
}
