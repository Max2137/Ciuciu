using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroSharp : MonoBehaviour
{
    public float attackDamage = 10f;  // Warto�� obra�e� zadanych graczowi
    private WeaponAttack weaponAttack; // Referencja do skryptu WeaponAttack

    private void Start()
    {
        // Znalezienie komponentu WeaponAttack na tym samym obiekcie
        weaponAttack = GetComponent<WeaponAttack>();
        if (weaponAttack == null)
        {
            Debug.LogError("WeaponAttack script not found on this object.");
        }
    }

    // Funkcja wywo�ywana w momencie kolizji z innym obiektem
    private void OnCollisionEnter(Collision collision)
    {
            // Zadajemy obra�enia graczowi
            DealDamageToPlayer(collision.gameObject);
    }

    private void DealDamageToPlayer(GameObject player)
    {
        if (weaponAttack != null)
        {
            // Wywo�anie funkcji zadaj�cej obra�enia z WeaponAttack
            weaponAttack.DealDamage(player, attackDamage);
        }
        else
        {
            Debug.LogError("WeaponAttack reference is null. Cannot deal damage.");
        }
    }
}