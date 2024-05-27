using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour
{
    public float raycastDistance = 5f;  // D³ugoœæ raycasta
    public int numberOfRays = 9;  // Iloœæ promieni w wachlarzu
    public float fanAngle = 90f;  // K¹t wachlarza w stopniach
    public int puncture = 2;  // Liczba obiektów, które raycast mo¿e przebiæ
    public int raycastDirection;

    private List<GameObject> enemiesHitThisAttack = new List<GameObject>();  // Lista obiektów, które ju¿ otrzyma³y obra¿enia

    public RaycastHit[] Detect()
    {
        Debug.Log("Odebrano");

        List<RaycastHit> hits = new List<RaycastHit>();

        // Oblicz k¹t pomiêdzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Iteruj przez ka¿dy promieñ w wachlarzu
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
                // SprawdŸ czy trafiony obiekt ma tag "Enemy" i nie otrzyma³ jeszcze obra¿eñ w ramach tego ataku
                if (hit.collider.CompareTag("Enemy") && !enemiesHitThisAttack.Contains(hit.collider.gameObject))
                {
                    hits.Add(hit);
                    enemiesHitThisAttack.Add(hit.collider.gameObject);  // Dodaj obiekt do listy, aby unikn¹æ wielokrotnego zadawania obra¿eñ
                    Debug.Log("Wykryto" + hit.collider.gameObject.name);
                    punctureCount++;

                    // Zatrzymaj, jeœli osi¹gniêto maksymaln¹ liczbê trafionych obiektów
                    if (punctureCount >= puncture)
                    {
                        break;
                    }
                }
            }
        }
        enemiesHitThisAttack.Clear();  // Wyczyœæ listê po ka¿dym ataku
        return hits.ToArray();
    }
}