using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
    void Awake()
    {
        // Rejestracja funkcji OnSceneLoaded do zdarzenia �adowania sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // Upewnij si�, �e wyrejestrowujesz funkcj� przy niszczeniu obiektu
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Zniszcz ten obiekt, gdy nowa scena zostanie za�adowana
        Destroy(gameObject);
    }
}