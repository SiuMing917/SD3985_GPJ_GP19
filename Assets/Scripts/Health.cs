using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float currentHealth = 100f;
    public Image healthBar;
    public float maxhealth = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            
            TakeDamage(20);
        }
    }
    public void TakeDamage(int amount)
    {
      
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth /maxhealth;

        if (currentHealth<=0)
        {
            GetComponent<MovementController>().DeathSequence();
            //player died 
            //show death scene
            //GetComponent<EnemyManager>().isPlayerAlive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destructible"))
        {
            Debug.Log("hittt");
            TakeDamage(5);
        }

        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("hittt");
            TakeDamage(5);
        }
    }

}
