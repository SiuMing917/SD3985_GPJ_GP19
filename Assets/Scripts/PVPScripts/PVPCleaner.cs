using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PVPCleaner : MonoBehaviourPun
{
    public GameManager gameManager;
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Role"))
        {
            if (!collision.GetComponent<Person>().isDefended && !collision.GetComponent<Person>().CleanerUser)
            {
                collision.GetComponent<Person>().photonView.RPC("ClearWeapon", RpcTarget.All);
            }
        }

        //if (collision.CompareTag("WeaponHand")|| collision.CompareTag("Arrow") || collision.CompareTag("Bomb") || collision.CompareTag("Spike") || collision.CompareTag("Chopper") || collision.CompareTag("WeaponMagic") || collision.CompareTag("Weapon_Arrow") || collision.CompareTag("Weapon_Rocket"))
        //{
        //    //PhotonNetwork.Destroy(collision.gameObject.GetPhotonView().ViewID);
        //    //GameManager.Instance.photonView.RPC("DestroyMyObject", RpcTarget.All, collision.gameObject.GetPhotonView().ViewID);
        //    PhotonNetwork.Destroy(PhotonView.Find(collision.gameObject.GetPhotonView().ViewID));
        //}

    }
}
