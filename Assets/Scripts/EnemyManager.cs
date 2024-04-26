
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isPlayerAlive = true;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public GameObject effect;


    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        if (isPlayerAlive == true)
        {

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(effect, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            return;
        }




    }
}