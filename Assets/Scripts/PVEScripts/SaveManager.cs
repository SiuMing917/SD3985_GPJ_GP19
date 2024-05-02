using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] string savePath;
    [SerializeField] List<GameObject> itemsPrefabs;

    [SerializeField] private SaveData saveData;
    private void Awake()
    {
        instance = this;
        savePath = Application.persistentDataPath + "/saveFile.dat";
    }
    public void SaveItemsOnMap()
    {
        FindAllSaveItems();
        BinarySaveSystem.WriteToBinaryFile<SaveData>(savePath, saveData);
    }
    private void FindAllSaveItems()
    {
        //saveData.itemsToSave 
        ItemPickup[] itemsOnMap = FindObjectsOfType<ItemPickup>();
        for (int i = 0; i < itemsOnMap.Length; i++)
        {
            saveData.itemsToSave.Add(itemsOnMap[i].GetSaveInfo());
        }
    }
    public void LoadSave()
    {
        saveData = BinarySaveSystem.ReadFromBinaryFile<SaveData>(savePath);
        LoadAllSavedItems();
    }
    private void LoadAllSavedItems()
    {
        //Clear all Items first
        ItemPickup[] itemsOnMap = FindObjectsOfType<ItemPickup>();
        for (int i = 0; i < itemsOnMap.Length; i++)
        {
            Destroy(itemsOnMap[i].gameObject);
        }

        for (int i = 0; i < saveData.itemsToSave.Count; i++)
        {
            GameObject prefab = itemsPrefabs.Find(item => item.GetComponent<ItemPickup>()?.type == saveData.itemsToSave[i].itemType);
            Vector3 posToSpawn = saveData.itemsToSave[i].position;
            if (prefab)
                Instantiate(prefab, posToSpawn, Quaternion.identity);
        }
    }
}
[System.Serializable]
public class SaveData
{
    public List<SavableItem> itemsToSave;
    // add more stuff to be saved
    public string rubbishstring;
}
