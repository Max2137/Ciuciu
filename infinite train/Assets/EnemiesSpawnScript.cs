using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SpectrumRange
{
    public float from;
    public float to;
}

[System.Serializable]
public class ListElement
{
    public GameObject prefab;
    public int difficulty;
    public SpectrumRange spectrum;
    public int limit; // Nowa w�a�ciwo�� - limit spawnowania
}

public class EnemiesSpawnScript : MonoBehaviour
{
    public float runModeModifier;
    public float difficultyScore;
    public ScoreScript scoreScript;

    // Dodajemy list� element�w
    public List<ListElement> elements = new List<ListElement>();

    // Lista pierwotnych limit�w
    private List<int> originalLimits = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        // Inicjalizacja listy pierwotnych limit�w
        foreach (ListElement element in elements)
        {
            originalLimits.Add(element.limit);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewWagon()
    {
        // Usuwanie wszystkich poprzednio zespawnowanych przeciwnik�w
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }

        Scene currentScene = SceneManager.GetSceneAt(1);
        Debug.Log("Aktualna scena: " + currentScene.name);

        if (currentScene.name == "SceneFightingWagonPlain" || currentScene.name == "SceneFightingWagonRun")
        {
            // Implementacja logiki tworzenia nowego wagonu
            List<GameObject> newWagonPrefabs = new List<GameObject>();
            difficultyScore = scoreScript.BeatenWagons * 10;
            float remainingDifficulty = difficultyScore;

            if (currentScene.name == "SceneFightingWagonRun")
            {
                remainingDifficulty = remainingDifficulty * runModeModifier;
            }

            Debug.Log(remainingDifficulty);

            int beatenWagons = scoreScript.BeatenWagons;

            // Filtrujemy elementy, kt�re s� w zakresie spectrum bazuj�c na beatenWagons
            List<ListElement> validElements = elements.FindAll(e => e.spectrum.from <= beatenWagons && e.spectrum.to >= beatenWagons);

            while (remainingDifficulty > 0 && validElements.Count > 0)
            {
                // Losujemy element z listy validElements
                int randomIndex = Random.Range(0, validElements.Count);
                ListElement selectedElement = validElements[randomIndex];

                // Sprawdzamy, czy limit nie zosta� osi�gni�ty
                if (selectedElement.limit <= 0)
                {
                    validElements.RemoveAt(randomIndex);
                    continue; // Przechodzimy do kolejnego elementu
                }

                if (selectedElement.difficulty <= remainingDifficulty)
                {
                    newWagonPrefabs.Add(selectedElement.prefab);
                    remainingDifficulty -= selectedElement.difficulty;
                    Debug.Log("zabrano: " + selectedElement.difficulty + " zosta�o: " + remainingDifficulty);

                    // Zmniejszamy limit
                    selectedElement.limit--;
                }
                else
                {
                    // Je�li element jest zbyt trudny, usuwamy go z listy validElements
                    validElements.RemoveAt(randomIndex);
                }
            }

            // Spawnowanie prefab�w (przyk�ad, musisz dostosowa� do swojego systemu)
            foreach (GameObject prefab in newWagonPrefabs)
            {
                Instantiate(prefab, transform.position, transform.rotation);
                Debug.Log(prefab.name);
            }

            // Po zako�czeniu tworzenia wagonu resetujemy limity
            ResetLimits();
        }
    }

    // Metoda resetuj�ca limity do ich pierwotnych warto�ci
    private void ResetLimits()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].limit = originalLimits[i]; // Resetujemy limit dla ka�dego elementu
        }
    }
}