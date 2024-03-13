using UnityEngine;

public class AdditionalFollowingPlayerFromAbove : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;

    private Rigidbody enemyRigidbody;

    public float attackCooldown = 2f;  // Czas oczekiwania mi�dzy atakami
    public float attackDamage = 10f;  // Obra�enia zadawane podczas ataku

    private UniversalHealth playerHealth;
    [SerializeField] private float currentCooldown = 0f;
    private bool isTouchingPlayer;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        if (enemyRigidbody == null)
        {
            Debug.LogError("Skrypt wymaga komponentu Rigidbody. Dodaj Rigidbody do wroga.");
        }

        // Je�eli targetObject nie jest przypisane w inspectorze, znajd� najbli�szy obiekt z tagiem "Player"
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
        if (targetObject != null && enemyRigidbody != null)
        {
            // Obliczanie kierunku, w kt�rym wrogi obiekt powinien pod��a�
            Vector3 direction = targetObject.position - transform.position;
            float distanceToTarget = direction.magnitude;
            direction.Normalize();

            // Przesu� wrogi obiekt w kierunku celu, ale zatrzymaj si� na okre�lonej odleg�o�ci
            if (distanceToTarget > stoppingDistance)
            {
                enemyRigidbody.velocity = direction * moveSpeed;

                // Obr�� obiekt w kierunku celu
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
        // Znajd� najbli�szy obiekt z tagiem "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            targetObject = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Nie znaleziono obiektu gracza z tagiem 'Player'. Upewnij si�, �e obiekt gracz zosta� oznaczony poprawnym tagiem.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Sprawd�, czy wrogi obiekt koliduje z graczem
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zatrzymaj wroga, mo�na doda� dodatkowe dzia�ania, np. odejmowanie zdrowia gracza itp.
            enemyRigidbody.velocity = Vector3.zero;
        }

        currentCooldown = attackCooldown * 0.25f;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Po opuszczeniu kolizji wznow ruch wroga
        if (collision.gameObject.CompareTag("Player"))
        {
            // Mo�esz dostosowa� pr�dko��, aby wrogi obiekt zn�w ruszy� si� po opuszczeniu kolizji.
        }

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
        }
    }
}