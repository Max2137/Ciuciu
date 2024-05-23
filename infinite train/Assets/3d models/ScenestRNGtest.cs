using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WagonLoader : MonoBehaviour
{
    // Lista scen do losowania
    public List<string> basicWagons = new List<string>();
    public List<string> specialWagons = new List<string>();
    public int specialWagonFrequency = 5; // Czêstotliwoœæ losowania sceny specjalnej

    private int basicWagonCount = 0; // Licznik wagonów podstawowych
    private int specialWagonCount = 0; // Licznik wagonów specjalnych
    private string currentSceneName; // Przechowuje nazwê aktualnie za³adowanej sceny

    // Funkcja do ³adowania kolejnego wagonu
    public void LoadNextWagon()
    {
        // SprawdŸ, czy istnieje poprzednia scena, jeœli tak, usuñ j¹
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
        }
        else
        {
            // Jeœli currentSceneName jest puste, usuñ scenê o nazwie "SceneStart"
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
            basicWagonCount = 0; // Zresetowanie licznika wagonów podstawowych
        }

        specialWagonCount++;

        // £adowanie nowej sceny, zastêpuj¹c obecn¹
        SceneManager.LoadScene(randomSceneName, LoadSceneMode.Additive);
        currentSceneName = randomSceneName; // Aktualizacja nazwy aktualnie za³adowanej sceny

        Debug.Log("Za³adowano nowy wagon: " + randomSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        // Sprawdzanie, czy klawisz CTRL zosta³ naciœniêty
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // Wywo³anie funkcji LoadNextWagon po naciœniêciu klawisza CTRL
            //LoadNextWagon();
        }
    }
}