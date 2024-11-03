using UnityEngine;
using UnityEngine.AI;

public class EnemyShooterScript : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 5f;
    public float escapingDistance;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    public GameObject projectilePrefab;
    private Transform firePoint;

    private NavMeshAgent navMeshAgent;
    private UniversalHealth playerHealth;
    private float currentCooldown = 0f;

    private bool isTouchingPlayer;
    private float currentCooldownPhysical;
    public float attackCooldownPhysical;
    public float attackDamagePhysical;

    private AudioSource audioSource;
    public AudioClip shootingSound;

    private Animator mAnimator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        isTouchingPlayer = false;

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

        if (firePoint == null)
        {
            CreateFirePoint();
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
                currentCooldown = attackCooldown;
            }
            else if (distanceToTarget < escapingDistance)
            {
                // Przeciwnik zbyt blisko, wycofanie
                Vector3 escapeDirection = (transform.position - targetObject.position).normalized;
                Vector3 escapePosition = targetObject.position + escapeDirection * ((stoppingDistance + escapingDistance) / 2f);
                navMeshAgent.SetDestination(escapePosition);
                currentCooldown = attackCooldown;
            }
            else
            {
                navMeshAgent.ResetPath();
            }

            if (currentCooldown <= 0 && distanceToTarget <= stoppingDistance)
            {
                AttackPlayer();
                currentCooldown = attackCooldown;
            }
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (currentCooldownPhysical > 0)
        {
            currentCooldownPhysical -= Time.deltaTime;
        }

        if (isTouchingPlayer && currentCooldownPhysical <= 0)
        {
            AttackPlayerPhysical();
            currentCooldownPhysical = attackCooldownPhysical;
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

    void CreateFirePoint()
    {
        firePoint = new GameObject("FirePoint").transform;
        firePoint.SetParent(transform);
        firePoint.localPosition = new Vector3(0f, 1f, 1f);
    }

    void AttackPlayer()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            audioSource.PlayOneShot(shootingSound);
            mAnimator.SetTrigger("atak");

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<ProjectileStandardScript>().SetOwner(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            navMeshAgent.ResetPath();
            isTouchingPlayer = true;
        }

        currentCooldownPhysical = attackCooldownPhysical;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    void AttackPlayerPhysical()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamagePhysical, gameObject, EDamageType.OTHER);
        }
    }
}