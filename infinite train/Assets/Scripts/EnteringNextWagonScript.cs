using UnityEngine;

public class EnteringNextWagonScript : MonoBehaviour
{
    public bool isOpened;
    public bool wasOpenedLastFrame; // Dodane pole œledz¹ce stan otwarcia drzwi w poprzedniej klatce
    public WagonLoader wagonLoader;
    public Transform spawnPlace;
    public AudioClip doorOpenSound; // Dodane pole dla dŸwiêku otwierania drzwi

    private AudioSource audioSource;

    private Animator mAnimator; 

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            isOpened = true;
        }
        else
        {
            isOpened = false;
        }

        wasOpenedLastFrame = isOpened; // Inicjalizacja pola

        mAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isOpened)
        {
            Debug.Log("Doors detected a player and no enemies are present in the room.");

            // Przenoszenie gracza do miejsca spawnu przed przejœciem do kolejnego poziomu
            if (spawnPlace != null)
            {
                other.transform.position = spawnPlace.position;
            }
            else
            {
                Debug.LogError("SpawnPlace reference not set in EnteringNextWagonScript.");
            }

            if (wagonLoader != null)
            {
                wagonLoader.LoadNextWagon();
            }
            else
            {
                Debug.LogError("WagonLoader reference not set in EnteringNextWagonScript.");
            }
        }
    }

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            isOpened = true;
        }
        else
        {
            isOpened = false;
        }

        // Sprawdzenie, czy stan otwarcia drzwi w³aœnie zmieni³ siê z false na true
        if (!wasOpenedLastFrame && isOpened)
        {
            if (doorOpenSound != null)
            {
                audioSource.PlayOneShot(doorOpenSound); // Odtwarzanie dŸwiêku otwierania drzwi
                mAnimator.SetTrigger("open");
            }
        }
        
        wasOpenedLastFrame = isOpened; // Aktualizacja stanu otwarcia drzwi w poprzedniej klatce
    }
}