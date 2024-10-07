using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LootItem
{
    public string itemName;
    public int minQuantity;
    public int maxQuantity;
}

public enum EDamageType
{
    MELEE,
    MAGIC,
    OTHER
}

public class UniversalHealth : MonoBehaviour
{
    public List<LootItem> lootOnDeath = new List<LootItem>();
    private itemsListScript itemsList;

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

    public AudioClip audioClip;
    public AudioClip PlayerHurt;
    public List<AudioClip> GeneralHurtSounds;
    private AudioSource audioSource;
    public GameObject deathEffectPrefab;

    private Animator mAnimator;

    private PlayerStats playerStats;

    public CameraShake cameraShake;  // Referencja do CameraShake

    void Start()
    {
        itemsList = FindObjectOfType<itemsListScript>();
        if (itemsList == null)
        {
            Debug.LogError("Nie znaleziono obiektu itemsListScript w scenie!");
        }

        playerStats = FindObjectOfType<PlayerStats>();

        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        objTransform = transform;

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

        GameObject audioPlayerObject = GameObject.FindGameObjectWithTag("AudioPlayer");
        if (audioPlayerObject != null)
        {
            audioSource = audioPlayerObject.GetComponent<AudioSource>();
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

        // Automatyczne znajdowanie referencji do CameraShake
        cameraShake = FindObjectOfType<CameraShake>();
        if (cameraShake == null)
        {
            Debug.LogError("Nie znaleziono komponentu CameraShake w scenie!");
        }
    }

    public void TakeDamage(float damage, GameObject attacker, EDamageType dmgType)
    {
        Debug.Log(gameObject.name + " Taking damage from: " + attacker.name);

        float damageOutput = damage;
        Debug.Log(damageOutput);

        switch (dmgType)
        {
            case EDamageType.MELEE:
                {
                    Debug.Log(damageOutput);

                    Transform rootTransform = attacker.transform.root;

                    if (rootTransform.CompareTag("Player"))
                    {
                        Debug.Log(damageOutput);
                        damageOutput += playerStats.AttackMeleeStat;
                        Debug.Log(damageOutput);
                    }
                    else if (attacker.CompareTag("Enemy"))
                    {
                        // SprawdŸ, czy gracz ma obiekt z komponentem ArmorDefensePassive
                        ArmorDefensePassive armorDefense = GetComponentInChildren<ArmorDefensePassive>();
                        if (armorDefense != null)
                        {
                            damageOutput -= armorDefense.damageAbsortion;
                            if (armorDefense.isThorns)
                            {
                                // Oblicz obra¿enia Thorns
                                float thornsDamage = damage * armorDefense.ThornsFraction;
                                // Zadaj obra¿enia przeciwnikowi
                                UniversalHealth enemyHealth = attacker.GetComponent<UniversalHealth>();
                                if (enemyHealth != null)
                                {
                                    enemyHealth.TakeDamage(thornsDamage, gameObject, EDamageType.OTHER);
                                }
                            }
                        }
                    }
                    break;
                }
            case EDamageType.MAGIC:
                {
                    Debug.Log("Atakuje " + attacker.name);
                    if (attacker.CompareTag("Player") || attacker == gameObject || attacker.CompareTag("EnemiesAttacker"))
                    {
                        damageOutput += playerStats.AttackMagicStat;
                        Debug.Log("Dodano AttackMagicStat do damageOutput.");
                    }
                    else
                    {
                        Debug.Log("Atakuj¹cy nie jest graczem ani nie atakuje samego siebie.");
                    }
                    break;
                }
            case EDamageType.OTHER:
                {
                    break;
                }
        }

        if (gameObject.CompareTag("Player"))
        {
            if (playerStats.DefenseGeneralStat > 0)
            {
                currentHealth -= damageOutput / playerStats.DefenseGeneralStat;
            }
            else
            {
                currentHealth -= damageOutput;
            }

            if (cameraShake != null)
            {
                cameraShake.Shake(0.5f, 0.2f); // Adjust duration and magnitude as needed
            }

            PlayAudio(PlayerHurt);
        }
        else
        {
            currentHealth -= damageOutput;
            PlayRandomAudio(GeneralHurtSounds);
            mAnimator.SetTrigger("hit");

            if (FloatingTextPrefab)
            {
                var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);
                var textMesh = go.GetComponent<TextMesh>();
                textMesh.text = "-" + damageOutput.ToString();
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

        playerXpBar.GainExperience(experienceWorth);

        foreach (LootItem lootItem in lootOnDeath)
        {
            int quantity = Random.Range(lootItem.minQuantity, lootItem.maxQuantity + 1);
            itemsList.Gain(lootItem.itemName, quantity);
        }

        if (deathEffectPrefab != null)
        {
            Vector3 deathEffectPosition = transform.position;
            deathEffectPosition.y = 0.25f;
            Instantiate(deathEffectPrefab, deathEffectPosition, Quaternion.identity);
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

    public void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log("Played Audio!");
        }
        else
        {
            Debug.LogError("Audio clip not set or AudioSource component not set!");
        }
    }

    public void PlayRandomAudio(List<AudioClip> clips)
    {
        if (clips != null && clips.Count > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, clips.Count);
            audioSource.clip = clips[randomIndex];
            audioSource.Play();
            Debug.Log("Played Random Audio!");
        }
        else
        {
            Debug.LogError("Audio clips list is empty or AudioSource component not set!");
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}