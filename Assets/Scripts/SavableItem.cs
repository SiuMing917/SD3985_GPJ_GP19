using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavableItem
{
    public Vector3 position
    {
        get { return new Vector3(posX, posY, posZ); }
        set { posX = value.x; posY = value.y; posZ = value.z; }
    }
    public ItemPickup.ItemType itemType;
    public float posX;
    public float posY;
    public float posZ;
}
