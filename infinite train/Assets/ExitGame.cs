using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Metoda zamykaj¹ca grê
    public void QuitGame()
    {
        // Wyjœcie z gry
        Application.Quit();

        // Dodatkowe wyjœcie dla trybu gry w edytorze Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}