using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public enum EnemyStates { GUARD,PATROL,CHASE,DEAD }
public class EnemyStats : CharacterStats
{
    private GameObject damageText;
    private Transform player;
    private AIPath aiPath;
    private GameObject bullet;
    private bool isChase=true;
    public GameObject debutEffect;
    public bool isElite;

    private EnemyStates enemyStates;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerController>().transform;
        aiPath = GetComponent<AIPath>();
        if (GetWeapon().weaponData.weaponType == WeaponType.KNIFE)
            bullet = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        Move();
        RotateWeapon();
        GenerateBullet();
        SwitchState();
    }
    
    public void TakeDamage(WeaponData_SO weaponData)
    {
        float chance = Random.Range(0, 1f);
        int damage = Random.Range(weaponData.minDamage, weaponData.maxDamage + 1);
        if (chance < weaponData.criticalChance) weaponData.isCritical = true;
        else weaponData.isCritical = false;

        if (weaponData.isCritical) damage *= 2;
        damage = Mathf.Max(damage - BaseDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage,0);

        GameObject damageInfo = GameObject.Find("DamageInfo");
        damageText=damageInfo.transform.Find("DamageInfoText").gameObject;
        if (weaponData.isCritical) damageText.GetComponent<Text>().color = Color.red;
        else damageText.GetComponent<Text>().color = Color.yellow;

        damageText.GetComponent<Text>().text = damage.ToString();
        InvokeRepeating("SetDamageInfoTextPos", 0, 0.02f);
        damageText.SetActive(true);
        StartCoroutine(SetDamageInfoTextFalse());

        if(CurrentHealth<=0)
        {
            isDead = true;
            anim.SetBool("dead", isDead);
            //aiPath.maxSpeed = 0;   //要获取这个组件还得先引用一下命名空间(这句话我在状态机调用)
            if (isElite == false && GameManager.Instance.enemiesOne.Contains(gameObject))
                GameManager.Instance.enemiesOne.Remove(gameObject);          //用数量计算的话有时--抽风，多减了一次，我就换成列表的形式
            else if (isElite == true && GameManager.Instance.enemiesTwo.Contains(gameObject))
            {
                debutEffect.SetActive(false);
                if (GameManager.Instance.enemiesTwo.Contains(gameObject))
                    GameManager.Instance.enemiesTwo.Remove(gameObject);
                else if (GameManager.Instance.enemiesThree.Contains(gameObject))
                    GameManager.Instance.enemiesThree.Remove(gameObject);
                else if (GameManager.Instance.enemiesFour.Contains(gameObject))
                    GameManager.Instance.enemiesFour.Remove(gameObject);
                else if (GameManager.Instance.enemiesFive.Contains(gameObject))
                    GameManager.Instance.enemiesFive.Remove(gameObject);
            }
            Destroy(gameObject, 2f);
        }
    }

    IEnumerator SetDamageInfoTextFalse()
    {
        yield return new WaitForSeconds(0.5f);
        damageText.SetActive(false);
        CancelInvoke("SetDamageInfoTextPos");
    }

    void SetDamageInfoTextPos()
    {
        damageText.transform.position = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
    }

    void Move()
    {
        if (GetComponent<EnemyStats>().isDead == false)
        {
            if (transform.position.x < player.position.x)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    void RotateWeapon()
    {
        if (isDead == false)
        {
            float z;
            if (player.transform.position.y > weaponPos.position.y)
            {
                z = Vector3.Angle(Vector3.right, player.transform.position - weaponPos.position);
            }
            else
            {
                z = -Vector3.Angle(Vector3.right, player.transform.position - weaponPos.position);
            }
            weaponPos.rotation = Quaternion.Euler(0, 0, z);
            if (Mathf.Abs(z) > 90)
            {
                weaponPos.GetChild(0).transform.localEulerAngles = new Vector3(180, 0, 0);
            }
            else
            {
                weaponPos.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    void GenerateBullet()
    {
        if (isDead == false&&isChase==true)
        {
            if (GetWeapon().weaponData.weaponType == WeaponType.GUN&&(Time.time > nextFire))
            {
                nextFire = Time.time + GetWeapon().weaponData.coolDown;
                GameObject bullet = GetWeapon().weaponBulletPool.GetBullet();
                bullet.GetComponent<BulletController>().weaponData = Instantiate(weaponData);
                bullet.SetActive(true);
                bullet.transform.eulerAngles = weaponPos.eulerAngles;
                bullet.GetComponent<BulletController>().isActive = true;
                bullet.transform.position = weaponPos.position;
                Vector3 bulletDir = weaponPos.transform.right;
                bullet.GetComponent<BulletController>().rb.velocity = new Vector2(bulletDir.x, bulletDir.y) * 20;
            }
            else if(Time.time>nextFire)
            {
                nextFire = Time.time + GetWeapon().weaponData.coolDown;
                bullet.GetComponent<BulletController>().weaponData = Instantiate(weaponData);
                bullet.SetActive(true);
                aiPath.maxSpeed = 8;
            }
        }
    }

    void SwitchState()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;
        else if(GameManager.Instance.playerDead == true||GameManager.Instance.isPlayerInMainRoom==false)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            if(Vector3.Distance(player.transform.position,transform.position)<AttackRange)
            {
                enemyStates = EnemyStates.CHASE;
            }
            else
                enemyStates = EnemyStates.PATROL;
        }
        switch (enemyStates)
        {
            case EnemyStates.PATROL:
                isChase = false;
                aiPath.maxSpeed = 3;
                if (bullet) bullet.SetActive(false);
                break;
            case EnemyStates.CHASE:
                isChase = true;
                if(GetWeapon().weaponData.weaponType==WeaponType.GUN)
                    aiPath.maxSpeed = 6;
                break;
            case EnemyStates.DEAD:
                isDead = true;
                aiPath.maxSpeed = 0;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                if (bullet) bullet.SetActive(false);
                coll.enabled = false;
                break;
            case EnemyStates.GUARD:
                isChase = false;
                aiPath.maxSpeed = 0;
                break;
        }
    }

}
