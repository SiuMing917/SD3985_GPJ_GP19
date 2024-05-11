using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penalController : MonoBehaviour
{
    [SerializeField] GameObject penal;
    [SerializeField] List<UpgradeData> upgradeDatas;
    [SerializeField] List<UpgradeButton> upgradeButtons;

    public void OpenPenal()
    {
        Time.timeScale = 0;
        penal.SetActive(true);
        for (int i = 0; i < upgradeDatas.Count; i++)
        {
            upgradeButtons[i].Set(upgradeDatas[i]);
        }

    }

    public void ClosePenal()
    {
        Time.timeScale = 1;
        penal.SetActive(false);
    }
}
