using UnityEngine;
using System.Collections;

public class CompanionAttackShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 2f; // czas odnowienia miêdzy strza³ami
    public float detectionRadius = 10f; // promieñ wykrywania wrogów

    private GameObject nearestEnemy;
    private bool canFire = true;

    void Update()
    {
        FindNearestEnemy();
        if (nearestEnemy != null)
        {
            RotateTowardsEnemy();
            if (canFire)
            {
                StartCoroutine(FireProjectile());
            }
        }
    }

    void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        nearestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hitCollider.gameObject;
                }
            }
        }
    }

    void RotateTowardsEnemy()
    {
        Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 100f);
    }

    IEnumerator FireProjectile()
    {
        canFire = false;

        // Ustawienie rotacji pocisku tylko na osi y
        Quaternion projectileRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectileRotation);
        projectile.GetComponent<ProjectileStandardScript>().SetOwner(gameObject);

        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }
}