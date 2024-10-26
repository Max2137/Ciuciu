using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;

public class PlayerDeath : MonoBehaviour
{
    // Referencja do komponentu UniversalHealth
    private UniversalHealth universalHealth;

    // Lista scen do dodania po zresetowaniu gry
    public List<string> StartScenes = new List<string>();

    void Start()
    {
        // Pobierz komponent UniversalHealth przypisany do tego samego obiektu
        universalHealth = GetComponent<UniversalHealth>();

        // Sprawdü, czy komponent zosta≥ prawid≥owo znaleziony
        if (universalHealth == null)
        {
            Debug.LogError("Nie znaleziono komponentu UniversalHealth na obiekcie PlayerDeath!");
        }
    }

    void Update()
    {
        // Sprawdü, czy zdrowie gracza spad≥o do zera lub mniej
        if (universalHealth != null && universalHealth.currentHealth <= 0)
        {
            // Wywo≥aj funkcjÍ resetujπcπ grÍ
            ResetGame();
        }
    }

    void ResetGame()
    {
        SceneManager.LoadScene(StartScenes[0], LoadSceneMode.Single);
        Debug.Log("dodano " + StartScenes[0]);

        for (int i = 1; i < StartScenes.Count; i++)
        {
            SceneManager.LoadScene(StartScenes[i], LoadSceneMode.Additive);
            Debug.Log("dodano " + StartScenes[i]);
        }
    }
}