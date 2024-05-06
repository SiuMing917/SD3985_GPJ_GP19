using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeStatus : MonoBehaviour
{
    [SerializeField] List<UpgradeData> upgrades;
    List<UpgradeData> selectedUpgrades;

    [SerializeField] List<UpgradeData> aquiredUpgrades;

    public void Upgrade(int selectedUpgradeId)
    {
        selectedUpgrades = upgrades;
        Debug.Log("selectedUpgradeId" + selectedUpgradeId);
        UpgradeData upgradeData = selectedUpgrades[selectedUpgradeId];
        if (aquiredUpgrades == null) { aquiredUpgrades = new List<UpgradeData>(); }
        Debug.Log(aquiredUpgrades);
        Debug.Log(upgradeData);

        aquiredUpgrades.Add(upgradeData);
        Debug.Log("aquiredUpgrades"+ aquiredUpgrades);
        upgrades.Remove(upgradeData);
    }

}
