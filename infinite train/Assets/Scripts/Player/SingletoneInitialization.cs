using UnityEngine;

public class SingletonInitialization : MonoBehaviour
{
    private static bool isInitialized = false;

    private void Awake()
    {
        // Jeœli nie zosta³ jeszcze zainicjowany, utrzymaj obiekt poza scenami
        if (!isInitialized)
        {
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
        }
        else
        {
            // Jeœli obiekt ju¿ by³ zainicjowany, upewnij siê, ¿e ten obiekt równie¿ jest utrzymywany poza scenami
            if (gameObject.scene.IsValid())
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // Jeœli obiekt zosta³ ju¿ wyci¹gniêty ze sceny, zniszcz go
                Destroy(gameObject);
            }
        }
    }
}