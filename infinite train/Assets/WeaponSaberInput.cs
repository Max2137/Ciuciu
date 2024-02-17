using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaberInput : MonoBehaviour
{
    public float raycastDistance = 5f;  // D³ugoœæ raycasta
    public int attackDamage;
    public float attackCooldown = 1.0f;  // Czas oczekiwania miêdzy atakami
    public int numberOfRays = 9;  // Iloœæ promieni w wachlarzu
    public float fanAngle = 90f;  // K¹t wachlarza w stopniach

    private float lastAttackTime;  // Czas ostatniego ataku
    private List<GameObject> enemiesHitThisAttack = new List<GameObject>();  // Lista obiektów, które ju¿ otrzyma³y obra¿enia

    //INPUT
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack() && IsChildOfFirstSlot())
        {
                FanDetect(attackDamage);
                lastAttackTime = Time.time;
                enemiesHitThisAttack.Clear();  // Wyczyœæ listê po ka¿dym ataku
        }
    }

    //DETECTION
    public void FanDetect(float attackDamage)
    {
        // Oblicz k¹t pomiêdzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Iteruj przez ka¿dy promieñ w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            // Oblicz kierunek promienia wachlarza
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.forward);
            Vector3 direction = rotation * transform.up;

            // Wykonaj raycast
            RaycastHit hit;
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // SprawdŸ czy trafiony obiekt ma tag "Enemy" i nie otrzyma³ jeszcze obra¿eñ w ramach tego ataku
                if (hit.collider.CompareTag("Enemy") && !enemiesHitThisAttack.Contains(hit.collider.gameObject))
                {
                    // Dodaj obiekt do listy, aby unikn¹æ wielokrotnego zadawania obra¿eñ
                    enemiesHitThisAttack.Add(hit.collider.gameObject);

                    // SprawdŸ czy obiekt ma skrypt UniversalHealth
                    UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                    if (enemyHealth != null)
                    {
                        // Zadaj obra¿enia obiektowi, przekazuj¹c attackDamage
                        GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
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

    // Rysuj linie raycastów w edytorze do celów wizualizacyjnych
    void OnDrawGizmos()
    {
        // Oblicz k¹t pomiêdzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Rysuj ka¿dy promieñ w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.forward);
            Vector3 direction = rotation * transform.up;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * raycastDistance);
        }
    }
}
