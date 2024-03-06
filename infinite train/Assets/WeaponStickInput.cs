using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStickInput : MonoBehaviour
{
    public float raycastDistance = 5f;  // D³ugoœæ raycasta
    public int attackDamage;
    public float attackCooldown = 1.0f;  // Czas oczekiwania miêdzy atakami
    public float attackPushForce = 10f;  // Si³a odpychaj¹ca obiekt po trafieniu

    private float lastAttackTime;  // Czas ostatniego ataku
    private WeaponInputManager inputManager;

    //INPUT
    public void Start()
    {
        // Uzyskaj referencjê do WeaponInputManager z obiektu rêki (parent)
        inputManager = GetComponentInParent<WeaponInputManager>();

        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found in the parent objects.");
        }
    }

    //INPUT
    public void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && CanAttack() && IsChildOfFirstSlot())
        {
            StickDetect(attackDamage, attackPushForce);
            lastAttackTime = Time.time;
        }
    }

    //DETECTION
    public void StickDetect(float attackDamage, float attackPushForce)
    {
        // Wykonaj raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green);

            // SprawdŸ czy trafiony obiekt ma tag "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // SprawdŸ czy obiekt ma skrypt UniversalHealth
                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null)
                {
                    // Zadaj obra¿enia obiektowi, przekazuj¹c attackDamage
                    GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);

                    // Odpal si³ê odpychaj¹c¹
                    Rigidbody enemyRigidbody = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (enemyRigidbody != null)
                    {
                        Vector3 pushDirection = (hit.collider.transform.position - transform.position).normalized;
                        enemyRigidbody.AddForce(pushDirection * attackPushForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    // SprawdŸ czy mo¿na wykonaæ atak z uwzglêdnieniem cooldownu
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

    // Rysuj linie raycasta w edytorze do celów wizualizacyjnych
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}
