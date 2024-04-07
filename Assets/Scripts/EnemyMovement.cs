using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public bool isPlayerAlive = true;
    Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    public float speed = 2f;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        
    }


    void Update()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
}
