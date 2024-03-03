using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerEqUiScript : MonoBehaviour
{
    public PlayerEqScript playerEqScript;
    public TextMeshProUGUI carriedItemsText;
    public TextMeshProUGUI categoriesText;

    public Transform carriedItemsPanel;
    public Transform categoriesPanel; // Dodane pole do przechowywania panelu kategorii
    public GameObject buttonPrefab;

    private List<GameObject> spawnedCarriedItemButtons = new List<GameObject>();
    private List<GameObject> spawnedCategoryButtons = new List<GameObject>();

    void Update()
    {
        DisplayCarriedItems();
        DisplayCategories();
    }

    void DisplayCarriedItems()
    {
        spawnedCarriedItemButtons.RemoveAll(button => !playerEqScript.CarriedItems.Contains(button.GetComponent<CarriedItemReference>().Item));

        Vector2 buttonPosition = Vector2.zero;

        foreach (GameObject item in playerEqScript.CarriedItems)
        {
            GameObject existingButton = spawnedCarriedItemButtons.Find(button => button.GetComponent<CarriedItemReference>().Item == item);

            if (existingButton == null)
            {
                GameObject button = Instantiate(buttonPrefab, carriedItemsPanel);
                button.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
                button.GetComponent<RectTransform>().anchoredPosition = buttonPosition;

                spawnedCarriedItemButtons.Add(button);

                button.AddComponent<CarriedItemReference>().Item = item;

                button.GetComponent<Button>().onClick.AddListener(() => OnCarriedItemClick(item));
            }

            buttonPosition.y -= 65f;
        }
    }

    void DisplayCategories()
    {
        spawnedCategoryButtons.RemoveAll(button => !playerEqScript.ItemsCategories.Contains(button.GetComponent<CategoryReference>().Category));

        Vector2 buttonPosition = Vector2.zero;

        foreach (Category category in playerEqScript.ItemsCategories)
        {
            GameObject existingButton = spawnedCategoryButtons.Find(button => button.GetComponent<CategoryReference>().Category == category);

            if (existingButton == null)
            {
                GameObject button = Instantiate(buttonPrefab, categoriesPanel);
                button.GetComponentInChildren<TextMeshProUGUI>().text = category.CategoryName;
                button.GetComponent<RectTransform>().anchoredPosition = buttonPosition;

                spawnedCategoryButtons.Add(button);

                button.AddComponent<CategoryReference>().Category = category;

                button.GetComponent<Button>().onClick.AddListener(() => OnCategoryClick(category));
            }

            buttonPosition.y -= 65f;
        }
    }

    void OnCarriedItemClick(GameObject item)
    {
        Debug.Log("Clicked on carried item: " + item.name);
    }

    void OnCategoryClick(Category category)
    {
        Debug.Log("Clicked on category: " + category.CategoryName);
    }
}

public class CarriedItemReference : MonoBehaviour
{
    public GameObject Item { get; set; }
}

public class CategoryReference : MonoBehaviour
{
    public Category Category { get; set; }
}