using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //[SerializeField] Transform player;
    public GameObject player;

    NavMeshAgent agent;
    Rigidbody2D rb;

    [SerializeField] float currentHealth, maxHealth;
    [SerializeField] FloatingHealthBar healthBar;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        if (Input.GetKeyDown(KeyCode.K))
        {

            TakeDamage(20);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {

            //DeathSequence();
            
            var healthComonent1 = this.gameObject.GetComponent<Boss>();

            
            Debug.Log(healthComonent1);
            Debug.Log("explosion hit boss");

            if (healthComonent1 != null)
            {
                healthComonent1.TakeDamage(10);
                //Debug.Log("explosion hit3");
            }
        }
    }
    public void TakeDamage(int amount)
    {

        currentHealth -= amount;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            //player died 
            //show death scene

        }
    }
}
