using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreath : MonoBehaviour
{
    GameObject breathBar;
    BreathManager breathManager;

    // Start is called before the first frame update
    void Start()
    {
        breathBar = GameObject.Find("BreathBar");
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

    public void ShowBreathBar()
    {
        breathBar.SetActive(true);
    }

    public void HideBreathBar()
    {
        breathBar.SetActive(false);
    }
}
