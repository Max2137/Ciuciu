using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
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
        // Zniszcz ten obiekt, gdy nowa scena zostanie za³adowana
        Destroy(gameObject);
    }
}