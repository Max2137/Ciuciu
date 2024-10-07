using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOfAirScript : MonoBehaviour
{
    [Header("Pushing Parameters")]
    public float pushForce = 10f;        // Force with which enemies will be pushed
    public float pushRadius = 5f;        // Maximum distance of the rays
    public int rayCount = 10;            // Number of rays to shoot in a fan-like spread
    public float rayAngle = 45f;         // Angle range of the fan in degrees
    public float pushDuration = 0.5f;    // Duration over which the push force is applied

    private CooldownScript cooldownScript;
    private ParentCheckScript parentCheckScript;
    private MousePositionScript mousePositionScript;
    private WeaponInputManager inputManager;

    private Vector3[] rayDirections;

    void Start()
    {
        cooldownScript = GetComponent<CooldownScript>();
        parentCheckScript = GetComponent<ParentCheckScript>();
        mousePositionScript = GetComponent<MousePositionScript>();
        inputManager = GetComponentInParent<WeaponInputManager>();

        if (pushForce <= 0)
        {
            Debug.LogError("pushForce must be greater than 0!");
        }

        rayDirections = new Vector3[rayCount];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && cooldownScript.CanSpawn() && parentCheckScript.IsChildOfFirstSlot())
        {
            Vector3 mousePosition = mousePositionScript.GetMouseWorldPosition();
            PushEnemies(mousePosition);
            cooldownScript.ResetCooldown();
        }
    }

    // Method to push enemies using raycasts
    void PushEnemies(Vector3 center)
    {
        // Ensure the y position is fixed to 0
        center.y = 0;

        // Calculate the direction to the mouse position
        Vector3 directionToMouse = (center - transform.position).normalized;
        directionToMouse.y = 0;

        // Calculate the initial direction for the rays
        Vector3 initialDirection = Quaternion.Euler(0, -rayAngle / 2, 0) * directionToMouse;

        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the rotation angle for each ray
            float angle = i * (rayAngle / (rayCount - 1));
            // Calculate the direction for each ray
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * initialDirection;

            // Store the ray direction for Gizmos drawing
            rayDirections[i] = rayDirection;

            // Shoot the raycast
            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit[] hits = Physics.RaycastAll(ray, pushRadius);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))  // Assuming enemies have the tag "Enemy"
                {
                    Rigidbody enemyRb = hit.collider.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        // Start coroutine to apply force gradually
                        StartCoroutine(ApplyPushForce(enemyRb, (hit.point - transform.position).normalized));
                    }
                }
                else if (hit.collider.CompareTag("AirMovable"))  // Check for "AirMovable" tag
                {
                    Destroy(hit.collider.gameObject);  // Destroy the object
                }
            }
        }
    }

    // Coroutine to apply the push force gradually
    IEnumerator ApplyPushForce(Rigidbody enemyRb, Vector3 pushDirection)
    {
        float timeElapsed = 0f;
        while (timeElapsed < pushDuration)
        {
            enemyRb.AddForce(pushDirection * (pushForce * Time.deltaTime / pushDuration), ForceMode.Impulse);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Draw Gizmos to visualize the ray directions
    void OnDrawGizmos()
    {
        if (rayDirections != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < rayDirections.Length; i++)
            {
                Gizmos.DrawRay(transform.position, rayDirections[i] * pushRadius);
            }
        }
    }
}