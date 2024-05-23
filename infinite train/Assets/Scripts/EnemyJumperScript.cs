using UnityEngine;

public class EnemyJumper : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;
    public float dashForce = 10f;
    public float dashWaitingTime = 2f;
    public float dashLongevity = 5f; // New variable for dash distance or time
    public float attackDamage;
    public float attackStandardDamage = 10f; // Dodane: standardowe obra¿enia
    public float attackStandardCooldown = 2f; // Dodane: standardowy czas odnowienia
    public GameObject attackSource; // Obiekt AudioSource
    public AudioClip attackClip; // DŸwiêk ataku

    private Rigidbody enemyRigidbody;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isTouchingPlayer;
    [SerializeField] private bool isWaiting;
    private Vector3 dashStartPosition; // Variable to store the dash start position
    private Vector3 dashEndPosition;
    private float dashStartTime; // New variable to store the dash start time
    private UniversalHealth playerHealth;
    private float lastAttackTime;
    private bool wasRecentlyAttacked;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        if (enemyRigidbody == null)
        {
            Debug.LogError("Script requires a Rigidbody component. Add Rigidbody to the enemy.");
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
    }

    void Update()
    {
        if (!isWaiting && isDashing && enemyRigidbody.velocity.magnitude < 0.1f)
        {
            isDashing = false;
            //enemyRigidbody.velocity = Vector3.zero;
            //CheckAttack();
            //Debug.Log("Zresetowano");
        }

        if (isTouchingPlayer)
        {
            if (Time.time - lastAttackTime > attackStandardCooldown)
            {
                AttackStandard();
                lastAttackTime = Time.time; // Zaktualizuj czas ostatniego ataku
            }
        }

        // Check if the enemy is not moving despite not waiting
        if (!isWaiting && enemyRigidbody.velocity.magnitude < 0.1f && isDashing)
        {
            //isDashing = false;
        }

        if (targetObject != null && enemyRigidbody != null)
        {
            Vector3 direction = targetObject.position - transform.position;
            float distanceToTarget = direction.magnitude;
            direction.Normalize();

            if (distanceToTarget > stoppingDistance && !isDashing)
            {
                enemyRigidbody.velocity = direction * moveSpeed;
            }
            else
            {
                Dash();
            }
        }
        else
        {
            Debug.LogWarning("No target object or Rigidbody component. Assign the target object and add Rigidbody in the inspector.");
        }

        if (isDashing == true)
        {
            float distanceCovered = Vector3.Distance(transform.position, dashStartPosition);

            // Check if the dash has covered the desired distance
            if (distanceCovered >= dashLongevity)
            {
                isDashing = false;
                enemyRigidbody.velocity = Vector3.zero;
                CheckAttack();
                Debug.Log("Dash ended after covering the required distance");
            }
        }

        // Resetowanie wasRecentlyAttacked po pewnym czasie
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
            dashStartPosition = transform.position; // Start from the current position
            dashEndPosition = targetObject.position;
            dashStartTime = Time.time; // Record the dash start time

            // Invoke ApplyDashForce after waiting for DashWaitingTime
            isWaiting = true;
            Invoke("ApplyDashForce", dashWaitingTime);
        }
    }

    void ApplyDashForce()
    {
        isWaiting = false;

        // Ustal kierunek dashu
        Vector3 dashDirection = (dashEndPosition - dashStartPosition).normalized;

        // Dodaj si³ê do przeciwnika
        enemyRigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
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
            Debug.LogWarning("Player object with tag 'Player' not found. Make sure the player object is tagged correctly.");
        }
    }

    private void CheckAttack()
    {
        if (isTouchingPlayer == true)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (wasRecentlyAttacked == false)
        {
            Debug.Log("Attacking player");
            playerHealth.TakeDamage(attackDamage, gameObject, EDamageType.OTHER);
        }
        wasRecentlyAttacked = true;
    }

    private void AttackStandard()
    {
        Debug.Log("Attacking player");
        playerHealth.TakeDamage(attackStandardDamage, gameObject, EDamageType.OTHER);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;

            if (isDashing && !isWaiting)
            {
                //playerHealth.TakeDamage(attackDamage, gameObject);
                //isDashing = false;
            }
            playerHealth.TakeDamage(attackDamage, gameObject, EDamageType.OTHER);
            isDashing = false;

            // Ustawienie wasRecentlyAttacked na true
            wasRecentlyAttacked = true;

            // Odtwórz dŸwiêk ataku
            if (attackSource != null && attackClip != null)
            {
                AudioSource audioSource = attackSource.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(attackClip);
                    Debug.Log("Zagrano: " + attackClip + " Ÿród³em " + attackSource);
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

    // Metoda do resetowania wasRecentlyAttacked po pewnym czasie
    void ResetAttackStatus()
    {
        wasRecentlyAttacked = false;
    }

    // Metoda do rysowania gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(dashEndPosition, 0.2f);
    }
}