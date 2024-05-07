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
    public int maxlife = 20;
    public float explosionTime = 1f;

    public int PlayerNO;//PlayerNO = 0:AI;PlayerNO > 0:player
    public int NO = 5;
    public int index;//the number of birth order
    public bool isDefended = false;
    public bool Online = false;
    public float delayTime;
    public bool isSkilled = false;

    public bool CleanerUser = false;
    public bool CanPlaceBox = true;

    public float SkillCD = 0f;
    public int WeaponBullet = 5;
    public float AutoHealthCD = 10f;
    public float AutoDefendCD = 15f;

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

        //photonView.RPC("InitializedPlayerStatus", RpcTarget.All, NO);

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
            //life = 1;
            photonView.RPC("InitializedPlayerStatus", RpcTarget.All, NO);
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

        if(this.bombNumber > this.maxbombNumber)
        {
            this.bombNumber = this.maxbombNumber;
        }

        if (this.bombRadius > this.maxbombRadius)
        {
            this.bombRadius = this.maxbombRadius;
        }

        if (this.speed > this.maxspeed)
        {
            this.speed = this.maxspeed;
        }

        if (this.life > this.maxlife)
        {
            this.life = this.maxlife;
        }

        if (WeaponBullet <= 0)
        {
            photonView.RPC("ClearWeapon", RpcTarget.All);
        }

        if (SkillCD > 0f)
        {
            if(NO == 1)
            {
                SkillCD -= Time.deltaTime*2;
            }
            else
                SkillCD -= Time.deltaTime;
        }


        if (AutoDefendCD > 0f)
        {
            AutoDefendCD -= Time.deltaTime;
        }
        else
        {
            if (NO == 4)
            {
                StartCoroutine(Defend());
            }
            AutoDefendCD = 20f;
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
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.left);
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
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.right);
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
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.down);
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
            transform.Translate(speed * Time.fixedDeltaTime * Vector3.up);
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

    [PunRPC]
    public void SetBoxOnline(int LocalIndex, Vector3 POS, int orientation)
    {

        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.PlaceBox(POS, orientation, LocalIndex));
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

    [PunRPC]
    public void UseFairyPowerOnline(Vector3 POS, int LocalIndex)
    {
        GameManager.Instance.photonView.RPC("UseFariyPower", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index);
    }

    [PunRPC]
    public void UsePowerUpOnline(Vector3 POS, int LocalIndex)
    {
        StartCoroutine(GameManager.Instance.UsePowerUp(POS, index));
    }

    [PunRPC]
    public void UseCleanerOnline(Vector3 POS, int LocalIndex)
    {
        StartCoroutine(GameManager.Instance.UseCleaner(POS, index));
    }

    [PunRPC]
    public void UseRocketOnline(Vector3 playPos, Vector3 targetPos, int host)
    {
        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.RocketFire(playPos, targetPos, host));
        }
    }
    [PunRPC]
    public void UseArrowOnline(Vector3 playPos, Vector3 targetPos, int host)
    {
        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.ArrowFire(playPos, targetPos, host));
        }
    }
    [PunRPC]
    public void UseChopperOnline(Vector3 playPos, Vector3 targetPos, int host)
    {
        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.ChopperFire(playPos, targetPos, host));
        }
    }

    [PunRPC]
    public void UseHandOnline(Vector3 playPos, Vector3 targetPos, int host)
    {
        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.HandFire(playPos, targetPos, host));
        }
    }

    [PunRPC]
    public void UseMagicOnline(Vector3 playPos, Vector3 targetPos, int host)
    {
        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.MagicFire(playPos, targetPos, host));
        }
    }

    [PunRPC]
    public void UseTaserOnline(Vector3 playPos, Vector3 targetPos, int host)
    {
        if (Online && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameManager.Instance.TaserFire(playPos, targetPos, host));
        }
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

        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale == 1f && CanPlaceBox)
        {
            photonView.RPC("SetBoxOnline", RpcTarget.All, index, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), this.orientation);
        }

        if (transform.Find("Skill").Find("Goku").gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Q )&& SkillCD <= 0 && !isSkilled)
            {
                photonView.RPC("UseKamahemahaOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index, orientation);
                this.SkillCD = 10f;
            }
        }

        if (transform.Find("Skill").Find("Fairy").gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && SkillCD <= 0 && !isSkilled)
            {
                photonView.RPC("UseFairyPowerOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index);
                this.SkillCD = 10f;
            }
        }

        if (transform.Find("Skill").Find("Master").gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && SkillCD <= 0)
            {
                this.WeaponBullet++;
                photonView.RPC("UsePowerUpOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index);
                //photonView.RPC("AddWeaponBullet", RpcTarget.All, 1);
                this.SkillCD = 15f;
            }
        }

        if (transform.Find("Skill").Find("Cleaner").gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && SkillCD <= 0)
            {
                this.CleanerUser = true;
                photonView.RPC("UseCleanerOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), index);
                //photonView.RPC("AddWeaponBullet", RpcTarget.All, 1);
                this.SkillCD = 20f;
            }
        }


        if (transform.Find("Weapon").Find("Rocket").gameObject.activeSelf == true && Time.timeScale == 1f)
        {
            if(bombNumber > 0 && Input.GetMouseButtonDown(0) && WeaponBullet >0)// 按下鼠標左鍵
            {
                this.WeaponBullet--;
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
                clickPosition.z = 0f;
                clickPosition.x = Mathf.Round(clickPosition.x);
                clickPosition.y = Mathf.Round(clickPosition.y);

                photonView.RPC("UseRocketOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), clickPosition, index);
                //this.WeaponBullet --;
                //Instantiate(bombPrefab).GetComponent<BombBomb>().ThrowBombActive(this.transform.position, clickPosition);
                //StartCoroutine(ThrowBomb(clickPosition)); // 執行放置炸彈到點擊位置的方法s
            }
        }

        if (Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("SpikeArrow").gameObject.activeSelf) // 按下鼠標左鍵
        {
            this.WeaponBullet--;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            photonView.RPC("UseArrowOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), clickPosition, index);
            //StartCoroutine(ShootSpike(clickPosition));
        }

        if (Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("Chopper").gameObject.activeSelf) // 按下鼠標左鍵
        {
            this.WeaponBullet--;
            //0:down 1:left 2:right 3:up
            /**Vector3 chopperposion = this.transform.position;
            if(orientation == 0)
            {
                chopperposion.y = this.transform.position.y - 1f; 
            }
            else if(orientation == 1)
            {
                chopperposion.x = this.transform.position.x - 1f;
            }
            else if (orientation == 2)
            {
                chopperposion.x = this.transform.position.x + 1f;
            }
            else if (orientation == 3)
            {
                chopperposion.y = this.transform.position.y + 1f;
            }**/
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            photonView.RPC("UseChopperOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), clickPosition, index);
        }

        if (Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("Hand").gameObject.activeSelf) // 按下鼠標左鍵
        {
            this.WeaponBullet--;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            photonView.RPC("UseHandOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), clickPosition, index);
        }

        if (Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("Magic").gameObject.activeSelf) // 按下鼠標左鍵
        {
            this.WeaponBullet--;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            photonView.RPC("UseMagicOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), clickPosition, index);
        }

        if (Input.GetMouseButtonDown(0) && transform.Find("Weapon").Find("Taser").gameObject.activeSelf) // 按下鼠標左鍵
        {
            this.WeaponBullet--;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 取得鼠標點擊位置
            clickPosition.z = 0f;
            clickPosition.x = Mathf.Round(clickPosition.x);
            clickPosition.y = Mathf.Round(clickPosition.y);
            photonView.RPC("UseTaserOnline", RpcTarget.All, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), clickPosition, index);
            //StartCoroutine(ShootSpike(clickPosition));
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

    [PunRPC]
    public void UsingMagic()
    {
        StartCoroutine(MagicMoving());
    }

    public void ReduceLife( int i)
    {
        if (true)
        {
            if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient))
            {
                this.life = this.life-i;
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
        for (int i = 0; i < 3; i++)
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
        }
        sr.color = new Color(1, 1, 1, 1);
        isDefended = false;
    }

    public IEnumerator MagicMoving()
    {
       this.GetComponent<Collider2D>().isTrigger = true;
       yield return new WaitForSeconds(2f);
       this.GetComponent<Collider2D>().isTrigger = false;
    }
    public IEnumerator StopMove()
    {
        isFree = false;
        yield return new WaitForSeconds(1f);
        photonView.RPC("ClearState", RpcTarget.All);
        photonView.RPC("ClearWeapon", RpcTarget.All);
        photonView.RPC("ClearSkill", RpcTarget.All);
        isFree = true;
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
        //if (other.gameObject.layer == LayerMask.NameToLayer("WeaponBullet"))
        //{
        //    other.isTrigger = true;
        //}
        //if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
        //{
        //    other.isTrigger = true;
        //}
    }

    [PunRPC]
    public void InitializedPlayerStatus(int NO)
    {
        if (NO == 0)
        {
            speed = 3.0f;
            bombNumber = 2;
            bombRadius = 1;
            life = 12;
            coin = 0;
            maxspeed = 8.0f;
            maxbombNumber = 12;
            maxbombRadius = 6;
            maxlife = 12;
        }
        if (NO == 1)
        {
            speed = 2.0f;
            bombNumber = 1;
            bombRadius = 2;
            life = 12;
            coin = 0;
            maxspeed = 6.0f;
            maxbombNumber = 6;
            maxbombRadius = 8;
            maxlife = 12;
        }

        if (NO == 2)
        {
            speed = 2.0f;
            bombNumber = 1;
            bombRadius = 2;
            life = 16;
            coin = 0;
            maxspeed = 4.0f;
            maxbombNumber = 5;
            maxbombRadius = 12;
            maxlife = 16;
            transform.GetComponent<Rigidbody2D>().mass = 30;
        }

        if (NO == 3)
        {
            speed = 2.0f;
            bombNumber = 2;
            bombRadius = 1;
            life = 12;
            coin = 0;
            maxspeed = 6.0f;
            maxbombNumber = 6;
            maxbombRadius = 8;
            maxlife = 12;
            explosionTime = 2f;
        }

        if (NO == 4)
        {
            speed = 2.0f;
            bombNumber = 2;
            bombRadius = 2;
            life = 8;
            coin = 0;
            maxspeed = 7.0f;
            maxbombNumber = 6;
            maxbombRadius = 9;
            maxlife = 8;
        }
    }

    //For Skills and Weapons
    #region
    [PunRPC]
    public void ActivateGokuSkill()
    {
        foreach (Transform child in transform.Find("Skill"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Skill").Find("Goku").gameObject.SetActive(true);
    }

    [PunRPC]
    public void ActivateFairySkill()
    {
        foreach (Transform child in transform.Find("Skill"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Skill").Find("Fairy").gameObject.SetActive(true);
    }

    [PunRPC]
    public void ActivateMasterSkill()
    {
        foreach (Transform child in transform.Find("Skill"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Skill").Find("Master").gameObject.SetActive(true);
    }

    [PunRPC]
    public void ActivateCleanerSkill()
    {
        foreach (Transform child in transform.Find("Skill"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Skill").Find("Cleaner").gameObject.SetActive(true);
    }

    [PunRPC]
    public void ActivateRocketWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("Rocket").gameObject.SetActive(true);
        WeaponBullet = 5;
    }
    [PunRPC]
    public void ActivateSpikeArrowWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("SpikeArrow").gameObject.SetActive(true);
        WeaponBullet = 2;
    }
    [PunRPC]
    public void ActivateTaserWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("Taser").gameObject.SetActive(true);
        WeaponBullet = 1;
    }
    [PunRPC]
    public void ActivateShotgunWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("Shotgun").gameObject.SetActive(true);
        WeaponBullet = 2;
    }
    [PunRPC]
    public void ActivateChopperWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("Chopper").gameObject.SetActive(true);
        WeaponBullet = 3;
    }

    [PunRPC]
    public void ActivateHandWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("Hand").gameObject.SetActive(true);
        WeaponBullet = 3;
    }

    [PunRPC]
    public void ActivateMagicWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Weapon").Find("Magic").gameObject.SetActive(true);
        WeaponBullet = 3;
    }

    [PunRPC]
    public void ActivateSTOPState()
    {
        foreach (Transform child in transform.Find("State"))
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("State").Find("STOP").gameObject.SetActive(true);
        StartCoroutine(StopMove());
    }

    [PunRPC]
    public void AddWeaponBullet(int number)
    {
        this.WeaponBullet += number;
    }

    [PunRPC]
    public void ClearWeapon()
    {
        foreach (Transform child in transform.Find("Weapon"))
        {
            child.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void ClearState()
    {
        foreach (Transform child in transform.Find("State"))
        {
            child.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void ClearSkill()
    {
        foreach (Transform child in transform.Find("Skill"))
        {
            child.gameObject.SetActive(false);
        }
    }

    #endregion

    [PunRPC]
    public void AddBombNumber(int i)
    {
        if(bombNumber <= maxbombNumber && bombNumber >= 0)
            this.bombNumber+= i;

        if (this.bombNumber <= 0)
            this.bombNumber = 1;

        if (this.bombNumber > this.maxbombNumber)
        {
            this.bombNumber = this.maxbombNumber;
        }
    }

    [PunRPC]
    public void AddLife(int i)
    {
        if(life<=maxlife && life >= 1)
        this.life+= i;

        if (this.life <= 0)
            this.life = 1;

        if (this.life > this.maxlife)
        {
            this.life = this.maxlife;
        }
    }

    [PunRPC]
    public void AddSpeed(float i)
    {
        if(speed<=maxspeed && speed >= 2f)
        this.speed += i;
        if (this.speed <= 1f)
            this.speed = 2f;

        if (this.speed > this.maxspeed)
        {
            this.speed = this.maxspeed;
        }
    }

    [PunRPC]
    public void AddRadius(int i)
    {
        if(bombRadius <= maxbombRadius && bombRadius >= 1)
        this.bombRadius += i;

        if (this.bombRadius <= 0f)
            this.bombRadius = 1;

        if (this.bombRadius > this.maxbombRadius)
        {
            this.bombRadius = this.maxbombRadius;
        }
    }

    [PunRPC]
    public void MoveToPosition(Vector3 position)
    {
        this.transform.position = position;
    }
}