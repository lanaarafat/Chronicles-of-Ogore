using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using TMPro;

[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class InventoryItem
{
    public string itemName;
    public int quantity;
    public float weight;
}

[Serializable]
public class Record
{
    public string playerName;
    public int level;
    public float health;
    public Position position;
    public List<InventoryItem> inventory;
}

[Serializable]
public class Root
{
    public Record record;
}

public class JSONHandler : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    private Root playerData;

    private string url = "https://api.jsonbin.io/v3/b/6686a992e41b4d34e40d06fa";

    private void Start()
    {
        StartCoroutine(FetchData());
    }

    private IEnumerator FetchData()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            playerData = JsonUtility.FromJson<Root>(request.downloadHandler.text);
            Debug.Log("Data fetched successfully!");
        }
        else
        {
            Debug.LogError("Failed to fetch data: " + request.error);
        }
    }

    // Method to display item details in the description text
    public void ShowItemDetails(string itemName)
    {
        if (playerData == null) return;

        InventoryItem item = playerData.record.inventory.Find(i => i.itemName == itemName);

        if (item != null)
        {
            descriptionText.text = $"Name: {item.itemName}\nQuantity: {item.quantity}\nWeight: {item.weight}";
        }
        else
        {
            descriptionText.text = "Item not found!";
        }
    }
}