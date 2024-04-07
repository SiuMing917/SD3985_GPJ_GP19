using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player1;
    public GameObject player2;
    public Tilemap destructibleTiles;
    private void Awake()
    {
        instance = this;
    }

}
