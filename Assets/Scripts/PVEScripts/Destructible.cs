using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Destructible : MonoBehaviourPun
{
    public float destructionTime = 1f;
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.2f;
    public GameObject[] spawnableItems;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Instantiate(spawnableItems[randomIndex].name, transform.position, Quaternion.identity);
            }
            else 
            { 
                Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity); 
            }
        }
    }

}
