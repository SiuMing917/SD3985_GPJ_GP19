using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviourPun
{
    //原生变量
    #region
    public float speed = 2.0f;
    public int bombNumber = 1;
    public int bombRadius = 1;
    public int bombsRemaining;
    public int life = 2;
    public int coin = 0;
    public float maxspeed = 6.0f;
    public int maxbombNumber = 8;
    public int maxbombRadius = 10;
    public int maxlife = 10;

    public int PlayerNO;//PlayerNO = 0:AI;PlayerNO > 0:player
    public int NO = 5;
    public int index;//the number of birth order
    public bool isDefended = false;
    public bool Online = false;
    public float delayTime;
    public bool isSkilled = false;
    public float SkillCD = 0f;


    public Sprite[] sprites;
    public GameObject bombPrefab;
    public Sprite[] deathSprites;


    [HideInInspector]
    public GameManager gameManager;
    private SpriteRenderer sr;

    public int leftCount = 0;
    public int rightCount = 0;
    public int upCount = 0;
    public int downCount = 0;
    public int orientation = 0;//0:下 1:左 2:右 3:上
    private bool isFree = true;
    private bool isDead = false;
    private bool setNo = false;
    public bool canPushBomb = false;
    public bool haveGun = false;
    public bool haveUnknownWeapon = false;
    public float canEnterDoor = 0f;//是否可以进入传送门，3s内不能在进入
    #endregion


    //基础函数
    #region
    private void OnEnable()
    {
        bombsRemaining = bombNumber;
    }
    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (PhotonNetwork.IsConnected == true) Online = true;
        if (Online)
        {
            if (photonView.IsMine && !PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("setIndex", RpcTarget.All, this.gameManager.OnlineLocalPlayerIndex);
            }
            PhotonNetwork.SendRate = 50;
            if (!photonView.IsMine && !PhotonNetwork.IsMasterClient)
            {
                gameManager.roleList.Add(this.gameObject);
            }

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        if (!Online)
        {
            sr.sprite = sprites[NO * 16];
        }
        else
        {
            if (photonView.IsMine)
            {

                photonView.RPC("changeSprite", RpcTarget.All, NO * 16);
            }
        }
        StartCoroutine(Defend());

        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (!photonView.IsMine)
            {
                PlayerNO = 5;
                //life = 1;
                gameManager.roleList.AddByIndex(this.gameObject, index);
            }
            delayTime = 5.0f;
        }
        else
        {
            delayTime = 0;
        }
        if (NO == 0)
        {
            speed = 3.0f;
            bombNumber = 2;
            bombRadius = 1;
            life = 3;
            coin = 0;
            maxspeed = 9.0f;
            maxbombNumber = 8;
            maxbombRadius = 8;
            maxlife = 8;
        }
        if (NO == 1)
        {
            speed = 2.0f;
            bombNumber = 1;
            bombRadius = 2;
            life = 3;
            coin = 0;
            maxspeed = 10.0f;
            maxbombNumber = 9;
            maxbombRadius = 9;
            maxlife = 8;
        }

        if (NO == 2)
        {
            speed = 1.0f;
            bombNumber = 1;
            bombRadius = 1;
            life = 5;
            coin = 0;
            maxspeed = 6.0f;
            maxbombNumber = 8;
            maxbombRadius = 12;
            maxlife = 10;
        }

        if (NO == 3)
        {
            speed = 3.0f;
            bombNumber = 2;
            bombRadius = 1;
            life = 3;
            coin = 0;
            maxspeed = 8.0f;
            maxbombNumber = 8;
            maxbombRadius = 8;
            maxlife = 7;
        }

        if (NO == 4)
        {
            speed = 2.0f;
            bombNumber = 2;
            bombRadius = 2;
            life = 2;
            coin = 0;
            maxspeed = 9.0f;
            maxbombNumber = 9;
            maxbombRadius = 9;
            maxlife = 7;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (delayTime > 0)
        {
            delayTime -= Time.fixedDeltaTime;
            if (delayTime <= 0)
            {
                StartCoroutine(Defend());
            }
            return;
        }
        if (canEnterDoor > 0)
        {
            canEnterDoor -= Time.deltaTime;
        }

        //  Debug.Log("life : "+life);
        if (PlayerNO > 0)
        {
            if (Menu.mode == 3 && PlayerNO != 1)
            {
                return;
            }
            MoveByKey();
        }

    }

    private void Update()
    {

        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
            life = 1;
            return;
        }

        if (!isDead && life <= 0)
        {
            //Debug.Log("die");
            Die();
        }
        if (PlayerNO > 0)
        {
            if (Menu.mode == 3 && PlayerNO != 1)
            {
                return;
            }
            FireByKey();
        }

        if (SkillCD > 0)
        {
            SkillCD -= Time.deltaTime;
        }

    }
    #endregion

    //原生函数
    #region
    //0:down 1:left 2:right 3:up
    public void Move(int direction)
    {
        if (!isFree)
            return;

        if (isSkilled)
            return;

        if (direction == 1)
        {
            rightCount = upCount = downCount = 0;
            leftCount = (leftCount + 1) % 20;
            if (!Online)
            {
                sr.sprite = sprites[NO * 16 + leftCount / 5 + 4];
            }
            if (Online && photonView.IsMine)
            {
                photonView.RPC("changeSprite", RpcTarget.All, NO * 16 + leftCount / 5 + 4);
            }
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.left, Space.World);
            orientation = 1;
        }
        if (direction == 2)
        {
            leftCount = downCount = upCount = 0;
            rightCount = (rightCount + 1) % 20;
            if (!Online)
                sr.sprite = sprites[NO * 16 + rightCount / 5 + 8];
            if (Online && photonView.IsMine)
                photonView.RPC("changeSprite", RpcTarget.All, NO * 16 + rightCount / 5 + 8);
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.right, Space.World);
            orientation = 2;
        }
        if (direction == 0)
        {
            rightCount = leftCount = upCount = 0;
            downCount = (downCount + 1) % 20;
            if (!Online)
            {
                sr.sprite = sprites[NO * 16 + downCount / 5];
            }
            if (Online && photonView.IsMine)
            {
                photonView.RPC("changeSprite", RpcTarget.All, NO * 16 + downCount / 5);
            }
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.down, Space.World);
            orientation = 0;
        }
        if (direction == 3)
        {
            rightCount = leftCount = downCount = 0;
            upCount = (upCount + 1) % 20;
            if (!Online)
                sr.sprite = sprites[NO * 16 + upCount / 5 + 12];
            if (Online && photonView.IsMine)
                photonView.RPC("changeSprite", RpcTarget.All, NO * 16 + upCount / 5 + 12);
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.up, Space.World);
            orientation = 3;
        }
        if (direction == -1)
        {
            if (!Online)
                sr.sprite = sprites[NO * 16 + orientation * 4];
            if (Online && photonView.IsMine)
                photonView.RPC("changeSprite", RpcTarget.All, NO * 16 + orientation * 4);
        }




    }

    private void MoveByKey()
    {

        float h = 0, v = 0;

        if (PlayerNO == 1)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        if (PlayerNO == 2)
        {
            h = Input.GetAxisRaw("Horizontal2");
            v = Input.GetAxisRaw("Vertical2");
        }


        if (h != 0) v = 0;
        if (h > 0)
        {
            orientation = 2;
            Move(2);
            return;
        }
        if (h < 0)
        {
            orientation = 1;
            Move(1);
            return;
        }
        if (v > 0)
        {
            orientation = 3;
            Move(3);
        }
        if (v < 0)
        {
            orientation = 0;
            Move(0);
        }
        if (h == 0 && v == 0)
        {
            Move(-1);
        }

    }

    public void Fire()
    {

        if (isFree && !Online)
        {
            if (haveGun)
            {
                StartCoroutine(Defend());
                gameManager.RaserAttack(orientation, this.transform.position);
                haveGun = false;
            }
            else
            {
                StartCoroutine(gameManager.PlaceBomb(this.transform.position, bombRadius, index));
            }


        }
        else if (isFree && Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(gameManager.PlaceBomb(this.transform.position, bombRadius, index));

        }
        else if (isFree && Online && !PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetBombOnline", RpcTarget.All, index, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), bombRadius);
        }
    }

    [PunRPC]
    public void SetBombOnline(int LocalIndex, Vector3 POS, int bombradius)
    {

        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.PlaceBomb(POS, bombradius, LocalIndex));
        }
    }

    public void UseKamahemaha()
    {
        GameManager.Instance.photonView.RPC("UseKamehameha", RpcTarget.All,new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index, orientation);
    }

    [PunRPC]
    public void UseKamahemahaOnline(Vector3 POS, int LocalIndex,int orientation)
    {
        //StartCoroutine(GameManager.Instance.UseKamehameha(POS, LocalIndex));
        GameManager.Instance.photonView.RPC("KamehamehaActive", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index, orientation);
    }



    private void FireByKey()
    {
        if (PlayerNO == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale == 1f)
            {
                Fire();
            }
        }

        if (PlayerNO == 2)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Fire();
            }
        }

        if(transform.Find("Skill").Find("Goku").gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Q )&& SkillCD <= 0)
            {
                photonView.RPC("UseKamahemahaOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index, orientation);
            }
        }

    }

    private void Die()
    {
        if (delayTime > 0)
        {
            life = 1;
            return;
        }
        isFree = false;
        isDead = true;
        StartCoroutine(DeathEffect());
    }


    public void ReduceLife()
    {
        if (true)
        {
            if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient))
            {
                this.life--;
            }
            if (life > 0)
            {
                StartCoroutine(Defend());
            }
        }
    }

    private IEnumerator DeathEffect()
    {
        sr.sprite = deathSprites[NO * 6];
        yield return new WaitForSeconds(1.5f);
        sr.sprite = deathSprites[NO * 6 + 1];
        yield return new WaitForSeconds(0.05f);
        sr.sprite = deathSprites[NO * 6 + 2];
        yield return new WaitForSeconds(0.05f);
        sr.sprite = deathSprites[NO * 6 + 3];
        yield return new WaitForSeconds(0.05f);
        sr.sprite = deathSprites[NO * 6 + 4];
        yield return new WaitForSeconds(0.05f);
        sr.sprite = deathSprites[NO * 6 + 5];
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);

    }
    //3s无敌时间
    public IEnumerator Defend()
    {
        isDefended = true;
        for (int i = 0; i < 6; i++)
        {
            sr.color = new Color(1, 1, 1, 0.9f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.8f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.7f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.6f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.6f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.7f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.8f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 0.9f);
            yield return new WaitForSeconds(0.1f);
            sr.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
            if (Menu.mode != 3 && i == 2)
            {
                break;
            }
        }
        sr.color = new Color(1, 1, 1, 1);
        isDefended = false;
    }
    [PunRPC]
    void changeSprite(int i)
    {
        if (this.setNo == false)
        {
            this.NO = i / 16;
            this.setNo = true;
        }
        //Debug.Log("in RPC:changespite:" + i);
        sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = sprites[i];
    }
    #endregion

    [PunRPC]
    void setIndex(int i)
    {
        index = i;
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }

    [PunRPC]
    public void ActivateGokuSkill()
    {
        foreach (Transform child in transform.Find("Skill"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Skill").Find("Goku").gameObject.SetActive(true);
    }

}