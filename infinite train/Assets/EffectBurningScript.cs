using System.Collections;
using UnityEngine;

public class EffectBurningScript : MonoBehaviour
{
    public float damagePerTick = 1; // Damage per tick
    public float tickCooldown = 3f; // Time between ticks in seconds
    private UniversalHealth targetHealth;

    private void Start()
    {
        // Find the UniversalHealth component on this object
        targetHealth = GetComponent<UniversalHealth>();

        // Check if targetHealth was found
        if (targetHealth != null)
        {
            // Start applying the burning effect
            StartCoroutine(ApplyBurningEffect());
            // Start the coroutine to remove this script after 5 seconds
            StartCoroutine(RemoveScriptAfterTime(10f));
        }
        else
        {
            Debug.LogError("No UniversalHealth component found on object " + gameObject.name);
        }
    }

    private IEnumerator ApplyBurningEffect()
    {
        // Repeat the burning effect
        while (true)
        {
            // Check if targetHealth is not null
            if (targetHealth != null)
            {
                // Apply damage
                targetHealth.TakeDamage(damagePerTick, gameObject);
            }
            else
            {
                // Log an error and break the loop if targetHealth is null
                Debug.LogError("targetHealth is null during ApplyBurningEffect on " + gameObject.name);
                yield break; // Exit the coroutine
            }

            // Wait for cooldown
            yield return new WaitForSeconds(tickCooldown);
        }
    }

    private IEnumerator RemoveScriptAfterTime(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        // Remove this script from the game object
        Destroy(this);
    }
}