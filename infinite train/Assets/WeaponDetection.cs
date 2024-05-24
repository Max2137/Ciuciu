using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour
{
    public float raycastDistance = 5f;  // D�ugo�� raycasta
    public int numberOfRays = 9;  // Ilo�� promieni w wachlarzu
    public float fanAngle = 90f;  // K�t wachlarza w stopniach

    private List<GameObject> enemiesHitThisAttack = new List<GameObject>();  // Lista obiekt�w, kt�re ju� otrzyma�y obra�enia

    public RaycastHit[] Detect()
    {
        List<RaycastHit> hits = new List<RaycastHit>();

        // Oblicz k�t pomi�dzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Iteruj przez ka�dy promie� w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            // Oblicz kierunek promienia wachlarza
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.up);
            Vector3 direction = rotation * transform.forward;

            // Wykonaj raycast
            RaycastHit hit;
            Ray ray = new Ray(transform.position, direction);

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Sprawd� czy trafiony obiekt ma tag "Enemy" i nie otrzyma� jeszcze obra�e� w ramach tego ataku
                if (hit.collider.CompareTag("Enemy") && !enemiesHitThisAttack.Contains(hit.collider.gameObject))
                {
                    hits.Add(hit);
                    enemiesHitThisAttack.Add(hit.collider.gameObject);  // Dodaj obiekt do listy, aby unikn�� wielokrotnego zadawania obra�e�
                }
            }
        }
        enemiesHitThisAttack.Clear();  // Wyczy�� list� po ka�dym ataku
        return hits.ToArray();
    }
}