using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public int BeatenWagons = 0;
    public TextMeshProUGUI scoreText;
    private EnemiesSpawnScript enemiesSpawnScript;

    void Start()
    {
        BeatenWagons = 1;

        SceneManager.sceneLoaded += OnSceneLoaded;

        UpdateScoreText();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        enemiesSpawnScript = FindObjectOfType<EnemiesSpawnScript>();

        // SprawdŸ, czy za³adowana scena to "SceneStart", jeœli tak, zresetuj liczbê wagonów
        if (scene.name == "SceneStart")
        {
            BeatenWagons = 1;
            UpdateScoreText();
        }
        else
        {
            // W przeciwnym razie zwiêksz liczbê wagonów
            IncreaseBeatenWagons();
        }
    }

    public void IncreaseBeatenWagons()
    {
        BeatenWagons++;
        UpdateScoreText();
        Debug.Log(BeatenWagons);
        enemiesSpawnScript.NewWagon();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Current wagon: " + BeatenWagons;
        }
        else
        {
            //Debug.LogError("Referencja do obiektu TextMeshProUGUI nie zosta³a przypisana!");
        }
    }
}