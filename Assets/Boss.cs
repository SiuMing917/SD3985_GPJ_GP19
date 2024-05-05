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
    private float patrolRange = 3f;

    [SerializeField] float currentHealth, maxHealth;
    [SerializeField] FloatingHealthBar healthBar;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float speed;
    public float lineofSite;
    public float shootingRange;
    public float fireRate = 1f;
    private float nextFireTime;


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
        //agent.SetDestination(player.transform.position);
        float distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < lineofSite && distanceToPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            //agent.SetDestination(player.transform.position);
        }
        else if (distanceToPlayer <= shootingRange && nextFireTime < Time.time)
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
           // var bullet1 = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            //var bullet2 = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            //var bullet3 = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, 45) * bulletSpawnPoint.up * speed; //for shooting towards the right
            //bullet1.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, -45) * bulletSpawnPoint.up * speed; //for shooting towards the left
            //bullet2.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, 135) * bulletSpawnPoint.up * speed; //for shooting towards the down
            //bullet3.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, -135) * bulletSpawnPoint.up * speed; //for shooting towards the up
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {

            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            bullet.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, 45) * bulletSpawnPoint.up * speed; //for shooting towards the right
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            //DeathSequence();            
            var healthComonent1 = this.gameObject.GetComponent<Boss>();            
            //Debug.Log(healthComonent1);
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
            Debug.Log("boss died");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineofSite);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

}
