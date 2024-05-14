using UnityEngine;

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

    private Rigidbody enemyRigidbody;
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

        enemyRigidbody = GetComponent<Rigidbody>();

        if (enemyRigidbody == null)
        {
            Debug.LogError("Skrypt wymaga komponentu Rigidbody. Dodaj Rigidbody do wroga.");
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
        if (targetObject != null && enemyRigidbody != null)
        {
            // Obliczanie kierunku, w którym wrogi obiekt powinien pod¹¿aæ
            Vector3 direction = targetObject.position - transform.position;
            float distanceToTarget = direction.magnitude;
            direction.Normalize();

            Quaternion toRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

            // Ustaw obrót tylko w osi Y
            toRotation = Quaternion.Euler(0f, toRotation.eulerAngles.y, 0f);

            enemyRigidbody.MoveRotation(Quaternion.RotateTowards(enemyRigidbody.rotation, toRotation, Time.deltaTime * 1000f));

            // Przesuñ wrogi obiekt w kierunku celu, ale zatrzymaj siê na okreœlonej odleg³oœci
            if (distanceToTarget > stoppingDistance)
            {
                enemyRigidbody.velocity = direction * moveSpeed;
                currentCooldown = attackCooldown;
            }
            else if (distanceToTarget < escapingDistance)
            {
                // Przeciwnik jest za blisko lub w EscapeDistance, udaæ siê na dystans uœredniony
                float avgDistance = (stoppingDistance + escapingDistance) / 2f;
                Vector3 avgPosition = targetObject.position + direction.normalized * avgDistance;
                Vector3 avgDirection = avgPosition - transform.position;

                enemyRigidbody.velocity = avgDirection.normalized * moveSpeed;
                currentCooldown = attackCooldown;
            }
            else
            {
                enemyRigidbody.velocity = Vector3.zero;
            }

            // SprawdŸ czy przeciwnik mo¿e zaatakowaæ
            if (currentCooldown <= 0 && distanceToTarget <= stoppingDistance)
            {
                AttackPlayer();
                currentCooldown = attackCooldown;
            }
        }
        else
        {
            Debug.LogWarning("Brak obiektu celu lub komponentu Rigidbody. Przypisz obiekt celu i dodaj Rigidbody w inspektorze.");
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
            Debug.LogWarning("Nie znaleziono obiektu gracza z tagiem 'Player'. Upewnij siê, ¿e obiekt gracz zosta³ oznaczony poprawnym tagiem.");
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
        Debug.Log("Attacking player");

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
        // SprawdŸ, czy wrogi obiekt koliduje z graczem
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zatrzymaj wroga, mo¿na dodaæ dodatkowe dzia³ania, np. odejmowanie zdrowia gracza itp.
            enemyRigidbody.velocity = Vector3.zero;

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
        Debug.Log("Attacking player");
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamagePhysical, gameObject);
        }
    }
}