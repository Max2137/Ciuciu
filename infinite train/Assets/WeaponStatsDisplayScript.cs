using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WeaponStatsDisplayScript : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI NameSpace;
    public TextMeshProUGUI DescriptionSpace;
    public UnityEngine.UI.Image uiImage; // Dodane pole Image
    public string itemTag = "Item";
    public float searchingDistance = 10f;

    private WeaponStatsData currentWeaponStatsData;

    void Update()
    {
        SearchForNearestItem();
        UpdateUI();
        UpdateUIImage(); // Nowa funkcja do aktualizacji Image
    }

    void SearchForNearestItem()
    {
        GameObject nearestItem = null;
        float nearestDistance = float.MaxValue;

        // Znajdü wszystkie obiekty z tagiem "Item" w okreúlonym promieniu
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchingDistance);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(itemTag))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // Sprawdü, czy to jest najbliøszy obiekt
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestItem = collider.gameObject;
                }
            }
        }

        // Jeúli znaleziono najbliøszy obiekt, pobierz WeaponStatsData z niego
        if (nearestItem != null)
        {
            WeaponStatsData weaponStatsData = nearestItem.GetComponent<WeaponStatsData>();
            if (weaponStatsData != null)
            {
                currentWeaponStatsData = weaponStatsData;
            }
        }
        else
        {
            // Jeúli nie znaleziono øadnego obiektu, zresetuj aktualne WeaponStatsData
            currentWeaponStatsData = null;
        }
    }

    void UpdateUI()
    {
        // Sprawdü, czy obiekt TextMeshProUGUI zosta≥ przypisany
        if (displayText == null)
        {
            Debug.LogError("Nie przypisano obiektu TextMeshProUGUI.");
            return;
        }

        // WyczyúÊ tekst przed dodaniem nowych informacji
        displayText.text = "";

        // Sprawdü, czy obiekt WeaponStatsData zosta≥ znaleziony
        if (currentWeaponStatsData != null)
        {
            // Aktualizuj treúÊ obiektu TextMeshProUGUI na podstawie zmiennych z WeaponStatsData
            if (NameSpace != null)
            {
                NameSpace.text = currentWeaponStatsData.Name;
            }

            if (DescriptionSpace != null)
            {
                DescriptionSpace.text = currentWeaponStatsData.Description;
            }

            // Uzyskaj statystyki z WeaponStatsData
            Dictionary<string, object> stats = currentWeaponStatsData.GetStats();

            // Aktualizuj tekst na podstawie uzyskanych statystyk
            foreach (var stat in stats)
            {
                displayText.text += stat.Key + ": " + stat.Value + "\n";
            }
        }
        else
        {
            // Jeúli currentWeaponStatsData jest null, ustaw teksty na puste
            if (NameSpace != null)
            {
                NameSpace.text = "";
            }

            if (DescriptionSpace != null)
            {
                DescriptionSpace.text = "";
            }
        }
    }

    void UpdateUIImage()
    {
        // Sprawdü, czy obiekt Image zosta≥ przypisany
        if (uiImage != null)
        {
            // Ustaw stan aktywnoúci obiektu Image w zaleønoúci od obecnoúci currentWeaponStatsData
            uiImage.gameObject.SetActive(currentWeaponStatsData != null);
        }
    }
}