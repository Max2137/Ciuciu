using UnityEngine;

public class EnemyHealerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;
    public float healingCooldown = 3f;
    public float healingAmount = 20f;
    public float switchTargetCooldown = 5f;

    private Rigidbody healerRigidbody;
    [SerializeField] private UniversalHealth targetHealth;
    [SerializeField] private GameObject[] potentialTargets;
    private bool isHealing;
    private float currentHealingCooldown;
    private float currentSwitchTargetCooldown;
    private GameObject healingEffect; // Usuni�cie publicznego pola, b�dzie automatycznie ustawiane na pierwszego potomka

    void Start()
    {
        healerRigidbody = GetComponent<Rigidbody>();
        if (healerRigidbody == null)
        {
            Debug.LogError("Skrypt wymaga komponentu Rigidbody. Dodaj Rigidbody do lecznika.");
        }

        // Znajd� wszystkie obiekty z tagiem "Enemy" (lub innym odpowiednim)
        potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");

        // Inicjalizuj pierwszy cel
        FindNextTarget();

        // Znajd� pierwszego potomka (childa) obiektu i ustaw go jako obiekt HealingEffect
        if (transform.childCount > 0)
        {
            healingEffect = transform.GetChild(0).gameObject;
        }
        else
        {
            Debug.LogError("Nie znaleziono �adnych potomk�w (child�w) tego obiektu.");
        }
        healingEffect.SetActive(false);
    }

    void Update()
    {
        FindNextTarget();

        potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");

        if (targetHealth != null && targetHealth.gameObject.activeSelf && healerRigidbody != null)
        {
            // Obliczanie kierunku, w kt�rym lecznik powinien pod��a�
            Vector3 direction = targetHealth.transform.position - transform.position;
            float distanceToTarget = direction.magnitude;
            direction.Normalize();

            // Przesu� lecznika w kierunku celu, ale zatrzymaj si� na okre�lonej odleg�o�ci
            if (distanceToTarget > stoppingDistance)
            {
                healerRigidbody.velocity = direction * moveSpeed;
            }
            else
            {
                healerRigidbody.velocity = Vector3.zero;
            }

            // Sprawd�, czy lecznik mo�e leczy�
            if (!isHealing && currentHealingCooldown <= 0)
            {
                CheckAndHealTarget();
                currentHealingCooldown = healingCooldown;
            }

            // Sprawd�, czy lecznik mo�e prze��czy� cel
            if (currentSwitchTargetCooldown <= 0)
            {
                FindNextTarget();
                currentSwitchTargetCooldown = switchTargetCooldown;
            }

            // Aktualizuj liczniki czasu
            if (currentHealingCooldown > 0)
            {
                currentHealingCooldown -= Time.deltaTime;
            }

            if (currentSwitchTargetCooldown > 0)
            {
                currentSwitchTargetCooldown -= Time.deltaTime;
            }
        }

        if (targetHealth != null)
        {
            FindNextTarget(); // Je�eli obecny cel przestaje istnie�, znajd� nowy cel
            //Debug.LogWarning("Brak obiektu celu lub komponentu Rigidbody");
        }
        else
        {
            healingEffect.SetActive(false);
        }

        // Ustawianie aktywno�ci obiektu HealingEffect
        if (healingEffect != null)
        {
            //healingEffect.SetActive(isHealing);
            //Debug.Log(isHealing);
        }
    }

    void CheckAndHealTarget()
    {
        if (targetHealth != null && targetHealth.currentHealth < targetHealth.maxHealth * 0.9f)
        {
            // Lecz obecny cel
            isHealing = true;
            Debug.Log("Healing target");
            targetHealth.Heal(healingAmount);
            healingEffect.SetActive(true);

            // Je�eli obecny cel ma teraz wi�cej ni� 90% zdrowia, znajd� nowy cel
            if (targetHealth.currentHealth >= targetHealth.maxHealth * 0.9f)
            {
                healingEffect.SetActive(false);
                FindNextTarget();
            }

            isHealing = false;
        }
    }

    void FindNextTarget()
    {
        // Sortuj potencjalne cele wed�ug odleg�o�ci
        System.Array.Sort(potentialTargets, CompareTargets);

        foreach (GameObject potentialTarget in potentialTargets)
        {
            // Dodaj sprawdzenie, czy obiekt nie zosta� zniszczony
            if (potentialTarget == null)
            {
                continue;
            }

            UniversalHealth health = potentialTarget.GetComponent<UniversalHealth>();

            // Dodaj sprawdzenie, czy komponent UniversalHealth nie jest null
            if (health != null && health.currentHealth < health.maxHealth * 0.9f && health.gameObject.activeSelf)
            {
                targetHealth = health;
                return;
            }
        }

        // Je�eli nie znaleziono celu, ustaw obecny cel na null
        targetHealth = null;
    }

    int CompareTargets(GameObject target1, GameObject target2)
    {
        // Dodaj sprawdzenie, czy obiekty nie zosta�y zniszczone
        if (target1 == null || target2 == null)
        {
            return 0;
        }

        float distance1 = Vector3.Distance(transform.position, target1.transform.position);
        float distance2 = Vector3.Distance(transform.position, target2.transform.position);

        return distance1.CompareTo(distance2);
    }
}