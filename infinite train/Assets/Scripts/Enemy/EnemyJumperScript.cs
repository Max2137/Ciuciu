using UnityEngine;
using UnityEngine.AI;

public class EnemyJumper : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;
    public float dashForce = 10f;
    public float dashWaitingTime = 2f;
    public float dashLongevity = 5f;
    public float attackDamage;
    public float attackStandardDamage = 10f;
    public float attackStandardCooldown = 2f;
    public GameObject attackSource;
    public AudioClip attackClip;

    private NavMeshAgent navAgent;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isTouchingPlayer;
    [SerializeField] private bool isWaiting;
    private Vector3 dashStartPosition;
    private Vector3 dashEndPosition;
    private float dashStartTime;
    private UniversalHealth playerHealth;
    private float lastAttackTime;
    private bool wasRecentlyAttacked;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("Script requires a NavMeshAgent component. Add NavMeshAgent to the enemy.");
        }

        navAgent.stoppingDistance = stoppingDistance;
        navAgent.speed = moveSpeed;

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
    }

    void Update()
    {
        if (targetObject != null && navAgent != null)
        {
            navAgent.SetDestination(targetObject.position);

            if (navAgent.remainingDistance > stoppingDistance && !isDashing)
            {
                Dash();
            }
            else if (isDashing && navAgent.remainingDistance <= stoppingDistance)
            {
                isDashing = false;
                CheckAttack();
            }
        }

        if (isTouchingPlayer)
        {
            if (Time.time - lastAttackTime > attackStandardCooldown)
            {
                AttackStandard();
                lastAttackTime = Time.time;
            }
        }

        if (wasRecentlyAttacked)
        {
            Invoke("ResetAttackStatus", 1f);
        }
    }

    void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashStartPosition = transform.position;
            dashEndPosition = targetObject.position;
            dashStartTime = Time.time;

            isWaiting = true;
            Invoke("ApplyDashForce", dashWaitingTime);
        }
    }

    void ApplyDashForce()
    {
        isWaiting = false;
        Vector3 dashDirection = (dashEndPosition - dashStartPosition).normalized;
        navAgent.Move(dashDirection * dashForce * Time.deltaTime);
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
            Debug.LogWarning("Player object with tag 'Player' not found.");
        }
    }

    private void CheckAttack()
    {
        if (isTouchingPlayer)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (!wasRecentlyAttacked)
        {
            Debug.Log("Attacking player");
            playerHealth.TakeDamage(attackDamage, gameObject, EDamageType.MELEE);
            wasRecentlyAttacked = true;
        }
    }

    private void AttackStandard()
    {
        Debug.Log("Attacking player");
        playerHealth.TakeDamage(attackStandardDamage, gameObject, EDamageType.MELEE);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            if (isDashing && !isWaiting)
            {
                playerHealth.TakeDamage(attackDamage, gameObject, EDamageType.MELEE);
                isDashing = false;
                wasRecentlyAttacked = true;

                if (attackSource != null && attackClip != null)
                {
                    AudioSource audioSource = attackSource.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(attackClip);
                        Debug.Log("Played: " + attackClip + " from source " + attackSource);
                    }
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
            isDashing = false;
        }
    }

    void ResetAttackStatus()
    {
        wasRecentlyAttacked = false;
    }
}