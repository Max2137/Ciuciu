using UnityEngine;

public class EnemyBehaviourAttack : MonoBehaviour
{
    public int attackDamage = 10;        // Damage dealt per attack
    public AudioClip attackSound;        // Sound played on attack
    public Animator mAnimator;           // Animator for attack animation
    private AudioSource audioSource;     // Audio source for playing attack sound
    private EnemyBehaviourRaycast enemyBehaviourRaycast; // Reference to the raycast script
    private GameObject player;           // Reference to the detected player

    void Awake()
    {
        // Attempt to find the EnemyBehaviourRaycast script on the same GameObject
        enemyBehaviourRaycast = GetComponent<EnemyBehaviourRaycast>();

        if (enemyBehaviourRaycast == null)
        {
            Debug.LogError("EnemyBehaviourRaycast script not found on the GameObject.");
        }

        this.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SetTargetPlayer();
        AttackPlayer();
        this.enabled = false; // Deactivate only this script
    }

    void SetTargetPlayer()
    {
        if (enemyBehaviourRaycast != null)
        {
            player = enemyBehaviourRaycast.DetectedTarget;
        }
        else
        {
            Debug.Log("nie ma skryptu raycastu");
        }
    }

    void AttackPlayer()
    {
        if (player == null)
        {
            Debug.Log("No player detected to attack.");
            return;
        }

        UniversalHealth playerHealth = player.GetComponent<UniversalHealth>();

        Debug.Log("Attacking player");
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage, gameObject, EDamageType.MELEE);

            if (mAnimator != null)
            {
                mAnimator.SetTrigger("atak");
            }

            if (attackSound != null && audioSource != null)
            {
                // Play attack sound
                audioSource.PlayOneShot(attackSound);
            }
        }
    }
}