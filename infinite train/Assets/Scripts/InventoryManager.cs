using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Add(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += amount;
        else
            items[itemName] = amount;
    }

    public void Pay(string itemName, int amount)
    {
        if (items.ContainsKey(itemName))
            items[itemName] -= amount;
        // Optionally handle cases where the item is not found
    }

    public int GetQuantity(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }
}