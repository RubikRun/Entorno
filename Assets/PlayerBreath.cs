using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBreath : MonoBehaviour
{
    GameObject breathBar;
    BreathManager breathManager;

    // Start is called before the first frame update
    void Start()
    {
        breathBar = GameObject.Find("BreathBar");
        breathManager = breathBar.GetComponent<BreathManager>();
        breathBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LooseBreathInWater()
    {
        breathManager.LooseBreath(0.044f);
        HandleDeath();
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

    private void HandleDeath()
    {
        if (breathManager.IsOutOfBreath())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
