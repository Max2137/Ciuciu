using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpearInput : MonoBehaviour
{
    public float raycastDistance = 5f;  // D�ugo�� raycasta
    public int attackDamage;
    public int attackPuncture = 1;  // Przebicie, czyli ilo�� wrog�w, przez kt�r� przebije si� w��cznia
    public float attackCooldown = 1.0f;  // Czas oczekiwania mi�dzy atakami

    private float lastAttackTime;  // Czas ostatniego ataku

    //INPUT
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack() && IsChildOfFirstSlot())
        {
            SpearDetect(attackDamage, attackPuncture);
            lastAttackTime = Time.time;
        }
    }

    //DETECTION
    public void SpearDetect(float attackDamage, int attackPuncture)
    {
        // Wykonaj raycast
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.up, raycastDistance);
        Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.green);

        // Iteruj przez trafienia z uwzgl�dnieniem attackPuncture
        for (int i = 0; i < Mathf.Min(hits.Length, attackPuncture); i++)
        {
            // Sprawd� czy trafiony obiekt ma tag "Enemy"
            if (hits[i].collider.CompareTag("Enemy"))
            {
                // Sprawd� czy obiekt ma skrypt UniversalHealth
                UniversalHealth enemyHealth = hits[i].collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null)
                {
                    // Zadaj obra�enia obiektowi, przekazuj�c attackDamage
                    GetComponent<WeaponAttack>().DealDamage(hits[i].collider.gameObject, attackDamage);
                }
            }
        }
    }

    // Sprawd� czy mo�na wykona� atak z uwzgl�dnieniem cooldownu
    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    // Dodatkowa metoda do sprawdzania, czy obiekt jest dzieckiem obiektu z tagiem "1stSlot"
    private bool IsChildOfFirstSlot()
    {
        Transform parent = transform.parent;
        return parent != null && parent.CompareTag("1stSlot");
    }

    // Rysuj linie raycasta w edytorze do cel�w wizualizacyjnych
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * raycastDistance);
    }
}
