using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Category
{
    public string CategoryName;
    public GameObject CategoryPlacement;
    public GameObject CategoryCarried;
}

public class PlayerEqScript : MonoBehaviour
{
    public List<GameObject> CarriedItems = new List<GameObject>();
    public List<Category> ItemsCategories = new List<Category>();

    void Start()
    {
        // Tutaj mo¿esz dodaæ kod inicjalizacyjny, jeœli jest taka potrzeba
    }

    void Update()
    {
        // Tutaj mo¿esz dodaæ kod aktualizacji, jeœli jest taka potrzeba
    }

    // Przenosi obiekt z CarriedItems do CategoryCarried
    public void MoveToCategory(int itemIndex, int categoryIndex)
    {
        if (itemIndex < 0 || itemIndex >= CarriedItems.Count ||
            categoryIndex < 0 || categoryIndex >= ItemsCategories.Count)
        {
            Debug.LogError("Invalid indices.");
            return;
        }

        GameObject itemToMove = CarriedItems[itemIndex];
        Category destinationCategory = ItemsCategories[categoryIndex];

        // Ustawia obiekt jako child CategoryPlacement
        itemToMove.transform.parent = destinationCategory.CategoryPlacement.transform;

        // Ustawia pozycjê i rotacjê obiektu zgodnie z CategoryPlacement
        itemToMove.transform.position = destinationCategory.CategoryPlacement.transform.position;
        itemToMove.transform.rotation = destinationCategory.CategoryPlacement.transform.rotation;

        // Przenosi obiekt z CarriedItems do CategoryCarried
        CarriedItems.RemoveAt(itemIndex);
        destinationCategory.CategoryCarried = itemToMove;
        destinationCategory.CategoryCarried.SetActive(true);
    }

    // Przenosi obiekt z CategoryCarried do CarriedItems
    public void MoveToCarried(int categoryIndex)
    {
        if (categoryIndex < 0 || categoryIndex >= ItemsCategories.Count)
        {
            Debug.LogError("Invalid index.");
            return;
        }

        Category sourceCategory = ItemsCategories[categoryIndex];

        if (sourceCategory.CategoryCarried != null)
        {
            GameObject itemToMove = sourceCategory.CategoryCarried;

            // Usuwa obiekt z CategoryCarried
            sourceCategory.CategoryCarried = null;
            itemToMove.SetActive(false);

            // Dodaje obiekt do CarriedItems
            CarriedItems.Add(itemToMove);

            // Usuwa obiekt jako child CategoryPlacement
            itemToMove.transform.parent = null;
        }
    }
}