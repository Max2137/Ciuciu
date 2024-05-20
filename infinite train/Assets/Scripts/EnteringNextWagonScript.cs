using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        mAnimator = GetComponentInChildren<Animator>();
        
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
                StartCoroutine(ExecuteWithDelay());
            }
            else
            {
                Debug.LogError("WagonLoader reference not set in EnteringNextWagonScript.");
            }
        }
    }

    private IEnumerator ExecuteWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        Scene currentScene = SceneManager.GetSceneAt(1);
        Debug.Log(currentScene.name);
        if (currentScene.name == "SceneFightingWagonPlain")
        {
            mAnimator.SetTrigger("close");
        }
    }

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Scene currentScene = SceneManager.GetSceneAt(1);
        if (enemies.Length != 0 && currentScene.name == "SceneFightingWagonPlain")
        {
            isOpened = false;
        }
        else
        {
            isOpened = true;
        }

        // Sprawdzenie, czy stan otwarcia drzwi w³aœnie zmieni³ siê z false na true
        if (!wasOpenedLastFrame && isOpened)
        {
            if (doorOpenSound != null)
            {
                audioSource.clip = doorOpenSound;
                audioSource.Play(); // Odtwarzanie dŸwiêku otwierania drzwi
                Debug.Log("Drzwi otwarto dŸwiêk");
                mAnimator.SetTrigger("open");
            }
        }
       
        wasOpenedLastFrame = isOpened; // Aktualizacja stanu otwarcia drzwi w poprzedniej klatce
    }
}