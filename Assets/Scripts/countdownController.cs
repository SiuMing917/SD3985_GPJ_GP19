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
    public GameObject otherGameObject;

    public bool gamePlaying { get; private set; }

    private void Start()
    {
        
        StartCoroutine(CountdownToStart());
    }
    IEnumerator CountdownWaveTime()
    {
        otherGameObject.SetActive(true);
        while (waveTime > 0)
        {
            
            waveTimeCountDisplay.text = waveTime.ToString();

            yield return new WaitForSeconds(1f);

            waveTime--;
            
        }
        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
        // show store/ upgrade page
    }
    IEnumerator CountdownToStart()
    {

        otherGameObject.SetActive(false);
        while (countdownTime > 0)
        {
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
    private void BeginGame()
    {
        gamePlaying = true;
        
    }
}
