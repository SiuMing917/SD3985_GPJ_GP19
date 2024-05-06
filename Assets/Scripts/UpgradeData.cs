using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    BoombombAmount,
    speed,
    health
}
[CreateAssetMenu(fileName = "Data", menuName = "UpgradeData", order = 1)]
public class UpgradeData : ScriptableObject
{
    public UpgradeType upgradeType;
    public string Name;
    public Sprite icon;
}

