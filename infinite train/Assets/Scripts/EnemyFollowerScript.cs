using UnityEngine;

public class EnemyFollowerScript : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;

    private Rigidbody enemyRigidbody;

    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    private float currentCooldown = 0f;
    private bool isTouchingPlayer;

    private UniversalHealth playerHealth;
    public AudioClip attackSound; // DŸwiêk ataku, ustaw w inspectorze
    private AudioSource audioSource;

    private Animator mAnimator;

   


    void Start()
    {
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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Dodaj komponent AudioSource, jeœli nie zosta³ jeszcze dodany
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        mAnimator = GetComponentInChildren<Animator>();
    }
    

    void Update()
    {
        if (targetObject != null && enemyRigidbody != null)
        {
            Vector3 direction = targetObject.position - transform.position;
            float distanceToTarget = direction.magnitude;
            direction.Normalize();

            Quaternion toRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            toRotation = Quaternion.Euler(0f, toRotation.eulerAngles.y, 0f);
            enemyRigidbody.MoveRotation(Quaternion.RotateTowards(enemyRigidbody.rotation, toRotation, Time.deltaTime * 1000f));

            

            if (distanceToTarget > stoppingDistance)
            {
                enemyRigidbody.velocity = direction * moveSpeed;
            }
            else
            {
                enemyRigidbody.velocity = Vector3.zero;
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
            Debug.LogWarning("Nie znaleziono obiektu gracza z tagiem 'Player'. Upewnij siê, ¿e obiekt gracz zosta³ oznaczony poprawnym tagiem.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyRigidbody.velocity = Vector3.zero;
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
        Debug.Log("Attacking player");
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage, gameObject);

            mAnimator.SetTrigger("atak");

            if (attackSound != null && audioSource != null)
            {
                // Odtwórz dŸwiêk ataku
                audioSource.PlayOneShot(attackSound);
            }
        }
    }
}