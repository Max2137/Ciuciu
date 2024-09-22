using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0f; // How long the shake will last
    public float shakeMagnitude = 0.1f; // How intense the shake is
    public float dampingSpeed = 1.0f; // How quickly the shake fades
    private Vector3 initialPosition; // The camera's starting position

    private void OnEnable()
    {
        initialPosition = transform.position; // Store the camera's initial position
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.position = initialPosition; // Reset the position
        }
    }

    // Call this method to start the shake effect
    public void Shake(float duration, float magnitude)
    {
                initialPosition = transform.position; // Store the camera's initial position
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}