using UnityEngine;

public class EnemyBehaviourBullet : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefab pocisku
    public Transform firePoint;           // Punkt, z którego bêdzie wystrzeliwany pocisk

    void Awake()
    {
        this.enabled = false;
    }

    void OnEnable()
    {
        FireProjectile();
        this.enabled = false; // Dezaktywacja tylko tego skryptu
    }

    private void FireProjectile()
    {
        // Ustawienie rotacji pocisku tylko na osi y
        Quaternion projectileRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Zespanowanie pocisku
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectileRotation);
        projectile.GetComponent<ProjectileStandardScript>().SetOwner(gameObject);
    }
}