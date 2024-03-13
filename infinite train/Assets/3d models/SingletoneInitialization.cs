using UnityEngine;

public class SingletonInitialization : MonoBehaviour
{
    private static bool isInitialized = false;

    private void Awake()
    {
        // Je�li nie zosta� jeszcze zainicjowany, utrzymaj obiekt poza scenami
        if (!isInitialized)
        {
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
        }
        else
        {
            // Je�li obiekt ju� by� zainicjowany, upewnij si�, �e ten obiekt r�wnie� jest utrzymywany poza scenami
            if (gameObject.scene.IsValid())
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // Je�li obiekt zosta� ju� wyci�gni�ty ze sceny, zniszcz go
                Destroy(gameObject);
            }
        }
    }
}