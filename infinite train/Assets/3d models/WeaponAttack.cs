using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public bool isBurning;
    public string burningScript;

    public void DealDamage(GameObject enemy, float attackDamage)
    {
        // Sprawdü czy obiekt ma skrypt UniversalHealth
        UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();

        if (isBurning)
        {
            System.Type burningScriptType = System.Type.GetType(burningScript);

            // Sprawdü, czy obiekt ma juø ten komponent
            var existingComponent = enemy.GetComponent(burningScriptType);

            // Jeúli komponent istnieje, usuÒ go
            if (existingComponent != null)
            {
                Destroy(existingComponent);
            }

            // Dodaj komponent ponownie
            enemy.AddComponent(burningScriptType);
        }

        //Debug.Log("Wywo≥ano atak");

        if (enemyHealth != null)
        {
            // Zadaj obraøenia obiektowi
            enemyHealth.TakeDamage(attackDamage, gameObject);
        }
    }
}