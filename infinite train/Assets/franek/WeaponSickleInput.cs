using System.Collections;
using UnityEngine;

public class WeaponSickleInput : MonoBehaviour
{
    public float raycastDistance = 5f;
    public float attackCooldown = 1.0f;
    public int attackDamage = 2;
    public int attackPunctureDamage = 1;
    public float attackPunctureCooldown = 0.5f;

    private float lastAttackTime;
    private bool isAttacking;
    private RaycastHit currentHit;
    private float lastPunctureTime;
    private Transform piercedObject;
    private Vector3 lastPlayerPosition;

    void Update()
    {
        // Dodany warunek sprawdzaj¹cy, czy sickle ma rodzica
        if (transform.parent != null)
        {
            if (Input.GetMouseButtonDown(0) && CanAttack() && IsChildOfFirstSlot())
            {
                StartAttack();
            }

            if (Input.GetMouseButton(0) && isAttacking)
            {
                // Dodane sprawdzenie ruchu gracza
                if (Vector3.Distance(transform.parent.position, lastPlayerPosition) > 0.01f)
                {
                    // Je¿eli gracz siê poruszy³, automatycznie koñcz atak
                    EndAttack();
                }
                else
                {
                    ContinueAttack();
                }
            }

            if (Input.GetMouseButtonUp(0) && isAttacking)
            {
                EndAttack();
            }

            // Aktualizacja pozycji gracza
            lastPlayerPosition = transform.parent.position;
        }
    }

    private void StartAttack()
    {
        if (CanAttack())
        {
            isAttacking = true;
            PerformRaycast();
            lastAttackTime = Time.time;
            lastPunctureTime = Time.time;
        }
    }

    private void ContinueAttack()
    {
        PerformRaycast();
        CheckPunctureDamage();

        if (piercedObject != null)
        {
            // Przesuñ obiekt o sta³¹ odleg³oœæ w kierunku raycasta
            Vector3 newPosition = transform.position + transform.up * raycastDistance;
            piercedObject.position = newPosition;
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        ReleasePiercedObject();
        // Dodane zresetowanie currentHit
        currentHit = new RaycastHit();
    }

    private void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, raycastDistance))
        {
            Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.green);

            if (hit.collider.CompareTag("Enemy"))
            {
                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null && hit.collider != currentHit.collider)
                {
                    GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
                    SetPiercedObject(hit.collider.transform);

                    currentHit = hit;
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up * raycastDistance, Color.red);
        }
    }

    private void SetPiercedObject(Transform objectTransform)
    {
        ReleasePiercedObject();

        piercedObject = objectTransform;
        piercedObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void ReleasePiercedObject()
    {
        if (piercedObject != null)
        {
            piercedObject.GetComponent<Rigidbody>().isKinematic = false;
            piercedObject = null;
        }
    }

    private void CheckPunctureDamage()
    {
        if (Time.time - lastPunctureTime >= attackPunctureCooldown)
        {
            DealPunctureDamage();
            lastPunctureTime = Time.time;
        }
    }

    private void DealPunctureDamage()
    {
        if (currentHit.collider != null && currentHit.collider.CompareTag("Enemy"))
        {
            UniversalHealth enemyHealth = currentHit.collider.gameObject.GetComponent<UniversalHealth>();

            if (enemyHealth != null)
            {
                GetComponent<WeaponAttack>().DealDamage(currentHit.collider.gameObject, attackPunctureDamage);
            }
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    private bool IsChildOfFirstSlot()
    {
        Transform parent = transform.parent;
        return parent != null && parent.CompareTag("1stSlot");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * raycastDistance);
    }
}