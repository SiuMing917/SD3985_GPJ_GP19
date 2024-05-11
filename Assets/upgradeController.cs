using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upgradeController : MonoBehaviour
{
    [SerializeField] List<UpgradeData> upgrades;
    [SerializeField] List<UpgradeButton> upgradeButtons;
    List<UpgradeData> selectedUpgrades;

    [SerializeField] List<UpgradeData> aquiredUpgrades;
    public List<UpgradeData> GetUpgrades(int count)
    {

        List<UpgradeData> upgradeList = new List<UpgradeData>();
        //Debug.Log(count);

        if (count > upgrades.Count)
        {
            count = upgrades.Count;
            Debug.Log("count1" + count);
        }

        for (int i = 0; i < count; i++)
        {
            //Debug.Log(count);
            //Debug.Log("canget");
            upgradeList.Add(upgrades[Random.Range(0, upgrades.Count)]);
            //Debug.Log("canget");
        }

        return upgradeList;
    }

    public void Upgrade(int pressedButtonId)
    {
        //Debug.Log("pressed" + pressedButtonId.ToString());

        GameObject.Find("Player").transform.GetComponent<upgradeStatus>().Upgrade(pressedButtonId);
        //Upgrade.(pressedButtonId);
        GetComponent<penalController>().ClosePenal();

    }
}
