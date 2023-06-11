using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathManager : MonoBehaviour
{
    [SerializeField]
    public Image breathBar;

    private float breathAmount = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LooseBreath(float breath)
    {
        breathAmount -= breath;
        breathAmount = Mathf.Clamp(breathAmount, 0, 100f);
        breathBar.fillAmount = breathAmount / 100f;
    }

    public void RegainBreath()
    {
        breathAmount = 100f;
        breathBar.fillAmount = 1f;
    }

    public bool IsOutOfBreath()
    {
        return breathAmount <= 0f;
    }
}
