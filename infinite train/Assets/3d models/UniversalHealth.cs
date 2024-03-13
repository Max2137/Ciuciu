using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalHealth : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public float maxHealth;
    public float currentHealth;
    public float blinkDuration = 0.2f; // Duration of the blink effect
    public Color blinkColor = Color.red; // Color to blink on damage
    public Color healBlinkColor = Color.green; // Color to blink while healing
    public Color deathColor = Color.black; // Color when the object is about to die
    public float deathDuration = 0.5f; // Duration of the death effect
    public int experienceWorth;

    private HashSet<GameObject> attackers = new HashSet<GameObject>();
    private Renderer rend;
    private Color originalColor;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Transform objTransform;

    private Coroutine blinkCoroutine;

    private PlayerXpBar playerXpBar;

    void Start()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        objTransform = transform;

        // Automatyczne znalezienie obiektu z tagiem "Player" i uzyskanie komponentu PlayerXpBar
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerXpBar = playerObject.GetComponent<PlayerXpBar>();
            if (playerXpBar == null)
            {
                Debug.LogError("Nie znaleziono komponentu PlayerXpBar na obiekcie z tagiem 'Player'.");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'Player'. Upewnij siê, ¿e obiekt gracza ma tag 'Player'.");
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        Debug.Log(gameObject.name + " Taking damage from: " + attacker.name);
        currentHealth -= damage;

        // Stop any ongoing blink coroutine before starting a new one
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkOnDamage(blinkColor));

        if (currentHealth <= 0)
        {
            if (!gameObject.CompareTag("Player"))
            {
                StartCoroutine(DieWithColorChange());
            }
        }

        attackers.Add(attacker);

        if(FloatingTextPrefab)
        {
            // Spawnowanie tekstu bez przypisywania go jako dziecko
            var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);

            // Pobieranie referencji do komponentu TextMesh
            var textMesh = go.GetComponent<TextMesh>();

            // Ustawianie tekstu
            textMesh.text = "-" + damage.ToString();

            // Obracanie tekstu w kierunku kamery
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                // Ustawienie rotacji w kierunku kamery
                go.transform.LookAt(go.transform.position + mainCamera.transform.rotation * Vector3.forward,
                                    mainCamera.transform.rotation * Vector3.up);
            }
            else
            {
                Debug.LogWarning("Main camera not found. Text rotation might not work correctly.");
            }
        }
    }

    public void Heal(float amount)
    {
        // Stop any ongoing blink coroutine before starting a new one
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkOnDamage(healBlinkColor));

        // Ustaw nowe zdrowie, ale nie przekraczaj maksymalnej wartoœci
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    IEnumerator BlinkOnDamage(Color blinkColor)
    {
        rend.material.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);
        rend.material.color = originalColor;

        // Reset blinkCoroutine to null when the blink is finished
        blinkCoroutine = null;
    }

    IEnumerator DieWithColorChange()
    {
        DisableComponentsExceptEssentials();
        rend.material.color = deathColor;
        yield return new WaitForSeconds(deathDuration);

        playerXpBar.GainExperience(experienceWorth);

        Destroy(gameObject);
    }

    private void DisableComponentsExceptEssentials()
    {
        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component != meshFilter && component != meshRenderer && component != objTransform && component != this)
            {
                Destroy(component);
            }
        }
    }

    public void RemoveAttacker(GameObject attacker)
    {
        attackers.Remove(attacker);
    }

    public List<GameObject> GetAttackersList()
    {
        return new List<GameObject>(attackers);
    }

    // Dodana funkcja zwracaj¹ca bie¿¹ce zdrowie
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}