using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        HandleDeath();
    }

    public void HitByEagle()
    {
        healthManager.TakeDamage(0.1f);
        HandleDeath();
    }

    public void HitByShark()
    {
        healthManager.TakeDamage(0.2f);
        HandleDeath();
    }

    public void HealByCat()
    {
        healthManager.Heal(0.02f);
    }

    private void HandleDeath()
    {
        if (healthManager.IsDead())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
