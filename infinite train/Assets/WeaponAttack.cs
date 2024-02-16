using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public void DealDamage(GameObject enemy, float attackDamage)
    {
        // Sprawdü czy obiekt ma skrypt UniversalHealth
        UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();

        //Debug.Log("Wywo≥ano atak");

        if (enemyHealth != null)
        {
            // Zadaj obraøenia obiektowi
            enemyHealth.TakeDamage(attackDamage, gameObject);
        }
    }
}