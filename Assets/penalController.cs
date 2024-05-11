using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class penalController : MonoBehaviour
{
    [SerializeField] GameObject penal;
    [SerializeField] GameObject gameOverpenal;
    [SerializeField] GameObject playerDeadpenal;
    [SerializeField] List<UpgradeData> upgradeDatas;
    [SerializeField] List<UpgradeButton> upgradeButtons;
    SceneManage sceneManager;

    public void OpenPenal()
    {
 
        Time.timeScale = 0;
        penal.SetActive(true);
        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            upgradeButtons[i].Set(upgradeDatas[i]);
        }

    }

    public void ClosePenal()
    {
        Time.timeScale = 1;
        penal.SetActive(false);
    }
    // SHUFFLE LIST
    public void ShuffleList(List<UpgradeData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public void ShowGameOverPenal()
    {
        Time.timeScale = 0;
        gameOverpenal.SetActive(true);
    }
    public void ShowPlayerDeadPenal()
    {
        Time.timeScale = 0;
        playerDeadpenal.SetActive(true);
    }

    public class Upgrade
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rarity { get; set; }
        public float Increase { get; set; }
    }
    public void ReloadScene()
    {
        sceneManager.LoadScene("PVEGAME");
    }
    public void Back()
    {
        sceneManager.LoadScene("1");
    }
    
}



