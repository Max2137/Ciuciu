using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour
{
    public float raycastDistance = 5f;  // D�ugo�� raycasta
    public int numberOfRays = 9;  // Ilo�� promieni w wachlarzu
    public float fanAngle = 90f;  // K�t wachlarza w stopniach
    public int puncture = 2;  // Liczba obiekt�w, kt�re raycast mo�e przebi�
    public int raycastDirection;

    private List<GameObject> enemiesHitThisAttack = new List<GameObject>();  // Lista obiekt�w, kt�re ju� otrzyma�y obra�enia

    public RaycastHit[] Detect()
    {
        Debug.Log("Odebrano");

        List<RaycastHit> hits = new List<RaycastHit>();

        // Oblicz k�t pomi�dzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Iteruj przez ka�dy promie� w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            // Oblicz kierunek promienia wachlarza
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.up);
            Quaternion adjustedRotation = Quaternion.Euler(0f, raycastDirection, 0f) * rotation; // Dodaj 90 stopni rotacji w osi Y
            Vector3 direction = adjustedRotation * transform.forward;

            // Wykonaj raycast
            Ray ray = new Ray(transform.position, direction);
            RaycastHit[] rayHits = Physics.RaycastAll(ray, raycastDistance);
            int punctureCount = 0;

            // Iteruj przez wszystkie trafione obiekty
            foreach (RaycastHit hit in rayHits)
            {
                // Sprawd� czy trafiony obiekt ma tag "Enemy" i nie otrzyma� jeszcze obra�e� w ramach tego ataku
                if (hit.collider.CompareTag("Enemy") && !enemiesHitThisAttack.Contains(hit.collider.gameObject))
                {
                    hits.Add(hit);
                    enemiesHitThisAttack.Add(hit.collider.gameObject);  // Dodaj obiekt do listy, aby unikn�� wielokrotnego zadawania obra�e�
                    Debug.Log("Wykryto" + hit.collider.gameObject.name);
                    punctureCount++;

                    // Zatrzymaj, je�li osi�gni�to maksymaln� liczb� trafionych obiekt�w
                    if (punctureCount >= puncture)
                    {
                        break;
                    }
                }
            }
        }
        enemiesHitThisAttack.Clear();  // Wyczy�� list� po ka�dym ataku
        return hits.ToArray();
    }
}