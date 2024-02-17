using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHammerInput : MonoBehaviour
{
    public float raycastDistance = 5f;
    public int attackDamage;
    public float attackCooldown = 1.0f;
    public float attackPrepTime = 1.5f;

    private float lastAttackTime;
    private float attackPrepTimer;
    private bool isReadyForAttack;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = transform.position;
    }

    void Update()
    {
        // SprawdŸ zmianê pozycji gracza i zeruj attackPrepTimer jeœli zachodzi
        if (transform.position != lastPlayerPosition)
        {
            attackPrepTimer = 0f;
            isReadyForAttack = false;
        }

        lastPlayerPosition = transform.position;

        // Pozosta³a czêœæ metody Update pozostaje bez zmian

        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            attackPrepTimer = 0f;
        }

        if (Input.GetMouseButton(0) && attackPrepTimer < attackPrepTime)
        {
            attackPrepTimer += Time.deltaTime;

            if (attackPrepTimer >= attackPrepTime)
            {
                isReadyForAttack = true;
                lastAttackTime = Time.time;
                Debug.Log("gotowy");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isReadyForAttack)
            {
                Detect(attackDamage);
            }

            attackPrepTimer = 0f;
            isReadyForAttack = false;
        }
    }

    void Detect(float attackDamage)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.up);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

        Debug.Log("raycastowano");

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Wykryto wroga");

                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null)
                {
                    GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);

                    Debug.Log("zadano damage");
                }
            }
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * raycastDistance);
    }
}