using UnityEngine;

public class EnemyFollowerScript : MonoBehaviour
{
    public Transform targetObject;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;

    private Rigidbody enemyRigidbody;

    public float attackCooldown = 2f;  // Czas oczekiwania miêdzy atakami
    public float attackDamage = 10f;  // Obra¿enia zadawane podczas ataku

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

        // Je¿eli targetObject nie jest przypisane w inspectorze, znajdŸ najbli¿szy obiekt z tagiem "Player"
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
        // ZnajdŸ najbli¿szy obiekt z tagiem "Player"
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
        // SprawdŸ, czy wrogi obiekt koliduje z graczem
        if (collision.gameObject.CompareTag("Player"))
        {
            // Zatrzymaj wroga, mo¿na dodaæ dodatkowe dzia³ania, np. odejmowanie zdrowia gracza itp.
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
        }
    }
}