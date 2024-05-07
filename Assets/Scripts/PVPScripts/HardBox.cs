using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HardBox : MonoBehaviourPun
{
    public Sprite[] HardBoxSprites;
    public int index;
    public int life = 3; 
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            index = Random.Range(0, 3);
            GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2];
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2 + 1];
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                index = 0;
                GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2];
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2 + 1];
                photonView.RPC("changeSprite", RpcTarget.Others, index);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(life == 2)
        {
            index = 1;
            GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2];
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2 + 1];
            photonView.RPC("changeSprite", RpcTarget.Others, index);
        }

        if (life == 1)
        {
            index = 2;
            GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2];
            transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = HardBoxSprites[index * 2 + 1];
            photonView.RPC("changeSprite", RpcTarget.Others, index);
        }
    }

    [PunRPC]
    public void changeSprite(int i)
    {
        index = i;
        GetComponent<SpriteRenderer>().sprite = HardBoxSprites[i * 2];
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = HardBoxSprites[i * 2 + 1];
    }
}
