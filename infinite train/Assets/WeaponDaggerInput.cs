using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDaggerInput : MonoBehaviour
{
    public float raycastDistance = 5f;  // D�ugo�� raycasta
    public int attackDamage;
    public float attackCooldown = 1.0f;  // Czas oczekiwania mi�dzy atakami

    private float lastAttackTime;  // Czas ostatniego ataku

    //INPUT
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack() && IsChildOfFirstSlot())
        {
            Detect(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    //DETECTION
    public void Detect(float attackDamage)
    {
        // Wykonaj raycast
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.up);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Sprawd� czy trafiony obiekt ma tag "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // Sprawd� czy obiekt ma skrypt UniversalHealth
                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null)
                {
                    // Zadaj obra�enia obiektowi, przekazuj�c attackDamage
                    GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
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