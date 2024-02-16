using UnityEngine;

public class EnteringNextWagonScript : MonoBehaviour
{
    public bool isOpened;
    public WagonLoader wagonLoader;
    public Transform spawnPlace; // Dodane pole do ustawienia obiektu SpawnPlace

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

    }
}