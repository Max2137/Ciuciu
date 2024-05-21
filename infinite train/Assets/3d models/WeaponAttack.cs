using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public bool isBurning;
    public string burningScript;

    public void DealDamage(GameObject enemy, float attackDamage)
    {
        // Sprawd� czy obiekt ma skrypt UniversalHealth
        UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();

        if (isBurning)
        {
            System.Type burningScriptType = System.Type.GetType(burningScript);

            // Sprawd�, czy obiekt ma ju� ten komponent
            var existingComponent = enemy.GetComponent(burningScriptType);

            // Je�li komponent istnieje, usu� go
            if (existingComponent != null)
            {
                Destroy(existingComponent);
            }

            // Dodaj komponent ponownie
            enemy.AddComponent(burningScriptType);
        }

        //Debug.Log("Wywo�ano atak");

        if (enemyHealth != null)
        {
            // Zadaj obra�enia obiektowi
            enemyHealth.TakeDamage(attackDamage, gameObject);
        }
    }
}