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

    void Start()
    {

        StartCoroutine(CountdownToStart());
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
        
        while (waveTime > 0)
        {
            
            waveTimeCountDisplay.text = waveTime.ToString();

            yield return new WaitForSeconds(1f);

            waveTime--;
            
        }
        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
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
        Invoke("ShowShopPenal", 1.25f);
    }
    private void ShowShopPenal()
    {
        shopPenal.SetActive(true);
    }

}
