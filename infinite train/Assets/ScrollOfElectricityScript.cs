using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOfElectricityScript : MonoBehaviour
{
    private WeaponDetection weaponDetection;  // Referencja do skryptu WeaponDetection
    private WeaponAttack weaponAttack;        // Referencja do skryptu WeaponAttack
    private CooldownScript cooldownScript;    // Referencja do skryptu CooldownScript

    public float attackDamage = 10f;  // Iloœæ zadawanych obra¿eñ

    private Coroutine attackCoroutine;

    void Awake()
    {
        // Automatyczne znajdowanie skryptów
        weaponDetection = GetComponentInChildren<WeaponDetection>();
        weaponAttack = GetComponentInChildren<WeaponAttack>();
        cooldownScript = GetComponentInChildren<CooldownScript>();

        // Sprawdzenie, czy skrypty zosta³y znalezione
        if (weaponDetection == null)
        {
            Debug.LogError("WeaponDetection script not found!");
        }
        if (weaponAttack == null)
        {
            Debug.LogError("WeaponAttack script not found!");
        }
        if (cooldownScript == null)
        {
            Debug.LogError("CooldownScript script not found!");
        }
    }

    void OnEnable()
    {
        // Uruchomienie zadawania obra¿eñ co okreœlony czas
        attackCoroutine = StartCoroutine(AttackRoutine());
    }

    void OnDisable()
    {
        // Zatrzymanie zadawania obra¿eñ
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (cooldownScript.CanSpawn())
            {
                // Wykryj przeciwników
                RaycastHit[] hits = weaponDetection.Detect();

                // Zadaj obra¿enia wykrytym przeciwnikom
                foreach (RaycastHit hit in hits)
                {
                    weaponAttack.DealDamage(hit.collider.gameObject, attackDamage);
                }

                // Resetuj cooldown
                cooldownScript.ResetCooldown();
            }

            // Czekaj na nastêpn¹ klatkê
            yield return null;
        }
    }
}