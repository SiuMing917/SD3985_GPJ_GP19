using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PVPDamages : MonoBehaviourPun
{
    public int damamge = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Role"))
        {
            if (!collision.GetComponent<Person>().isDefended)
            {
                collision.GetComponent<Person>().ReduceLife(damamge);
            }

        }

    }
}
