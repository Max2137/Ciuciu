using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Dodaj referencjê do TextMeshPro

[System.Serializable]
public class Item
{
    public string itemName; // Nazwa przedmiotu
    public int quantity;    // Iloœæ przedmiotu

    public Item(string name, int qty)
    {
        itemName = name;
        quantity = qty;
    }
}

public class itemsListScript : MonoBehaviour
{
    // Lista przedmiotów w ekwipunku gracza
    public List<Item> items = new List<Item>();
    public TextMeshProUGUI inventoryText; // Referencja do TextMeshProUGUI

    // Start is called before the first frame update
    void Start()
    {
        UpdateInventoryUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInventoryUI();
    }

    // Funkcja do dodawania przedmiotu do ekwipunku
    public void Gain(string itemName, int amount)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.quantity += amount;
        }
        else
        {
            items.Add(new Item(itemName, amount));
        }
        UpdateInventoryUI();
    }

    // Funkcja do odejmowania przedmiotu z ekwipunku
    public void Pay(string itemName, int amount)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.quantity -= amount;
            if (item.quantity < 0)
            {
                item.quantity = 0; // Zapobiega ujemnej iloœci
            }
        }
        UpdateInventoryUI();
    }

    // Funkcja do sprawdzania iloœci danego przedmiotu
    public int GetQuantity(string itemName)
    {
        Item item = items.Find(i => i.itemName == itemName);
        return item != null ? item.quantity : 0;
    }

    // Funkcja do aktualizowania UI
    private void UpdateInventoryUI()
    {
        inventoryText.text = ""; // Wyczyœæ istniej¹cy tekst

        foreach (Item item in items)
        {
            if (item.quantity > 0)
            {
                inventoryText.text += $"{item.itemName}: {item.quantity}\n";
            }
        }
    }
}