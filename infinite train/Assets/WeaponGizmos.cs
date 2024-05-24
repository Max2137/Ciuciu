using UnityEngine;

public class WeaponGizmos : MonoBehaviour
{
    // Usuñ metodê DrawRaycasts

    void OnDrawGizmos()
    {
        // SprawdŸ czy skrypt WeaponDetection jest do³¹czony do tego samego obiektu
        WeaponDetection weaponDetection = GetComponent<WeaponDetection>();
        if (weaponDetection == null)
        {
            Debug.LogWarning("WeaponDetection component not found on the same object as WeaponGizmos.");
            return;
        }

        // Pobierz dane z WeaponDetection
        int numberOfRays = weaponDetection.numberOfRays;
        float fanAngle = weaponDetection.fanAngle;
        float raycastDistance = weaponDetection.raycastDistance;

        // Oblicz k¹t pomiêdzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Rysuj ka¿dy promieñ w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.up);
            Vector3 direction = rotation * transform.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * raycastDistance);
        }
    }
}