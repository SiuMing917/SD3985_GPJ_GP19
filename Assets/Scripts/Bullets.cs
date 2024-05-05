using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;

    private void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 movDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(movDir.x, movDir.y);
        Destroy(this.gameObject, 2);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(5);

            }
            Destroy(this.gameObject);
        }
        
    }
}
