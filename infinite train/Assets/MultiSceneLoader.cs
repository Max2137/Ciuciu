using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneLoader : MonoBehaviour
{
    // Lista nazw scen do za³adowania w ramach multi-scen
    [SerializeField]
    private List<string> scenesToLoad;

    // Funkcja do za³adowania multi-scen
    public void LoadScenesOnList()
    {
        // Na pocz¹tku ³adujemy wszystkie nowe sceny z listy
        foreach (string sceneName in scenesToLoad)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }

        // Wywo³anie usuniêcia bie¿¹cej sceny po za³adowaniu nowych scen
        StartCoroutine(UnloadCurrentSceneAfterLoad());
    }

    // Coroutine do wy³adowania bie¿¹cej sceny po za³adowaniu nowych
    private System.Collections.IEnumerator UnloadCurrentSceneAfterLoad()
    {
        // Poczekaj a¿ wszystkie sceny z listy siê za³aduj¹
        yield return new WaitUntil(() => AreScenesLoaded());

        // Wy³adowanie bie¿¹cej sceny (sceny aktywnej)
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    // Sprawdza, czy wszystkie sceny z listy zosta³y za³adowane
    private bool AreScenesLoaded()
    {
        foreach (string sceneName in scenesToLoad)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
                return false;
        }
        return true;
    }
}