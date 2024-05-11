using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterController : MonoBehaviour
{
    private bool isWaterUsed=false;
    public GameObject waterInfo;
    private void Update()
    {
        if(waterInfo.activeSelf)
            waterInfo.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isWaterUsed==false&&collision.CompareTag("Player"))
        {
            GameManager.Instance.playerStats.CurrentHealth = GameManager.Instance.playerStats.MaxHealth;
            GameManager.Instance.playerStats.CurrentEnergy = GameManager.Instance.playerStats.MaxEnergy;
            isWaterUsed = true;
            waterInfo.GetComponent<Text>().text = "It is good right?";
            waterInfo.SetActive(true);
            StartCoroutine(SetInfoFalse());
        }
        else if(collision.CompareTag("Player"))
        {
            waterInfo.GetComponent<Text>().text = "No more water you can drink!";
            waterInfo.SetActive(true);
            StartCoroutine(SetInfoFalse());
        }
    }

    IEnumerator SetInfoFalse()
    {
        yield return new WaitForSeconds(2);
        waterInfo.SetActive(false);
    }
}
