using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class BossStats : CharacterStats
{
    private GameObject damageText;
    private Transform player;
    private AIPath aiPath;
    private bool isChase = true;
    private float nextKnife;
    public GameObject bulletEffect;
    public GameObject knife;
    public Slider bossBar;

    private EnemyStates enemyStates;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<PlayerController>().transform;
        aiPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        Move();
        GenerateBullet();
        SwitchState();
        RefreshBossBar();
    }

    void RefreshBossBar()
    {
        bossBar.value = (float)CurrentHealth / MaxHealth;
    }

    public void TakeDamage(WeaponData_SO weaponData)
    {
        float chance = Random.Range(0, 1f);
        int damage = Random.Range(weaponData.minDamage, weaponData.maxDamage + 1);
        if (chance < weaponData.criticalChance) weaponData.isCritical = true;
        else weaponData.isCritical = false;

        if (weaponData.isCritical) damage *= 2;
        damage = Mathf.Max(damage - BaseDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        GameObject damageInfo = GameObject.Find("DamageInfo");
        damageText = damageInfo.transform.Find("DamageInfoText").gameObject;
        if (weaponData.isCritical) damageText.GetComponent<Text>().color = Color.red;
        else damageText.GetComponent<Text>().color = Color.yellow;

        damageText.GetComponent<Text>().text = damage.ToString();
        InvokeRepeating("SetDamageInfoTextPos", 0, 0.02f);
        damageText.SetActive(true);
        StartCoroutine(SetDamageInfoTextFalse());

        if (CurrentHealth <= 0)
        {
            isDead = true;
            GameManager.Instance.isGameOverPVE = true;
            anim.SetBool("dead", isDead);
            GameManager.Instance.enemiesFinal.Remove(gameObject);
            Destroy(gameObject, 2f);
            GameObject penalcontrol = GameObject.Find("PenalManager");
            penalcontrol.GetComponent<penalController>().ShowGameOverPenal();
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
        if (isDead == false)
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


    void GenerateBullet()
    {
        if (isDead == false && isChase == true)
        {
            if (GetWeapon().weaponData.weaponType == WeaponType.GUN && (Time.time > nextFire))
            {
                nextFire = Time.time + GetWeapon().weaponData.coolDown;
                int angle = 0;
                for (int i = 0; i < 12; i++)
                {
                    GameObject bullet = GetWeapon().weaponBulletPool.GetBullet();
                    bullet.GetComponent<BulletController>().weaponData = Instantiate(weaponData);
                    bullet.SetActive(true);
                    bullet.transform.eulerAngles = weaponPos.eulerAngles;
                    bullet.GetComponent<BulletController>().isActive = true;
                    bullet.transform.position = weaponPos.position;
                    Vector3 bulletDir = weaponPos.transform.right;
                    Vector3 tempWeaponPos = new Vector3(weaponPos.rotation.x, weaponPos.rotation.y, weaponPos.rotation.z);
                    weaponPos.eulerAngles = new Vector3(tempWeaponPos.x, tempWeaponPos.y, tempWeaponPos.z + angle);
                    angle += 30;
                    bullet.GetComponent<BulletController>().rb.velocity = new Vector2(bulletDir.x, bulletDir.y) * 10;
                }
            }
            if (Time.time > nextKnife)
            {
                nextKnife = Time.time + knife.GetComponent<BossKnife>().weaponData.coolDown;
                anim.Play("boss_chop");
            }
        }
    }

    void SwitchState()
    {
        if (isDead)
        {
            enemyStates = EnemyStates.DEAD;
        }
        else if (GameManager.Instance.playerDead == true || GameManager.Instance.isPlayerInBossRoom == false)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            if (Vector3.Distance(player.transform.position, transform.position) < AttackRange)
            {
                //Debug.Log("Found Player!");
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
                knife.SetActive(false);
                anim.Play("boss_walk");
                break;
            case EnemyStates.CHASE:
                isChase = true;
                knife.SetActive(true);
                if (GetWeapon().weaponData.weaponType == WeaponType.GUN)
                    aiPath.maxSpeed = 6;
                anim.Play("boss_chop");
                break;
            case EnemyStates.DEAD:
                isDead = true;
                aiPath.maxSpeed = 0;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                knife.SetActive(false);
                coll.enabled = false;
                anim.Play("boss_dead");
                break;
            case EnemyStates.GUARD:
                isChase = false;
                knife.SetActive(false);
                aiPath.maxSpeed = 0;
                anim.Play("boss_idle");
                break;
        }
    }


}
