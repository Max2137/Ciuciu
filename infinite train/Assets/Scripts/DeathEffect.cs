using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject effectPrefab; // Prefab efektu �mierci, przypisany bezpo�rednio

    void OnDestroy()
    {
        SpawnEffect();
    }

    void SpawnEffect()
    {
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }
}