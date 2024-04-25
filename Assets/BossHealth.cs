using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public float currentHealth ;
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
        if (Input.GetKeyDown(KeyCode.K))
        {

            TakeDamage(20);
        }
    }
    public void TakeDamage(int amount)
    {

        currentHealth -= amount;
        healthBar.fillAmount = currentHealth ;

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            //player died 
            //show death scene
            
        }
    }

}
