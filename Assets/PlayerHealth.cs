using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    HealthManager healthManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject healthBar = GameObject.Find("HealthBar");
        healthManager = healthBar.GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitByBear()
    {
        healthManager.TakeDamage(0.1f);
    }

    public void HealByCat()
    {
        healthManager.Heal(0.02f);
    }
}
