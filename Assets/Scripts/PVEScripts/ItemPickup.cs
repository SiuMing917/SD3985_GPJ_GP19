using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [System.Serializable]
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
        SkillGoku,
        SkillFairy,
        WeaponRocket,
        WeaponSpikeArrow,
    }

    public ItemType type;
    public SavableItem GetSaveInfo()
    {
        SavableItem data = new SavableItem();
        data.itemType = this.type;
        data.position = this.transform.position;
        return data;
    }
    private void OnItemPickup(GameObject player)
    {
        Transform skillTransform = player.transform.Find("Skill");
        Transform weaponTransform = player.transform.Find("Weapon");
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++;
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().speed++;
                break;
            case ItemType.SkillGoku:
                foreach (Transform child in skillTransform)
                {
                    child.gameObject.SetActive(false);
                }
                skillTransform.Find("Goku").gameObject.SetActive(true);
                break;
            case ItemType.SkillFairy:
                foreach (Transform child in skillTransform)
                {
                    child.gameObject.SetActive(false);
                }
                skillTransform.Find("Fairy").gameObject.SetActive(true);
                break;
            case ItemType.WeaponRocket:
                foreach (Transform child in weaponTransform)
                {
                    child.gameObject.SetActive(false);
                }
                weaponTransform.Find("Rocket").gameObject.SetActive(true);
                break;
            case ItemType.WeaponSpikeArrow:
                foreach (Transform child in weaponTransform)
                {
                    child.gameObject.SetActive(false);
                }
                weaponTransform.Find("SpikeArrow").gameObject.SetActive(true);
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }

}
