using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEqUiScript : MonoBehaviour
{
    public PlayerEqScript playerEqScript; // Referencja do skryptu zarz¹dzaj¹cego ekwipunkiem

    public Transform carriedItemsPanel; // Panel dla obiektów w CarriedItems
    public Transform categoryCarriedPanel; // Panel dla obiektów w CategoryCarried

    public GameObject itemButtonPrefab; // Prefab przycisku reprezentuj¹cego obiekt

    void Start()
    {
        UpdateUi();
    }

    // Aktualizuje interfejs u¿ytkownika
    public void UpdateUi()
    {
        ClearPanel(carriedItemsPanel);
        ClearPanel(categoryCarriedPanel);

        // Dodaje przyciski dla obiektów w CarriedItems
        foreach (GameObject carriedItem in playerEqScript.CarriedItems)
        {
            AddItemButton(carriedItem, carriedItemsPanel);
        }

        // Dodaje przyciski dla obiektów w CategoryCarried
        foreach (Category category in playerEqScript.ItemsCategories)
        {
            if (category.CategoryCarried != null)
            {
                AddItemButton(category.CategoryCarried, categoryCarriedPanel, category);
            }
        }
    }

    // Dodaje przycisk dla obiektu do panelu
    void AddItemButton(GameObject item, Transform panel, Category category = null)
    {
        GameObject button = Instantiate(itemButtonPrefab, panel);
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = item.name;

        // Dodaje funkcjê wywo³ywan¹ po klikniêciu przycisku
        button.GetComponent<Button>().onClick.AddListener(delegate { OnItemButtonClick(item, category); });
    }

    // Obs³uguje klikniêcie na przycisk obiektu
    void OnItemButtonClick(GameObject item, Category category)
    {
        if (category == null)
        {
            // Klikniêto na obiekt w CarriedItems
            playerEqScript.MoveToCategory(playerEqScript.CarriedItems.IndexOf(item), 0); // Przenosi do pierwszej kategorii
        }
        else
        {
            // Klikniêto na obiekt w CategoryCarried
            playerEqScript.MoveToCarried(playerEqScript.ItemsCategories.IndexOf(category));
        }

        // Aktualizuje interfejs u¿ytkownika po przeniesieniu
        UpdateUi();
    }

    // Czyœci panel z przycisków
    void ClearPanel(Transform panel)
    {
        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
        }
    }
}