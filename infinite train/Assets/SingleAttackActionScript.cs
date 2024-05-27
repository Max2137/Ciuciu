using UnityEngine;

public class SingleAttackActionScript : MonoBehaviour
{
    public float attackDamage;
    public float detectionRadius = 5f; // Promie� wykrywania

    void Start()
    {
        // Sprawd�, czy obiekt ma komponent WeaponAttack
        WeaponAttack weaponAttack = GetComponentInChildren<WeaponAttack>();

        // Sprawd�, czy komponent zosta� znaleziony
        if (weaponAttack == null)
        {
            Debug.LogError("Nie mo�na znale�� komponentu WeaponAttack na tym obiekcie lub jego dzieciach.");
            return;
        }

        Debug.Log("Trafianie");

        // Znajd� wszystkie collidery w obszarze sferycznym
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            UniversalHealth enemyHealth = collider.gameObject.GetComponent<UniversalHealth>();

            if (enemyHealth != null)
            {
                // U�yj metody DealDamage z WeaponAttack do zadawania obra�e�
                weaponAttack.DealDamage(collider.gameObject, attackDamage);
                Debug.Log(collider.gameObject.name);
            }
        }
    }
}