using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public GameObject effectPrefab; // Prefab efektu śmierci, przypisany bezpośrednio

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