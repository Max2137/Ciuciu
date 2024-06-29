using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
    public int destroyAfterSceneChanges = 1; // Liczba zmian sceny przed zniszczeniem obiektu
    private int sceneChangeCount = 0;

    void Awake()
    {
        // Rejestracja funkcji OnSceneLoaded do zdarzenia ³adowania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Upewnij siê, ¿e wyrejestrowujesz funkcjê przy niszczeniu obiektu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Zwiêksz licznik zmian sceny
        sceneChangeCount++;

        // Zniszcz ten obiekt po okreœlonej liczbie zmian sceny
        if (sceneChangeCount >= destroyAfterSceneChanges)
        {
            Destroy(gameObject);
        }
    }
}