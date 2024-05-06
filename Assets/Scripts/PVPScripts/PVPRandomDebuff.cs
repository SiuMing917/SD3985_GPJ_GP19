using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PVPRandomDebuff : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Role"))
        {
            if (!collision.GetComponent<Person>().isDefended)
            {
                int randomIndex = Random.Range(0, 3);
                if (randomIndex == 0)
                {
                    collision.GetComponent<Person>().ReduceLife(1);
                    collision.GetComponent<Person>().photonView.RPC("AddBombNumber", RpcTarget.All, -1);
                }
                if (randomIndex == 1)
                {
                    collision.GetComponent<Person>().ReduceLife(1);
                    collision.GetComponent<Person>().photonView.RPC("AddRadius", RpcTarget.All, -1);
                }
                if (randomIndex == 2)
                {
                    collision.GetComponent<Person>().ReduceLife(1);
                    collision.GetComponent<Person>().photonView.RPC("AddSpeed", RpcTarget.All, -1f);
                }
                if (randomIndex == 3)
                {
                    collision.GetComponent<Person>().ReduceLife(1);
                    collision.GetComponent<Person>().photonView.RPC("AddLife", RpcTarget.All, -1);
                }
            }

        }

    }
}
