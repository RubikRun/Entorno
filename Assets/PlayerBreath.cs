using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreath : MonoBehaviour
{
    BreathManager breathManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject breathBar = GameObject.Find("BreathBar");
        breathManager = breathBar.GetComponent<BreathManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LooseBreathInWater()
    {
        breathManager.LooseBreath(0.02f);
    }

    public void RegainBreathOutOfWater()
    {
        breathManager.RegainBreath();
    }
}
