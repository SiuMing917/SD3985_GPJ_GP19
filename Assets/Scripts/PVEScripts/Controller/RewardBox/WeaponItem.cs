using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    private GameObject weaponOnHand;
    private PlayerStats playerStats;
    private float changeCD = 2f;
    private void Awake()
    {
        weaponOnHand = GetComponent<WeaponController>().weaponData.weaponPrefab;
    }

    private void Start()
    {
        playerStats = GameManager.Instance.playerStats;

    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
       if (playerStats.isSecondWeapon == false && playerStats.ChangeWeaponCD <= 0f && collision.CompareTag("Player"))
       {
            playerStats.ChangeWeaponCD = 2f;
            Instantiate(playerStats.mainWeapon.GetComponent<WeaponController>().weaponData.weaponOnGroundPrefab, transform.parent);
            playerStats.mainWeapon = weaponOnHand;
            Destroy(playerStats.weaponPos.GetChild(0).gameObject);
       }         
       else if(playerStats.isSecondWeapon == true && playerStats.ChangeWeaponCD <= 0f && collision.CompareTag("Player"))
       {
            playerStats.ChangeWeaponCD = 2f;
           Instantiate(playerStats.secondWeapon.GetComponent<WeaponController>().weaponData.weaponOnGroundPrefab, transform.parent);
           playerStats.secondWeapon = weaponOnHand;
           Destroy(playerStats.weaponPos.GetChild(0).gameObject);
       }
       
    }
}
