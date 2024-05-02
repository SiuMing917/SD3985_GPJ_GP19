using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyPower : MonoBehaviour
{
    [Header("Fairy Power")]
    public GameObject fairypowerPrefab;
    public float FuseTime = 1f;
    public GameObject player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector2 position = player.transform.position;
            StartCoroutine(useFariyPower(position));
        }
    }

    public void FairyPowerActive(Vector3 playPos)
    {
        //useFariyPower(playPos);
    }

    private IEnumerator useFariyPower(Vector3 playPos)
    {
        Vector2 position = playPos;

        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject power = Instantiate(fairypowerPrefab, position, Quaternion.identity);

        yield return new WaitForSeconds(FuseTime);

        Destroy(power);
    }
}
