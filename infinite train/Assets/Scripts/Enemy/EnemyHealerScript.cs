using UnityEngine;
using UnityEngine.AI;

public class EnemyHealerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;
    public float healingCooldown = 3f;
    public float healingAmount = 20f;
    public float switchTargetCooldown = 5f;
    public AudioClip healingSound;

    private NavMeshAgent navAgent;
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
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("Script requires a NavMeshAgent component. Add NavMeshAgent to the healer.");
        }

        navAgent.stoppingDistance = stoppingDistance;
        navAgent.speed = moveSpeed;

        potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");

        FindNextTarget();

        if (transform.childCount > 0)
        {
            healingEffect = transform.GetChild(0).gameObject;
        }
        else
        {
            Debug.LogError("No child object found.");
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

        if (targetHealth != null && targetHealth.gameObject.activeSelf)
        {
            navAgent.SetDestination(targetHealth.transform.position);

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

        if (targetHealth == null)
        {
            healingEffect.SetActive(false);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
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
            if (potentialTarget == null) continue;

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
        if (target1 == null || target2 == null) return 0;

        float distance1 = Vector3.Distance(transform.position, target1.transform.position);
        float distance2 = Vector3.Distance(transform.position, target2.transform.position);

        return distance1.CompareTo(distance2);
    }
}