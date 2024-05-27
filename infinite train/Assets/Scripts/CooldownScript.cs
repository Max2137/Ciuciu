using UnityEngine;

public class CooldownScript : MonoBehaviour
{
    public float cooldown = 5f;  // Cooldown in seconds
    private float nextSpawnTime = 0f;  // Time when the next spawn is allowed

    // Check if spawning is allowed
    public bool CanSpawn()
    {
        return Time.time >= nextSpawnTime;
    }

    // Reset the cooldown timer
    public void ResetCooldown()
    {
        nextSpawnTime = Time.time + cooldown;
    }
}