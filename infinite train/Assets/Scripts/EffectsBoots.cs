using UnityEngine;

public class EffectsBoots : MonoBehaviour
{
    public bool isBurning = false; // Czy posta� si� pali?

    public GameObject prefabToSpawn; // Prefab, kt�ry chcesz zespawnowa�

    void Update()
    {
        if (isBurning)
        {
            // Pobierz wszystkie obiekty na scenie z komponentem PlayerMovement
            PlayerMovement[] playerMovements = FindObjectsOfType<PlayerMovement>();

            // Sprawd� isDashing dla ka�dego znalezionego obiektu PlayerMovement
            foreach (PlayerMovement playerMovement in playerMovements)
            {
                if (playerMovement.isDashing)
                {
                        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.1f;
                        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}