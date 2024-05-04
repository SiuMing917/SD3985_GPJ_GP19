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
                collision.GetComponent<Person>().photonView.RPC("AddBombNumber", RpcTarget.All);
        }
        if (CompareTag("tool_potion"))
        {
            if (collision.GetComponent<Person>().bombRadius < collision.GetComponent<Person>().maxbombRadius)
                collision.GetComponent<Person>().bombRadius++;
        }
        if (CompareTag("tool_shoes"))
        {
            if (collision.GetComponent<Person>().speed < collision.GetComponent<Person>().maxspeed)
                collision.GetComponent<Person>().speed += 1.0f;
        }
        if (CompareTag("tool_heart"))
        {
            if (collision.GetComponent<Person>().life < collision.GetComponent<Person>().maxlife)
                collision.GetComponent<Person>().life++;
        }
        if (CompareTag("Skill_Goku"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateGokuSkill", RpcTarget.All);
        }
        if(CompareTag("Skill_Fairy"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateFairySkill", RpcTarget.All);
        }
        if (CompareTag("Weapon_Rocket"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateRocketWeapon", RpcTarget.All);
        }
        if (CompareTag("Weapon_SpikeArrow"))
        {
            collision.GetComponent<Person>().photonView.RPC("ActivateSpkikeArrowWeapon", RpcTarget.All);
        }

        //gameManager.itemsType[x, y] = GameManager.ItemType.EMPTY;
        PhotonNetwork.Destroy(gameObject);
    }
}