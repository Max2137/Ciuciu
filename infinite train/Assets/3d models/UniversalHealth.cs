using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    public AudioClip audioClip; // Zmienna do przechowywania dŸwiêku
    public AudioClip PlayerHurt;
    public List<AudioClip> GeneralHurtSounds; // List of general hurt sounds
    private AudioSource audioSource; // Komponent AudioSource do odtwarzania dŸwiêku
    public GameObject deathEffectPrefab; // Dodaj publiczn¹ zmienn¹ przechowuj¹c¹ efekt œmierci

    private Animator mAnimator;

    private PlayerStats playerStats;  // Referencja do skryptu PlayerStats

    // Metoda do odtwarzania dŸwiêku
    public void PlayAudio(AudioClip clip)
    {
        // ZnajdŸ obiekt na scenie z tagiem "audioPlayer"
        GameObject audioPlayerObject = GameObject.FindGameObjectWithTag("AudioPlayer");

        // SprawdŸ, czy obiekt Ÿród³a dŸwiêku zosta³ znaleziony
        if (audioPlayerObject != null)
        {
            // Spróbuj pobraæ komponent AudioSource z obiektu Ÿród³a dŸwiêku
            audioSource = audioPlayerObject.GetComponent<AudioSource>();

            // Jeœli komponent AudioSource zosta³ znaleziony, ustaw dŸwiêk
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
                Debug.LogError("Komponent AudioSource nie zosta³ znaleziony na obiekcie Ÿród³a dŸwiêku!");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'audioPlayer' na scenie!");
        }

        // SprawdŸ, czy dŸwiêk jest dostêpny
        if (clip != null && audioSource != null)
        {
            // Ustaw dŸwiêk
            audioSource.clip = clip;

            // Odtwórz dŸwiêk
            audioSource.Play();
            Debug.Log("Zagrano DŸwiêk!");
        }
        else
        {
            Debug.LogError("Error w graniu: " + clip);
        }
    }

    // Method to play a random audio clip from a list
    public void PlayRandomAudio(List<AudioClip> clips)
    {
        // ZnajdŸ obiekt na scenie z tagiem "audioPlayer"
        GameObject audioPlayerObject = GameObject.FindGameObjectWithTag("AudioPlayer");

        // SprawdŸ, czy obiekt Ÿród³a dŸwiêku zosta³ znaleziony
        if (audioPlayerObject != null)
        {
            // Spróbuj pobraæ komponent AudioSource z obiektu Ÿród³a dŸwiêku
            audioSource = audioPlayerObject.GetComponent<AudioSource>();

            // Jeœli komponent AudioSource zosta³ znaleziony, ustaw dŸwiêk
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
                Debug.LogError("Komponent AudioSource nie zosta³ znaleziony na obiekcie Ÿród³a dŸwiêku!");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'audioPlayer' na scenie!");
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

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

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
            Debug.LogError("Nie znaleziono obiektu z tagiem 'Player'. Upewnij siê, ¿e obiekt gracza ma tag 'Player'.");
        }

        // ZnajdŸ obiekt na scenie z tagiem "audioPlayer"
        GameObject audioPlayerObject = GameObject.FindGameObjectWithTag("AudioPlayer");

        // SprawdŸ, czy obiekt Ÿród³a dŸwiêku zosta³ znaleziony
        if (audioPlayerObject != null)
        {
            // Spróbuj pobraæ komponent AudioSource z obiektu Ÿród³a dŸwiêku
            audioSource = audioPlayerObject.GetComponent<AudioSource>();

            // Jeœli komponent AudioSource zosta³ znaleziony, ustaw dŸwiêk
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
                Debug.LogError("Komponent AudioSource nie zosta³ znaleziony na obiekcie Ÿród³a dŸwiêku!");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu z tagiem 'audioPlayer' na scenie!");
        }


        mAnimator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        Debug.Log(gameObject.name + " Taking damage from: " + attacker.name);

        float damageOutput = damage + playerStats.AttackMeleeStat;

        // Jeœli obiekt ma tag "Player", odtwórz dŸwiêk obra¿eñ gracza
        if (gameObject.CompareTag("Player"))
        {
            if (playerStats.DefenseGeneralStat>0)
            {
                currentHealth -= damageOutput / playerStats.DefenseGeneralStat;
            }
            else
            {
                currentHealth -= damageOutput;
            }
            
            PlayAudio(PlayerHurt);
        }
        else
        {
            currentHealth -= damageOutput;
            // If not player, play a random general hurt sound
            PlayRandomAudio(GeneralHurtSounds);
            mAnimator.SetTrigger("hit");

            if (FloatingTextPrefab)
            {
                // Spawnowanie tekstu bez przypisywania go jako dziecko
                var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);

                // Pobieranie referencji do komponentu TextMesh
                var textMesh = go.GetComponent<TextMesh>();

                // Ustawianie tekstu
                textMesh.text = "-" + damageOutput.ToString();

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
                PlayAudio(audioClip);

                StartCoroutine(DieWithColorChange());
            }
        }

        attackers.Add(attacker);

       
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

        if (deathEffectPrefab != null)
        {
            Vector3 deathEffectPosition = transform.position;
            deathEffectPosition.y = 0.25f; // Ustaw wysokoœæ na 0.25 na osi Y

            Instantiate(deathEffectPrefab, deathEffectPosition, Quaternion.identity); // Instancjonuj efekt œmierci
        }

        
        mAnimator.SetTrigger("dead");
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

    // Dodana funkcja zwracaj¹ca bie¿¹ce zdrowie
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}