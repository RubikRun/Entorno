using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyelidSleepEffect : MonoBehaviour
{
    SpriteRenderer eyelid;

    public float speedOfSleep = 0.001f;
    public float closedForTime = 0.3f;

    float tSleep = 0f;
    bool isSleeping = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject eyelidObj = GameObject.Find("Eyelid");
        eyelid = eyelidObj.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSleeping)
        {
            if (tSleep < 1f)
            {
                tSleep += speedOfSleep;
            }
            else
            {
                isSleeping = false;
                tSleep = 0f;
                eyelid.color = new Color(eyelid.color.r, eyelid.color.g, eyelid.color.b, 0f);
            }
        }
        UpdateEyelid();
    }

    void UpdateEyelid()
    {
        float tClosed = 1f;
        if (tSleep < 0.5f - closedForTime / 2f)
        {
            tClosed = tSleep / (0.5f - closedForTime / 2f);
        }
        else if (tSleep > 0.5f + closedForTime / 2f)
        {
            tClosed = 1f - (tSleep - (0.5f + closedForTime / 2f)) / (0.5f - closedForTime / 2f);
        }
        eyelid.color = new Color(eyelid.color.r, eyelid.color.g, eyelid.color.b, tClosed);
    }

    public void StartSleep()
    {
        isSleeping = true;
        tSleep = 0f;
    }

    public bool IsTimeForSunrise()
    {
        return tSleep > 0.5f + closedForTime / 2f - 0.01f;
    }
}
