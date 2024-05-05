using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] enemyprefabs;
    [SerializeField] private bool canSpawn = true;
    public Transform[] spawnPoints;
    public GameObject effect;


    void Start()
    {
        StartCoroutine(Spawn());
    }

   
    private IEnumerator Spawn ()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);
        WaitForSeconds delay = new WaitForSeconds(3f);
        while (canSpawn)
        {

            yield return wait;
            int rand = Random.Range(0, enemyprefabs.Length);
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject emenyToSpawn = enemyprefabs[rand];
            Instantiate(effect, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            yield return delay;
            Instantiate(emenyToSpawn, transform.position, Quaternion.identity);

        }
    }
}