using UnityEngine;

public class EffectsBoots : MonoBehaviour
{
    public bool isBurning = false; // Czy postaæ siê pali?

    public GameObject prefabToSpawn; // Prefab, który chcesz zespawnowaæ

    void Update()
    {
        if (isBurning)
        {
            // Pobierz wszystkie obiekty na scenie z komponentem PlayerMovement
            PlayerMovement[] playerMovements = FindObjectsOfType<PlayerMovement>();

            // SprawdŸ isDashing dla ka¿dego znalezionego obiektu PlayerMovement
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