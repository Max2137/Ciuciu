using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowerScript : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;

    private NavMeshAgent navMeshAgent;

    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    private float currentCooldown = 0f;
    private bool isTouchingPlayer;

    private UniversalHealth playerHealth;
    public AudioClip attackSound;
    private AudioSource audioSource;

    private Animator mAnimator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("Skrypt wymaga komponentu NavMeshAgent. Dodaj NavMeshAgent do wroga.");
        }
        else
        {
            navMeshAgent.speed = moveSpeed;
        }

        if (targetObject == null)
        {
            FindPlayer();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<UniversalHealth>();
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        mAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (targetObject != null && navMeshAgent != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);

            if (distanceToTarget > stoppingDistance)
            {
                navMeshAgent.SetDestination(targetObject.position);
            }
            else
            {
                navMeshAgent.ResetPath();
            }
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (isTouchingPlayer && currentCooldown <= 0)
        {
            AttackPlayer();
            currentCooldown = attackCooldown;
        }
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            targetObject = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu gracza z tagiem 'Player'.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            navMeshAgent.ResetPath();
            isTouchingPlayer = true;
        }

        currentCooldown = attackCooldown * 0.25f;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    void AttackPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage, gameObject, EDamageType.MELEE);
            mAnimator.SetTrigger("atak");

            if (attackSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(attackSound);
            }
        }
    }
}