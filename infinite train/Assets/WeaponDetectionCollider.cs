using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetectionCollider : MonoBehaviour
{
    public WeaponHoldManager holdManager; // Assuming you've renamed WeaponHoldToSpinManager to WeaponHoldManager

    private void Start()
    {
        if (holdManager == null)
        {
            Debug.LogError("WeaponHoldManager not assigned in the inspector.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            UniversalHealth enemyHealth = other.gameObject.GetComponent<UniversalHealth>();
            if (enemyHealth != null)
            {
                // Report hit to the WeaponHoldManager
                holdManager.ReportHit(other.gameObject);
            }
        }
    }
}