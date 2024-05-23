using System.Collections;
using UnityEngine;

public class EffectBurningScript : MonoBehaviour
{
    public string particleName; // Nazwa obiektu Particle do znalezienia w scenie
    public float damagePerTick = 1; // Damage per tick
    public float tickCooldown = 3f; // Time between ticks in seconds
    private UniversalHealth targetHealth;
    private GameObject burningParticle; // Renamed variable

    private void Start()
    {
        particleName = "BurningPS";

        // Find the UniversalHealth component on this object
        targetHealth = GetComponent<UniversalHealth>();

        // Check if targetHealth was found
        if (targetHealth != null)
        {
            // Start applying the burning effect
            StartCoroutine(ApplyBurningEffect());
            // Start the coroutine to remove this script after 10 seconds
            StartCoroutine(RemoveScriptAfterTime(10f));

            // Spawn the prefab as a child
            if (!string.IsNullOrEmpty(particleName))
            {
                // Find the particle object with the given name in the scene
                GameObject particleObject = GameObject.Find(particleName);
                if (particleObject != null)
                {
                    // Spawn the particle object as a child of this object
                    burningParticle = Instantiate(particleObject, transform.position, Quaternion.identity);
                    burningParticle.transform.parent = transform;
                }
                else
                {
                    Debug.LogError("Particle object with name " + particleName + " not found in the scene.");
                }
            }
            else
            {
                Debug.LogError("Particle name is not set for " + gameObject.name);
            }
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
            // Wait for cooldown
            yield return new WaitForSeconds(tickCooldown);

            // Check if targetHealth is not null
            if (targetHealth != null)
            {
                // Apply damage
                targetHealth.TakeDamage(damagePerTick, gameObject, EDamageType.OTHER);
                Debug.Log("spalono " + damagePerTick);
            }
            else
            {
                // Log an error and break the loop if targetHealth is null
                Debug.LogError("targetHealth is null during ApplyBurningEffect on " + gameObject.name);
                yield break; // Exit the coroutine
            }
        }
    }

    private IEnumerator RemoveScriptAfterTime(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the burningParticle if it exists
        if (burningParticle != null)
        {
            Destroy(burningParticle);
        }

        // Remove this script from the game object
        Destroy(this);
    }
}