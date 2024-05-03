using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class MapCreation
{
    public List<int[,]> mapList;

    public MapCreation(string filePath)
    {
        mapList = new List<int[,]>();

        // Read map data from the file
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] elements = line.Split(',');
            int[,] map = new int[elements.Length, elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                map[i, 0] = int.Parse(elements[i]);
            }
            mapList.Add(map);
        }
    }
    public MapCreation()
    {
        mapList = new List<int[,]>();
        int[,] map1 = new int[,]{{0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0 },
            {0,1,1,0,1,0,0,1,1,1,1,0,0,1,1,0 },
            {0,0,1,0,1,1,0,1,0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,1,0,1,1,0,0,1,1,0,0,0 },
            {0,1,1,0,0,0,0,0,1,0,1,1,0,0,1,0 },
            {0,1,0,0,1,2,0,0,1,0,0,0,0,0,1,0 },
            {0,0,0,0,1,1,1,0,0,0,1,2,0,0,1,0 },
            {0,0,1,0,1,0,1,0,0,0,0,0,0,0,0,0 },
            {1,1,1,0,1,0,1,0,1,1,0,1,1,0,0,0 },
            {0,0,1,0,2,0,1,0,1,1,0,0,0,0,1,1 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,1,0,1,1,1,1,0,0,2,0,0,1,1,1,0 },
            {0,1,0,0,0,0,0,0,1,1,0,0,0,0,1,0 },
            {0,1,1,1,0,0,1,1,1,0,0,1,1,0,1,0 },
            {0,0,0,1,1,0,0,0,0,0,1,1,0,0,0,0 }};
        mapList.Add(map1);
        int[,] map2 = new int[,]{{0,0,0,1,1,0,0,0,1,1,0,0,0,0,0,0},
            {0,1,0,1,0,0,0,0,0,1,0,0,0,1,1,0 },
            {0,1,0,0,0,2,1,0,0,0,0,1,0,0,0,0 },
            {0,1,0,0,1,1,1,1,0,1,1,1,0,0,1,1 },
            {0,0,0,0,1,0,0,0,0,0,0,1,0,0,1,0 },
            {0,0,1,1,1,0,1,0,1,1,0,1,0,0,1,0 },
            {1,0,0,0,1,0,1,0,0,1,0,2,0,0,0,1 },
            {1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0 },
            {1,1,1,0,0,0,0,1,0,0,0,1,0,0,1,0 },
            {0,1,0,0,1,0,0,0,0,1,0,1,0,0,1,0 },
            {0,0,0,0,1,1,0,1,1,1,2,1,0,0,0,1 },
            {0,0,0,1,2,0,0,0,0,1,0,0,0,0,0,0 },
            {0,1,0,1,0,0,0,0,0,1,0,0,0,0,1,0 },
            {0,1,1,1,0,0,1,0,1,0,0,1,1,0,1,0 },
            {0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0 }};
        mapList.Add(map2);
        int[,] map3 = new int[,] { {0,0,1,0,0,1,0,0,0,0,1,0,1,0,0,0 },
        {0,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0 },
        {0,1,0,1,1,1,0,2,1,1,0,1,0,1,0,0 },
        {0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,1 },
        {1,0,0,1,0,1,0,1,0,1,0,0,1,0,1,1 },
        {0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        {0,1,0,0,1,0,1,0,1,0,1,0,1,0,0,0 },
        {0,0,2,0,0,0,0,0,0,0,1,0,2,1,1,0 },
        {0,0,1,1,0,0,1,0,1,0,0,0,0,1,0,0 },
        {1,0,0,1,0,0,0,0,0,0,0,1,0,0,0,1 },
        {1,0,0,0,0,1,0,1,0,1,0,0,1,0,0,0 },
        {0,0,1,1,0,1,0,0,0,0,0,0,0,1,1,0 },
        {0,0,1,0,0,0,0,2,1,0,0,1,0,0,0,0 },
        {0,1,0,0,0,0,1,0,0,0,1,0,0,0,1,0 },
        {0,0,0,1,0,0,0,0,1,0,0,1,1,0,0,0 }};
        mapList.Add(map3);
        int[,] map4 = new int[,] { {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            {0,1,1,0,1,1,1,0,1,1,0,1,0,1,1,0 },
            {0,0,1,0,0,0,0,0,1,0,0,1,0,1,0,0 },
            {1,0,0,0,1,1,0,0,0,0,0,2,0,0,0,1 },
            {1,1,2,0,0,1,0,1,1,1,0,0,0,1,1,1 },
            {0,1,0,0,0,1,0,1,0,0,0,0,0,0,0,0 },
            {0,0,0,1,0,0,0,0,0,0,0,1,1,0,1,1 },
            {0,0,0,1,0,0,1,1,1,1,0,0,1,0,0,0 },
            {1,1,0,1,1,0,0,0,0,0,0,0,1,0,0,0 },
            {0,0,0,0,0,0,0,0,1,0,1,0,0,0,1,0 },
            {1,1,1,0,0,0,1,1,1,0,1,0,2,1,1,1 },
            {1,0,0,0,2,0,0,0,0,0,1,1,0,0,0,1 },
            {0,0,1,0,1,0,0,1,0,0,0,0,0,1,0,0 },
            {0,1,1,0,1,0,1,1,0,1,1,1,0,1,1,0 },
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }};
        mapList.Add(map4);
        int[,] map5 = new int[,]
        {{0,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0 },
        {0,1,0,1,1,0,1,0,1,0,0,0,0,1,1,0 },
        {0,1,0,0,0,0,1,0,1,0,0,1,0,0,1,0 },
        {0,0,0,1,0,1,0,0,0,1,0,1,0,0,0,0 },
        {0,0,0,2,0,0,0,0,0,1,0,2,0,0,1,0 },
        {1,1,0,1,0,0,1,0,0,0,0,0,1,0,0,0 },
        {0,0,0,0,0,1,0,0,1,0,1,0,0,0,1,1 },
        {0,0,0,1,0,0,1,0,1,0,0,0,0,0,0,0 },
        {0,1,0,0,0,0,0,0,0,1,0,1,1,1,0,0 },
        {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1 },
        {0,1,0,1,2,0,0,1,0,0,1,0,1,0,0,0 },
        {1,0,0,0,1,1,0,1,0,1,0,0,2,0,1,0 },
        {0,0,1,0,0,0,0,1,0,0,0,1,1,0,1,0 },
        {0,1,0,0,0,1,0,0,0,1,0,0,1,0,1,0 },
        {0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0 }};
        mapList.Add(map5);
    }

    public int[,] RandomMap()
    {
        int index = Random.Range(0, mapList.Count);
        return mapList[index];

    }

}
