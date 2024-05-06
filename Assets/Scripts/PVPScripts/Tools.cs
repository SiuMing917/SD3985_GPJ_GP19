using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tools : MonoBehaviourPun
{
    GameManager gameManager;

    public int x, y;
    public float disappearTime = 30f;

    public AudioClip toolAudio;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        disappearTime -= Time.deltaTime;
        if (disappearTime < 0)
        {
            //gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(toolAudio, new Vector3(x, y, 0));

        if (CompareTag("tool_bomb"))
        {
            if (collision.GetComponent<Person>().bombNumber < collision.GetComponent<Person>().maxbombNumber)
                collision.GetComponent<Person>().photonView.RPC("AddBombNumber", RpcTarget.All,1);
        }
        if (CompareTag("tool_potion"))
        {
            if (collision.GetComponent<Person>().bombRadius < collision.GetComponent<Person>().maxbombRadius)
                collision.GetComponent<Person>().photonView.RPC("AddRadius", RpcTarget.All, 1);
        }
        if (CompareTag("tool_shoes"))
        {
            if (collision.GetComponent<Person>().speed < collision.GetComponent<Person>().maxspeed)
                collision.GetComponent<Person>().photonView.RPC("AddSpeed", RpcTarget.All, 1f);
        }
        if (CompareTag("tool_heart"))
        {
            if (collision.GetComponent<Person>().life < collision.GetComponent<Person>().maxlife)
                collision.GetComponent<Person>().photonView.RPC("AddLife", RpcTarget.All, 1);
        }
        if (CompareTag("Skill_Goku"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateGokuSkill", RpcTarget.All);
        }
        if(CompareTag("Skill_Fairy"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateFairySkill", RpcTarget.All);
        }
        if (CompareTag("PowerUp"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateMasterSkill", RpcTarget.All);
        }
        if (CompareTag("Cleaner"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateCleanerSkill", RpcTarget.All);
        }
        if (CompareTag("Weapon_Rocket"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateRocketWeapon", RpcTarget.All);
        }
        if (CompareTag("Weapon_Arrow"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateSpikeArrowWeapon", RpcTarget.All);
        }
        if (CompareTag("Chopper"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateChopperWeapon", RpcTarget.All);
        }
        if (CompareTag("WeaponHand"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateHandWeapon", RpcTarget.All);
        }
        if (CompareTag("WeaponMagic"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateMagicWeapon", RpcTarget.All);
        }
        if (CompareTag("WeaponTaser"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateTaserWeapon", RpcTarget.All);
        }
        //gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
        PhotonNetwork.Destroy(gameObject);
    }
}