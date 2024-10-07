using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
    public enum ActionOnSceneChange
    {
        Destruction,
        PosReset
    }

    public ActionOnSceneChange actionOnSceneChange = ActionOnSceneChange.Destruction; // Domy�lnie ustawione na Destruction
    public int destroyAfterSceneChanges = 1; // Liczba zmian sceny przed podj�ciem akcji
    private int sceneChangeCount = 0;

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
        // Zwi�ksz licznik zmian sceny
        sceneChangeCount++;

        // Podj�cie wybranej akcji po okre�lonej liczbie zmian sceny
        if (sceneChangeCount >= destroyAfterSceneChanges)
        {
            switch (actionOnSceneChange)
            {
                case ActionOnSceneChange.Destruction:
                    Destroy(gameObject);
                    break;

                case ActionOnSceneChange.PosReset:
                    GameObject entryPositioner = GameObject.FindWithTag("EntryPositioner");
                    if (entryPositioner != null)
                    {
                        transform.position = entryPositioner.transform.position;
                    }
                    else
                    {
                        Debug.LogWarning("EntryPositioner tag not found!");
                    }
                    break;
            }
        }
    }
}