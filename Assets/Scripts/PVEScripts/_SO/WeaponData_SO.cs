using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { GUN,KNIFE}
public enum WeaponLevel { NORMAL,GOOD,RARE,EPIC,LEGEND}
[CreateAssetMenu(fileName ="New Weapon",menuName ="CharacterStats/Weapon Data")]
public class WeaponData_SO : ScriptableObject
{
    [Header("Attack Info")]
    public float attackRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;
    public int bulletAmount;
    public float criticalChance;
    public string weaponName;
    public string bulletPoolName;
    public WeaponType weaponType;
    public WeaponLevel weaponLevel;
    public GameObject weaponPrefab;
    public GameObject weaponOnGroundPrefab;

    public bool isCritical = false;

    public GameObject bulletPrefab;
    bool onGround;
}
