using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData_SO weaponData;
    public BulletPool weaponBulletPool;

    private void Start()
    {
        if(weaponData.bulletPoolName!=null)
        {
            if(GameObject.Find(weaponData.bulletPoolName)!=null)
                weaponBulletPool = GameObject.Find(weaponData.bulletPoolName).GetComponent<BulletPool>();
        }
    }
}
