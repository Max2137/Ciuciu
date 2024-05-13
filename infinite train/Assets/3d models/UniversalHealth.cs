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

    public AudioClip audioClip; // Zmienna do przechowywania d�wi�ku
    public AudioClip PlayerHurt;
    public List<AudioClip> GeneralHurtSounds; // List of general hurt sounds
    private AudioSource audioSource; // Komponent AudioSource do odtwarzania d�wi�ku
    public GameObject deathEffectPrefab; // Dodaj publiczn� zmienn� przechowuj�c� efekt �mierci

    private Animator mAnimator;

    private PlayerStats playerStats;  // Referencja do skryptu PlayerStats

    // Metoda do odtwarzania d�wi�ku
    public void PlayAudio(AudioClip clip)
    {
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

        // Sprawd�, czy d�wi�k jest dost�pny
        if (clip != null && audioSource != null)
        {
            // Ustaw d�wi�k
            audioSource.clip = clip;

            // Odtw�rz d�wi�k
            audioSource.Play();
            Debug.Log("Zagrano D�wi�k!");
        }
        else
        {
            Debug.LogError("Error w graniu: " + clip);
        }
    }

    // Method to play a random audio clip from a list
    public void PlayRandomAudio(List<AudioClip> clips)
    {
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


        mAnimator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        Debug.Log(gameObject.name + " Taking damage from: " + attacker.name);

        float damageOutput = damage + playerStats.AttackMeleeStat;

        // Je�li obiekt ma tag "Player", odtw�rz d�wi�k obra�e� gracza
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
        DisableComponentsExceptEssentials();
        rend.material.color = deathColor;
        yield return new WaitForSeconds(deathDuration);

        playerXpBar.GainExperience(experienceWorth);

        if (deathEffectPrefab != null)
        {
            Vector3 deathEffectPosition = transform.position;
            deathEffectPosition.y = 0.25f; // Ustaw wysoko�� na 0.25 na osi Y

            Instantiate(deathEffectPrefab, deathEffectPosition, Quaternion.identity); // Instancjonuj efekt �mierci
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

    // Dodana funkcja zwracaj�ca bie��ce zdrowie
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}