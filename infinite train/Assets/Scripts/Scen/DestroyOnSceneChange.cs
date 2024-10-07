using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
    public enum ActionOnSceneChange
    {
        Destruction,
        PosReset
    }

    public ActionOnSceneChange actionOnSceneChange = ActionOnSceneChange.Destruction; // Domyœlnie ustawione na Destruction
    public int destroyAfterSceneChanges = 1; // Liczba zmian sceny przed podjêciem akcji
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

        // Podjêcie wybranej akcji po okreœlonej liczbie zmian sceny
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