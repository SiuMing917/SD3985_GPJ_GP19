using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    private GameObject damageText;
    private Rigidbody2D rb;
    private float nextDefenceRestore;
    private Transform originalWeaponPos;

    [Header("SkillWeapon")]
    public Transform skillWeaponPos;
    public GameObject skillFireEffect;
    private int currentSkillPoint = 0;
    private const int maxSkillPoint = 200;
    public GameObject mainWeapon;
    public GameObject secondWeapon;
    public bool isSecondWeapon;
    public float ChangeWeaponCD = 2f;

    [Header("Skill")]
    public Image flashSlider;
    private bool isSkill=false;
    private void Update()
    {   
        if (isDead == false)
        {
            Skill();       
            RotateWeapon();
            ApplyWeapon();
            SwitchWeapon();
            GenerateBullet();
            KnifeAttack(); 
            RestoreDefence();
        }
        RefreshSkillUI();
        if (ChangeWeaponCD >= 0f)
            ChangeWeaponCD -= Time.deltaTime;
    }
    protected override void Awake()
    {
        base.Awake();
        Instantiate(mainWeapon, weaponPos);
        rb = GetComponent<Rigidbody2D>();
        originalWeaponPos = weaponPos.transform;
    }
    public void RotateWeapon()
    {
        if (GetWeapon() != null && GetWeapon().weaponData.weaponType == WeaponType.GUN)
        {
            float z;
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z += 10;
            if (mousePos.y > weaponPos.position.y)
            {
                z = Vector3.Angle(Vector3.right, mousePos - weaponPos.position);
            }
            else
            {
                z = -Vector3.Angle(Vector3.right, mousePos - weaponPos.position);
            }
            weaponPos.rotation = Quaternion.Euler(0, 0, z);
            skillWeaponPos.rotation = Quaternion.Euler(0, 0, z);
            if (Mathf.Abs(z) > 90)
            {
                weaponPos.GetChild(0).transform.localEulerAngles = new Vector3(180, 0, 0);
                if (isSkill && skillWeaponPos.childCount != 0)
                    skillWeaponPos.GetChild(0).transform.localEulerAngles = new Vector3(180, 0, 0);
            }
            else
            {
                weaponPos.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
                if (isSkill && skillWeaponPos.childCount != 0)
                    skillWeaponPos.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
            weaponPos.rotation = Quaternion.Euler(originalWeaponPos.rotation.x,originalWeaponPos.rotation.y,originalWeaponPos.rotation.z);
    }

    public void SwitchWeapon()
    {
        if (weaponPos.childCount == 0)
            Instantiate(mainWeapon, weaponPos);
        if(Input.GetAxis("Mouse ScrollWheel")!=0)
        {
            if(weaponPos.childCount!=0)
                Destroy(weaponPos.GetChild(0).gameObject);
            isSecondWeapon = !isSecondWeapon;
            if (isSecondWeapon)
                Instantiate(secondWeapon, weaponPos);
            else
                Instantiate(mainWeapon, weaponPos);
        }
    }

    void GenerateBullet()
    {
        if(Input.GetMouseButtonDown(0)&&GetWeapon().weaponData.weaponType==WeaponType.GUN&&CurrentEnergy-GetWeapon().weaponData.bulletAmount>=0) 
        {
            if(Time.time>nextFire)
            {
                nextFire = Time.time + GetWeapon().weaponData.coolDown;
                GameObject bullet = GetWeapon().weaponBulletPool.GetBullet();
                CurrentEnergy-=GetWeapon().weaponData.bulletAmount;
                bullet.GetComponent<BulletController>().weaponData = Instantiate(weaponData);
                bullet.SetActive(true);
                bullet.transform.eulerAngles = weaponPos.eulerAngles;
                bullet.GetComponent<BulletController>().isActive = true;
                bullet.transform.position = weaponPos.position;
                Vector3 dir = weaponPos.transform.right;
                bullet.GetComponent<BulletController>().rb.velocity = new Vector2(dir.x,dir.y)*20;
            }
        }
    }
    void KnifeAttack()
    {
        if(Input.GetMouseButtonDown(0) &&GetWeapon().weaponData.weaponType==WeaponType.KNIFE&&CurrentEnergy - GetWeapon().weaponData.bulletAmount >= 0)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + GetWeapon().weaponData.coolDown;
                if (transform.childCount == 6)
                    Instantiate(GetWeapon().weaponData.bulletPrefab, transform); 
                else if(GetWeapon().weaponData.bulletPrefab.name!=transform.GetChild(6).GetComponent<KnifeController>().knifeName)
                {
                    Destroy(transform.GetChild(6).gameObject);
                    Instantiate(GetWeapon().weaponData.bulletPrefab, transform);
                }
                GameObject knife = transform.GetChild(6).gameObject;
                knife.GetComponent<KnifeController>().weaponData = Instantiate(weaponData);
                knife.SetActive(true);
            }
        }
    }
    void Skill()
    {
        //if (Input.GetMouseButtonDown(1))
        if (Time.time > nextFire && isSkill == true&&GetWeapon().weaponData.weaponType==WeaponType.GUN)
        {
            nextFire = Time.time + GetWeapon().weaponData.coolDown / 2;
            if (skillWeaponPos.childCount == 0)
            {
                GameObject skillWeapon = Instantiate(weapon,skillWeaponPos);
                skillWeapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }

            GameObject bullet = GetWeapon().weaponBulletPool.GetBullet();
            bullet.GetComponent<BulletController>().weaponData = Instantiate(weaponData);
            bullet.SetActive(true);

            bullet.transform.eulerAngles = skillWeaponPos.eulerAngles;
            bullet.GetComponent<BulletController>().isActive = true;
            bullet.transform.position = skillWeaponPos.position;
            Vector3 dir = skillWeaponPos.transform.right;
            bullet.GetComponent<BulletController>().rb.velocity = new Vector2(dir.x, dir.y) * 20;

            GameObject bullet2 = GetWeapon().weaponBulletPool.GetBullet();
            bullet2.GetComponent<BulletController>().weaponData = Instantiate(weaponData);
            bullet2.SetActive(true);
            bullet2.transform.eulerAngles = weaponPos.eulerAngles;
            bullet2.GetComponent<BulletController>().isActive = true;
            bullet2.transform.position = weaponPos.position;
            Vector3 dir2 = weaponPos.transform.right;
            bullet2.GetComponent<BulletController>().rb.velocity = new Vector2(dir2.x, dir2.y) * 20;
        }
        else if(Time.time > nextFire && isSkill == true)
        {
            nextFire = Time.time + GetWeapon().weaponData.coolDown / 2;
            if (transform.childCount == 6)
                Instantiate(GetWeapon().weaponData.bulletPrefab, transform);
            else if (GetWeapon().weaponData.bulletPrefab.name != transform.GetChild(6).GetComponent<KnifeController>().knifeName)
            {
                Destroy(transform.GetChild(6).gameObject);
                Instantiate(GetWeapon().weaponData.bulletPrefab, transform);
            }
            GameObject knife = transform.GetChild(6).gameObject;
            knife.GetComponent<KnifeController>().weaponData = Instantiate(weaponData);
            knife.SetActive(true);
        }
    }



    public void RestoreDefence()
    {
        if(CurrentDefence<BaseDefence)
        {
            if (Time.time > nextDefenceRestore)
            {
                nextDefenceRestore = Time.time + 1.5f;
                CurrentDefence++;
            }
        }
    }
    public void RefreshSkillUI()
    {
        if(isDead)
        {
            skillFireEffect.SetActive(false);
            return;
        }
        if (Time.timeScale == 1)
        {
            if (Input.GetMouseButtonDown(1) && flashSlider.fillAmount == 1)
            {
                isSkill = true;
            }

            if (isSkill == false && currentSkillPoint < maxSkillPoint)
            {
                currentSkillPoint++;
            }
            else if (isSkill == true)
            {
                skillFireEffect.SetActive(true);
                currentSkillPoint -= 2;                           
                if (currentSkillPoint <= 0)
                {
                    isSkill = false;
                    skillFireEffect.SetActive(false);
                    for (int i = 0; i < skillWeaponPos.childCount; i++)
                    {
                        Destroy(skillWeaponPos.GetChild(i).gameObject);
                    }
                }
            }
            flashSlider.fillAmount = (float)currentSkillPoint / maxSkillPoint;
        }
    }
    public void TakeDamage(WeaponData_SO weaponData)
    {
        if(CurrentDefence!=0)
        {
            CurrentDefence--;
            nextDefenceRestore = Time.time + 4f;
        }
        else
        {
            nextDefenceRestore = Time.time + 4f;
            float chance = Random.Range(0, 1f);
            int damage = Random.Range(weaponData.minDamage, weaponData.maxDamage + 1);
            if (chance < weaponData.criticalChance) weaponData.isCritical = true;
            else weaponData.isCritical = false;

            if (weaponData.isCritical) damage = damage * 2;
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

            GameObject damageInfo = GameObject.Find("DamageInfo"); 
            damageText = damageInfo.transform.Find("DamageInfoText_Player").gameObject;
            if (weaponData.isCritical) damageText.GetComponent<Text>().color = Color.red;
            else damageText.GetComponent<Text>().color = Color.yellow;

            damageText.GetComponent<Text>().text = damage.ToString();
            InvokeRepeating("SetDamageInfoTextPos", 0, 0.02f);
            damageText.SetActive(true);
            StartCoroutine(SetDamageInfoTextFalse());

            if (CurrentHealth <= 0)
            {
                isDead = true;
                coll.enabled = false;
                rb.velocity = Vector2.zero;
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                anim.SetBool("dead", isDead);

                GameObject penalcontrol = GameObject.Find("PenalManager");
                penalcontrol.GetComponent<penalController>().ShowPlayerDeadPenal();
            }
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


}
