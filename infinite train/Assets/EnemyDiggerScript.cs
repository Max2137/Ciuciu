using UnityEngine;

public class EnemyDiggerScript : MonoBehaviour
{
    public float waitingTime;
    public float preparingTime;
    public float stayingAboveTime;
    public float damageAmount = 10f;

    public GameObject markerPrefab;
    public float markerYPosition = 2.0f; // Ustawienia pozycji Y dla markera
    public Vector3 markerScale = new Vector3(1.0f, 1.0f, 1.0f); // Skala dla markera

    private Transform targetObject;
    private float currentWaitingTime;
    private float currentPreparingTime;
    private float currentStayingAboveTime;
    private Vector3 unDiggingPlace;

    private bool isWaiting = true;
    private bool isPreparing = false;
    private bool isStayingAbove = false;

    private GameObject markerInstance;

    public float randomnessLevel;

    void Start()
    {
        TurnOff();

        currentWaitingTime = waitingTime + Random.Range(0, randomnessLevel * waitingTime);
        currentPreparingTime = preparingTime + Random.Range(0, randomnessLevel * preparingTime);
        currentStayingAboveTime = stayingAboveTime + Random.Range(0, randomnessLevel * stayingAboveTime);
    }

    void Update()
    {
        if (isWaiting)
        {
            HandleWaiting();
        }
        else if (isPreparing)
        {
            HandlePreparing();
        }
        else if (isStayingAbove)
        {
            HandleStayingAbove();
        }
    }

    void HandleWaiting()
    {
        currentWaitingTime -= Time.deltaTime;

        if (currentWaitingTime <= 0)
        {
            isWaiting = false;
            currentWaitingTime = waitingTime + Random.Range(0, randomnessLevel * waitingTime);
            isPreparing = true;
            // Po oczekiwaniu, wykonaj funkcjê FindPlayer
            FindPlayer();
            if (markerPrefab != null)
            {
                Vector3 markerPosition = new Vector3(unDiggingPlace.x, markerYPosition, unDiggingPlace.z);
                markerInstance = Instantiate(markerPrefab, markerPosition, Quaternion.identity);
                markerInstance.transform.localScale = markerScale;
            }
        }
    }

    void HandlePreparing()
    {
        currentPreparingTime -= Time.deltaTime;

        if (currentPreparingTime <= 0)
        {
            // Usuniêcie markera
            if (markerInstance != null)
            {
                Destroy(markerInstance);
            }
            isPreparing = false;
            currentPreparingTime = preparingTime + Random.Range(0, randomnessLevel * preparingTime);
            isStayingAbove = true;
            // Po przygotowaniu, przenieœ na wspó³rzêdne unDiggingPlace (z zachowaniem y na 0) i w³¹cz komponenty
            Vector3 newPosition = new Vector3(unDiggingPlace.x, 0f, unDiggingPlace.z);
            transform.position = newPosition;
            TurnOn();
        }
    }

    void HandleStayingAbove()
    {
        currentStayingAboveTime -= Time.deltaTime;

        if (currentStayingAboveTime <= 0)
        {
            isStayingAbove = false;
            currentStayingAboveTime = stayingAboveTime + Random.Range(0, randomnessLevel * stayingAboveTime);
            TurnOff();
            isWaiting = true;
            // Usuniêcie markera
            if (markerInstance != null)
            {
                Destroy(markerInstance);
            }
        }
    }

    void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            targetObject = playerObject.transform;
            FindUnoccupiedDiggingPlace();
        }
        else
        {
            Debug.LogWarning("Player object with tag 'Player' not found. Make sure the player object is tagged correctly.");
        }
    }

    void FindUnoccupiedDiggingPlace()
    {
        float maxAttempts = 5; // Maksymalna liczba prób znalezienia wolnego miejsca
        float minDistance = 2.0f; // Minimalna odleg³oœæ od innych obiektów
        bool foundUnoccupiedPlace = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomDistance = minDistance + Random.Range(0.5f, 1); // Losowy dystans w zakresie powy¿ej minimalnej odleg³oœci
            float randomAngle = Random.Range(0.0f, 360.0f); // Losowy k¹t

            Vector3 offsetDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0.0f, Mathf.Sin(randomAngle * Mathf.Deg2Rad)); // Kierunek offsetu
            Vector3 potentialDiggingPlace = unDiggingPlace + offsetDirection * randomDistance;

            if (!IsTooCloseToOtherObjects(potentialDiggingPlace))
            {
                unDiggingPlace = potentialDiggingPlace;
                foundUnoccupiedPlace = true;
                break;
            }
        }

        if (!foundUnoccupiedPlace)
        {
            Debug.LogWarning("Unable to find unoccupied digging place after multiple attempts. Using the original position.");
        }
    }

    bool IsTooCloseToOtherObjects(Vector3 position)
    {
        float minDistance = 1.5f; // Minimalna odleg³oœæ, aby uznaæ miejsce za wolne
        Collider[] colliders = Physics.OverlapSphere(position, minDistance); // Sprawdzanie kolizji wokó³ pozycji

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject) // SprawdŸ, czy to nie jest ta sama instancja obiektu
            {
                return true; // Miejsce jest za blisko innego obiektu
            }
        }

        return false; // Miejsce jest wolne
    }

    private void TurnOff()
    {
        Component[] components = GetComponents<Component>();

        foreach (Component component in components)
        {
            // Wy³¹cz wszystkie komponenty poza Transform i tym skryptem
            if (component != transform && component.GetType() != typeof(EnemyDiggerScript))
            {
                if (component is Behaviour)
                {
                    // Wy³¹cz pozosta³e komponenty
                    ((Behaviour)component).enabled = false;
                }
                else if (component is Renderer)
                {
                    // Jeœli komponent to Renderer, wy³¹cz równie¿ rendering
                    ((Renderer)component).enabled = false;
                }
                else if (component is Collider)
                {
                    // Jeœli komponent to Collider, wy³¹cz kolizje
                    ((Collider)component).enabled = false;
                }
                else if (component is Collider2D)
                {
                    // Jeœli komponent to Collider2D, wy³¹cz kolizje
                    ((Collider2D)component).enabled = false;
                }
            }
        }
    }

    private void TurnOn()
    {
        Component[] components = GetComponents<Component>();

        foreach (Component component in components)
        {
            // W³¹cz wszystkie wy³¹czone komponenty poza Transform i tym skryptem
            if (component != transform && component.GetType() != typeof(EnemyDiggerScript))
            {
                if (component is Behaviour)
                {
                    // W³¹cz wy³¹czone komponenty Behaviour
                    ((Behaviour)component).enabled = true;
                }
                else if (component is Renderer)
                {
                    // Jeœli komponent to Renderer, w³¹cz rendering
                    ((Renderer)component).enabled = true;
                }
                else if (component is Collider)
                {
                    // Jeœli komponent to Collider, w³¹cz kolizje
                    ((Collider)component).enabled = true;
                }
                else if (component is Collider2D)
                {
                    // Jeœli komponent to Collider2D, w³¹cz kolizje
                    ((Collider2D)component).enabled = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // SprawdŸ, czy collider nie jest ustawiony jako trigger
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            Debug.Log("Dealing damage to player");

            if (targetObject != null)
            {
                UniversalHealth playerHealth = targetObject.GetComponent<UniversalHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount, gameObject);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Rysuj sferê wokó³ pozycji gracza (unDiggingPlace)
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(unDiggingPlace, 0.2f);
    }
}