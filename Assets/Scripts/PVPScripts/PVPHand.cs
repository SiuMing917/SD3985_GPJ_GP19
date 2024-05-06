using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PVPHand : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Role"))
        {
            if (!collision.GetComponent<Person>().isDefended)
            {
                collision.GetComponent<Person>().photonView.RPC("ActivateSTOPState", RpcTarget.All);
                collision.GetComponent<Person>().ReduceLife(1);
            }

        }

    }
}
