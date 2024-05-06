using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdownController : MonoBehaviour
{
    [Header("CountdownTime")]
    //public Text enemyCounter;
    public Text waveTimeCountDisplay;
    public Text countdownDisplay;
    public int countdownTime;
    public int waveTime;
    //public PlayerInput playerInput;
    public bool isgamePlaying { get; private set; }
    private static countdownController instance1;
    public static countdownController Instance1 { get => instance1; set => instance1 = value; }
    public GameObject shopPenal;

    [SerializeField] List<UpgradeData> upgrades;
    [SerializeField] List<UpgradeButton> upgradeButtons;
    List<UpgradeData> selectedUpgrades;

    [SerializeField] List<UpgradeData> aquiredUpgrades;

    void Start()
    {

        StartCoroutine(CountdownToStart());
        //ShowShopPenal(GetUpgrades(3));

    }
    IEnumerator CountdownToStart()
    {
        countdownDisplay.gameObject.SetActive(true);

        while (countdownTime > 0)
        {
            Input.ResetInputAxes();
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownDisplay.text = "Start!";
        BeginGame();
        //GameController.Instance.BeginGame();
        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
        waveTimeCountDisplay.gameObject.SetActive(true);
        StartCoroutine(CountdownWaveTime());
    }
    IEnumerator CountdownWaveTime()
    {
        
        while (waveTime >= 0)
        {
            
            waveTimeCountDisplay.text = waveTime.ToString();

            yield return new WaitForSeconds(1f);

            waveTime--;
            
        }
        yield return new WaitForSeconds(1f);

        waveTimeCountDisplay.gameObject.SetActive(false);
        UpgradePhase();
        // show store/ upgrade page
    }
    
    private void BeginGame()
    {
        isgamePlaying = true;
        
    }
    private void UpgradePhase()
    {
        isgamePlaying = false;
        ShowShopPenal(upgrades);
    }
    public void ShowShopPenal(List<UpgradeData>upgradeDatas)
    {
        shopPenal.SetActive(true);
        for(int i = 0; i< upgradeDatas.Count; i++)
        {
            upgradeButtons[i].Set(upgradeDatas[i]);
        }
    }

    public void  CloseShopPenal()
    {
        shopPenal.SetActive(false);
    }
    public List<UpgradeData> GetUpgrades(int count)
    {
        
        List<UpgradeData> upgradeList = new List<UpgradeData>();
        //Debug.Log(count);

        if (count> upgrades.Count)
        {
            count = upgrades.Count;
            Debug.Log("count1"+ count);
        }

        for (int i = 0; i< count; i++)
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
        CloseShopPenal();

    }



}
