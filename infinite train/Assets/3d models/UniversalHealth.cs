using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalHealth : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public float maxHealth;
    public float currentHealth;
    public float blinkDuration = 0.2f;
    public Color blinkColor = Color.red;
    public Color healBlinkColor = Color.green;
    public Color deathColor = Color.black;
    public float deathDuration = 0.5f;
    public int experienceWorth;

    private HashSet<GameObject> attackers = new HashSet<GameObject>();
    private Renderer rend;
    private Color originalColor;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Transform objTransform;

    private Coroutine blinkCoroutine;

    private PlayerXpBar playerXpBar;
    [SerializeField] private AudioSource audioSource; // Dodajemy pole przechowuj¹ce komponent AudioSource

    public AudioClip audioClip;
    public AudioClip PlayerHurt;
    public List<AudioClip> GeneralHurtSounds;
    public GameObject deathEffectPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        objTransform = transform;

        // Pobieramy nazwê bie¿¹cej sceny
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Znajdujemy wszystkie obiekty z tagiem "AudioPlayer" na bie¿¹cej scenie
        GameObject[] audioPlayers = GameObject.FindGameObjectsWithTag("AudioPlayer");

        // Szukamy obiektu "AudioPlayer" na bie¿¹cej scenie
        foreach (GameObject audioPlayer in audioPlayers)
        {
            if (audioPlayer.scene.name == currentSceneName)
            {
                // Jeœli znaleziono obiekt "AudioPlayer" na bie¿¹cej scenie, pobieramy jego komponent AudioSource
                audioSource = audioPlayer.GetComponent<AudioSource>();
                break; // Przerywamy pêtlê, gdy znaleziono odpowiedni obiekt
            }
        }

        // Sprawdzamy, czy uda³o siê znaleŸæ obiekt "AudioPlayer" na bie¿¹cej scenie
        if (audioSource == null)
        {
            Debug.LogError("Nie znaleziono komponentu AudioSource na obiekcie z tagiem 'AudioPlayer' na scenie " + currentSceneName + ".");
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        // Pobieramy nazwê bie¿¹cej sceny
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Znajdujemy wszystkie obiekty z tagiem "AudioPlayer" na bie¿¹cej scenie
        GameObject[] audioPlayers = GameObject.FindGameObjectsWithTag("AudioPlayer");

        // Szukamy obiektu "AudioPlayer" na bie¿¹cej scenie
        foreach (GameObject audioPlayer in audioPlayers)
        {
            if (audioPlayer.scene.name == currentSceneName)
            {
                // Jeœli znaleziono obiekt "AudioPlayer" na bie¿¹cej scenie, pobieramy jego komponent AudioSource
                audioSource = audioPlayer.GetComponent<AudioSource>();
                break; // Przerywamy pêtlê, gdy znaleziono odpowiedni obiekt
            }
        }

        // Sprawdzamy, czy uda³o siê znaleŸæ obiekt "AudioPlayer" na bie¿¹cej scenie
        if (audioSource == null)
        {
            Debug.LogError("Nie znaleziono komponentu AudioSource na obiekcie z tagiem 'AudioPlayer' na scenie " + currentSceneName + ".");
        }

        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log("Zagrano DŸwiêk! " + clip + " audio source: " + audioSource);
        }
        else
        {
            Debug.LogError("B³¹d odtwarzania dŸwiêku: " + clip + " audio source: " + audioSource);
        }
    }

    public void PlayRandomAudio(List<AudioClip> clips)
    {
        // Pobieramy nazwê bie¿¹cej sceny
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Znajdujemy wszystkie obiekty z tagiem "AudioPlayer" na bie¿¹cej scenie
        GameObject[] audioPlayers = GameObject.FindGameObjectsWithTag("AudioPlayer");

        // Szukamy obiektu "AudioPlayer" na bie¿¹cej scenie
        foreach (GameObject audioPlayer in audioPlayers)
        {
            if (audioPlayer.scene.name == currentSceneName)
            {
                // Jeœli znaleziono obiekt "AudioPlayer" na bie¿¹cej scenie, pobieramy jego komponent AudioSource
                audioSource = audioPlayer.GetComponent<AudioSource>();
                break; // Przerywamy pêtlê, gdy znaleziono odpowiedni obiekt
            }
        }

        // Sprawdzamy, czy uda³o siê znaleŸæ obiekt "AudioPlayer" na bie¿¹cej scenie
        if (audioSource == null)
        {
            Debug.LogError("Nie znaleziono komponentu AudioSource na obiekcie z tagiem 'AudioPlayer' na scenie " + currentSceneName + ".");
        }

        if (clips != null && clips.Count > 0 && audioSource != null)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Count);
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
            Debug.Log("Played Audio!");
        }
        else
        {
            Debug.LogError("Audio clips list is empty or AudioSource component not set!");
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        Debug.Log(gameObject.name + " Taking damage from: " + attacker.name);

        if (gameObject.CompareTag("Player"))
        {
            PlayAudio(PlayerHurt);
        }
        else
        {
            PlayRandomAudio(GeneralHurtSounds);
        }

        currentHealth -= damage;

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkOnDamage(blinkColor));

        if (currentHealth <= 0)
        {
            if (!gameObject.CompareTag("Player"))
            {
                PlayAudio(audioClip);
                StartCoroutine(DieWithColorChange());
            }
        }

        attackers.Add(attacker);

        if (FloatingTextPrefab)
        {
            var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
            var textMesh = go.GetComponent<TextMesh>();
            textMesh.text = "-" + damage.ToString();
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
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
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        blinkCoroutine = StartCoroutine(BlinkOnDamage(healBlinkColor));

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    IEnumerator BlinkOnDamage(Color blinkColor)
    {
        rend.material.color = blinkColor;
        yield return new WaitForSeconds(blinkDuration);
        rend.material.color = originalColor;
        blinkCoroutine = null;
    }

    IEnumerator DieWithColorChange()
    {
        DisableComponentsExceptEssentials();
        rend.material.color = deathColor;
        yield return new WaitForSeconds(deathDuration);

        if (playerXpBar != null)
        {
            playerXpBar.GainExperience(experienceWorth);
        }
        else
        {
            Debug.LogWarning("PlayerXpBar is not assigned.");
        }

        if (deathEffectPrefab != null)
        {
            Vector3 deathEffectPosition = transform.position;
            deathEffectPosition.y = 0.25f;
            Instantiate(deathEffectPrefab, deathEffectPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void DisableComponentsExceptEssentials()
    {
        UnityEngine.Component[] components = GetComponents<UnityEngine.Component>();
        foreach (UnityEngine.Component component in components)
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}