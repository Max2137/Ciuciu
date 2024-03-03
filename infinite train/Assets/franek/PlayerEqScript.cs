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
    // Lista przechowuj¹ca aktualnie niesione przedmioty
    public List<GameObject> CarriedItems = new List<GameObject>();

    // Lista kategorii przedmiotów
    public List<Category> ItemsCategories = new List<Category>();

    // Start is called before the first frame update
    void Start()
    {
        // Tutaj mo¿esz dodaæ kod inicjalizacyjny, jeœli jest taka potrzeba
    }

    // Update is called once per frame
    void Update()
    {
        // Tutaj mo¿esz dodaæ kod aktualizacji, jeœli jest taka potrzeba
    }
}