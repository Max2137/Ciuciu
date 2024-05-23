using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WagonLoader : MonoBehaviour
{
    // Lista scen do losowania
    public List<string> basicWagons = new List<string>();
    public List<string> specialWagons = new List<string>();
    public int specialWagonFrequency = 5; // Cz�stotliwo�� losowania sceny specjalnej

    private int basicWagonCount = 0; // Licznik wagon�w podstawowych
    private int specialWagonCount = 0; // Licznik wagon�w specjalnych
    private string currentSceneName; // Przechowuje nazw� aktualnie za�adowanej sceny

    // Funkcja do �adowania kolejnego wagonu
    public void LoadNextWagon()
    {
        // Sprawd�, czy istnieje poprzednia scena, je�li tak, usu� j�
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
        }
        else
        {
            // Je�li currentSceneName jest puste, usu� scen� o nazwie "SceneStart"
            SceneManager.UnloadSceneAsync("SceneStart");
        }

        string randomSceneName;

        if (basicWagonCount < specialWagonFrequency)
        {
            // Losowanie sceny z listy basicWagons
            int randomIndex = Random.Range(0, basicWagons.Count);
            randomSceneName = basicWagons[randomIndex];
            basicWagonCount++;
        }
        else
        {
            // Losowanie sceny z listy specialWagons
            int randomIndex = Random.Range(0, specialWagons.Count);
            randomSceneName = specialWagons[randomIndex];
            basicWagonCount = 0; // Zresetowanie licznika wagon�w podstawowych
        }

        specialWagonCount++;

        // �adowanie nowej sceny, zast�puj�c obecn�
        SceneManager.LoadScene(randomSceneName, LoadSceneMode.Additive);
        currentSceneName = randomSceneName; // Aktualizacja nazwy aktualnie za�adowanej sceny

        Debug.Log("Za�adowano nowy wagon: " + randomSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        // Sprawdzanie, czy klawisz CTRL zosta� naci�ni�ty
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // Wywo�anie funkcji LoadNextWagon po naci�ni�ciu klawisza CTRL
            //LoadNextWagon();
        }
    }
}