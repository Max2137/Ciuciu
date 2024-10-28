using UnityEngine;

public class AudioTest : MonoBehaviour
{
    // Referencja do komponentu AudioSource
    private AudioSource audioSource;

    // Funkcja wywo³ywana na pocz¹tku
    void Start()
    {
        // Pobranie komponentu AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Funkcja wywo³ywana co klatkê
    void Update()
    {
        // Sprawdzenie, czy naciœniêto klawisz "K"
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Sprawdzenie, czy dŸwiêk jest przypisany i nie jest ju¿ odtwarzany
            if (audioSource != null && !audioSource.isPlaying)
            {
                // Odtworzenie dŸwiêku
                audioSource.Play();
            }
        }
    }
}