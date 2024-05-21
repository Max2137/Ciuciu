using System.Collections;
using UnityEngine;

public class EffectBurningScript : MonoBehaviour
{
    private float damagePerTick; // Damage per tick
    private float tickCooldown; // Time between ticks in seconds
    private UniversalHealth targetHealth;
    private Coroutine burningCoroutine;

    private void Awake()
    {
        // Find the UniversalHealth component on this object
        targetHealth = GetComponent<UniversalHealth>();

        // Check if targetHealth was found
        if (targetHealth == null)
        {
            Debug.LogError("No UniversalHealth component found on object " + gameObject.name);
        }
    }

    public void StartBurningEffect(float damagePerTick, float tickCooldown)
    {
        if (targetHealth == null)
        {
            Debug.LogError("No UniversalHealth component found on object " + gameObject.name);
            return;
        }

        this.damagePerTick = damagePerTick;
        this.tickCooldown = tickCooldown;

        // If a burning effect is already active, stop it before starting a new one
        if (burningCoroutine != null)
        {
            StopCoroutine(burningCoroutine);
        }

        // Start applying the burning effect
        burningCoroutine = StartCoroutine(ApplyBurningEffect());
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
}