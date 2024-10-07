using UnityEngine;

public class SingleAttackActionScript : MonoBehaviour
{
    public float attackDamage;
    public float detectionRadius = 5f; // PromieÒ wykrywania

    void Start()
    {
        // Sprawdü, czy obiekt ma komponent WeaponAttack
        WeaponAttack weaponAttack = GetComponentInChildren<WeaponAttack>();

        // Sprawdü, czy komponent zosta≥ znaleziony
        if (weaponAttack == null)
        {
            Debug.LogError("Nie moøna znaleüÊ komponentu WeaponAttack na tym obiekcie lub jego dzieciach.");
            return;
        }

        Debug.Log("Trafianie");

        // Znajdü wszystkie collidery w obszarze sferycznym
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            UniversalHealth enemyHealth = collider.gameObject.GetComponent<UniversalHealth>();

            if (enemyHealth != null)
            {
                // Uøyj metody DealDamage z WeaponAttack do zadawania obraøeÒ
                weaponAttack.DealDamage(collider.gameObject, attackDamage);
                Debug.Log(collider.gameObject.name);
            }
        }
    }
}