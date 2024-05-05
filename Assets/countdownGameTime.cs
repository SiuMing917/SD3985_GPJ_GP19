using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdownGameTime : MonoBehaviour
{
    public int countdowninGameTime;
    public Text countdownDisplay;

    private void Start()
    {
        StartCoroutine(CountdowninGameTime());
    }
    IEnumerator CountdowninGameTime()
    {   
        while (countdowninGameTime > 0)
        {
            countdownDisplay.text = countdowninGameTime.ToString();

            yield return new WaitForSeconds(1f);

            countdowninGameTime--;
        }

        

        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
        // show store/ upgrade page
    }


}
