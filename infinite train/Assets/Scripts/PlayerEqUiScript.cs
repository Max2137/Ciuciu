using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEqUiScript : MonoBehaviour
{
    public PlayerEqScript playerEqScript; // Referencja do skryptu zarz�dzaj�cego ekwipunkiem

    public Transform carriedItemsPanel; // Panel dla obiekt�w w CarriedItems
    public Transform categoryCarriedPanel; // Panel dla obiekt�w w CategoryCarried

    public GameObject itemButtonPrefab; // Prefab przycisku reprezentuj�cego obiekt

    void Start()
    {
        UpdateUi();
    }

    // Aktualizuje interfejs u�ytkownika
    public void UpdateUi()
    {
        ClearPanel(carriedItemsPanel);
        ClearPanel(categoryCarriedPanel);

        // Dodaje przyciski dla obiekt�w w CarriedItems
        foreach (GameObject carriedItem in playerEqScript.CarriedItems)
        {
            AddItemButton(carriedItem, carriedItemsPanel);
        }

        // Dodaje przyciski dla obiekt�w w CategoryCarried
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

        // Dodaje funkcj� wywo�ywan� po klikni�ciu przycisku
        button.GetComponent<Button>().onClick.AddListener(delegate { OnItemButtonClick(item, category); });
    }

    // Obs�uguje klikni�cie na przycisk obiektu
    void OnItemButtonClick(GameObject item, Category category)
    {
        if (category == null)
        {
            // Klikni�to na obiekt w CarriedItems
            playerEqScript.MoveToCategory(playerEqScript.CarriedItems.IndexOf(item), 0); // Przenosi do pierwszej kategorii
        }
        else
        {
            // Klikni�to na obiekt w CategoryCarried
            playerEqScript.MoveToCarried(playerEqScript.ItemsCategories.IndexOf(category));
        }

        // Aktualizuje interfejs u�ytkownika po przeniesieniu
        UpdateUi();
    }

    // Czy�ci panel z przycisk�w
    void ClearPanel(Transform panel)
    {
        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
        }
    }
}