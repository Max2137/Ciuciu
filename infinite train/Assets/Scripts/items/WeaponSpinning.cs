using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpinning : MonoBehaviour
{
    public float spinSpeed = 100f;
    public Vector3 spinAxis = Vector3.forward; // Ustawienia osi obracania

    private bool isSpinning = false;

    void OnEnable()
    {
        StartSpinning();
        // Zresetuj wszystkie si³y wp³ywaj¹ce na obiekt na zero
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    void OnDisable()
    {
        StopSpinning();
    }

    void StartSpinning()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinCoroutine());
        }
    }

    void StopSpinning()
    {
        isSpinning = false;
    }

    IEnumerator SpinCoroutine()
    {
        isSpinning = true;

        // ZnajdŸ skrypt PlayerRotation na najwy¿szym rodzicu
        PlayerRotation playerRotationScript = transform.root.GetComponent<PlayerRotation>();
        if (playerRotationScript != null)
        {
            // Dezaktywuj skrypt PlayerRotation
            playerRotationScript.enabled = false;
        }
        else
        {
            Debug.LogError("PlayerRotation script not found on the root object.");
        }

        while (isSpinning)
        {
            transform.root.Rotate(spinAxis, spinSpeed * Time.deltaTime);
            yield return null;
        }

        // Aktywuj skrypt PlayerRotation po zakoñczeniu obracania
        if (playerRotationScript != null)
        {
            playerRotationScript.enabled = true;
        }
    }
}