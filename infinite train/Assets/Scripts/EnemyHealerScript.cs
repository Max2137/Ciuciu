using UnityEngine;

public class EnemyHealerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;
    public float healingCooldown = 3f;
    public float healingAmount = 20f;
    public float switchTargetCooldown = 5f;
    public AudioClip healingSound;

    private Rigidbody healerRigidbody;
    [SerializeField] private UniversalHealth targetHealth;
    [SerializeField] private GameObject[] potentialTargets;
    private bool isHealing;
    private float currentHealingCooldown;
    private float currentSwitchTargetCooldown;
    private GameObject healingEffect;
    private AudioSource audioSource;

    private Animator mAnimator;

    void Start()
    {
        healerRigidbody = GetComponent<Rigidbody>();
        if (healerRigidbody == null)
        {
            Debug.LogError("Skrypt wymaga komponentu Rigidbody. Dodaj Rigidbody do lecznika.");
        }

        potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");

        FindNextTarget();

        if (transform.childCount > 0)
        {
            healingEffect = transform.GetChild(0).gameObject;
        }
        else
        {
            Debug.LogError("Nie znaleziono ¿adnych potomków (childów) tego obiektu.");
        }
        healingEffect.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = healingSound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;


        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        FindNextTarget();

        potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");

        if (targetHealth != null && targetHealth.gameObject.activeSelf && healerRigidbody != null)
        {
            Vector3 direction = targetHealth.transform.position - transform.position;
            float distanceToTarget = direction.magnitude;
            direction.Normalize();

            if (distanceToTarget > stoppingDistance)
            {
                healerRigidbody.velocity = direction * moveSpeed;
            }
            else
            {
                healerRigidbody.velocity = Vector3.zero;
            }

            if (!isHealing && currentHealingCooldown <= 0)
            {
                CheckAndHealTarget();
                currentHealingCooldown = healingCooldown;
            }

            if (currentSwitchTargetCooldown <= 0)
            {
                FindNextTarget();
                currentSwitchTargetCooldown = switchTargetCooldown;
            }

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
            FindNextTarget();
        }
        else
        {
            healingEffect.SetActive(false);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        if (healingEffect != null)
        {
            //healingEffect.SetActive(isHealing);
        }
    }

    void CheckAndHealTarget()
    {
        if (targetHealth != null && targetHealth.currentHealth < targetHealth.maxHealth * 0.9f)
        {
            isHealing = true;
            Debug.Log("Healing target");
            targetHealth.Heal(healingAmount);
            healingEffect.SetActive(true);

            if (healingSound != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(healingSound);
            }

            if (targetHealth.currentHealth >= targetHealth.maxHealth * 0.9f)
            {
                healingEffect.SetActive(false);
                FindNextTarget();
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }

            isHealing = false;
        }
    }

    void FindNextTarget()
    {
        System.Array.Sort(potentialTargets, CompareTargets);

        foreach (GameObject potentialTarget in potentialTargets)
        {
            if (potentialTarget == null)
            {
                continue;
            }

            UniversalHealth health = potentialTarget.GetComponent<UniversalHealth>();

            if (health != null && health.currentHealth < health.maxHealth * 0.9f && health.gameObject.activeSelf)
            {
                targetHealth = health;
                return;
            }
        }

        targetHealth = null;
    }

    int CompareTargets(GameObject target1, GameObject target2)
    {
        if (target1 == null || target2 == null)
        {
            return 0;
        }

        float distance1 = Vector3.Distance(transform.position, target1.transform.position);
        float distance2 = Vector3.Distance(transform.position, target2.transform.position);

        return distance1.CompareTo(distance2);
    }
}