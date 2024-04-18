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


    public AudioClip audioClip; // Zmienna do przechowywania d�wi�ku

    private AudioSource audioSource; // Komponent AudioSource do odtwarzania d�wi�ku

    public GameObject deathEffectPrefab; // Dodaj publiczn� zmienn� przechowuj�c� efekt �mierci


    // Metoda do odtwarzania d�wi�ku
    public void PlayAudio()
    {
        //Debug.Log("Zagrano D�wi�k!");

        // Sprawd�, czy d�wi�k jest dost�pny
        if (audioSource.clip != null)
        {
            // Odtw�rz d�wi�k
            audioSource.Play();
            Debug.Log("Zagrano D�wi�k!");
        }
        else
        {
            Debug.LogError("Nie ustawiono pliku audio!");
        }
    }

    // W skrypcie UniversalHealth

    // W skrypcie UniversalHealth

    void Start()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        //originalColor = rend.material.color;

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
            Debug.LogError("Nie znaleziono obiektu z tagiem 'Player'. Upewnij si�, �e obiekt gracza ma tag 'Player'.");
        }


        // Znajd� obiekt na scenie z tagiem "audioPlayer"
        GameObject audioPlayerObject = GameObject.FindGameObjectWithTag("AudioPlayer");

        // Sprawd�, czy obiekt �r�d�a d�wi�ku zosta� znaleziony
        if (audioPlayerObject != null)
        {
            // Spr�buj pobra� komponent AudioSource z obiektu �r�d�a d�wi�ku
            audioSource = audioPlayerObject.GetComponent<AudioSource>();

            // Je�li komponent AudioSource zosta� znaleziony, ustaw d�wi�k
            if (audioSource != null)
            {
                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                }
                else
                {
                    Debug.LogError("Nie ustawiono pliku audio w inspektorze!");
                }
            }
            else
            {
                Debug.LogError("Komponent AudioSource nie zosta� znaleziony na obiekcie �r�d�a d�wi�ku!");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'audioPlayer' na scenie!");
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
            if (deathEffectPrefab != null)
            {
                Vector3 deathEffectPosition = transform.position;
                deathEffectPosition.y = 0.25f; // Ustaw wysoko�� na 0.25 na osi Y

                Instantiate(deathEffectPrefab, deathEffectPosition, Quaternion.identity); // Instancjonuj efekt �mierci
            }

            //Debug.Log("Zagrano D�wi�k!");
            PlayAudio();

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

        // Ustaw nowe zdrowie, ale nie przekraczaj maksymalnej warto�ci
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
        //PlayAudio();
        //Debug.Log("Zagrano D�wi�k!");

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

    // Dodana funkcja zwracaj�ca bie��ce zdrowie
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}