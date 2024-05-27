using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponHammerInput : MonoBehaviour
{
    public Animator mAnimator;
    public GameObject hitEffectPrefab;
    public float raycastDistance = 5f;
    public int attackDamage;
    public float attackCooldown = 1.0f;
    public float attackPrepTime = 1.5f;
    public AudioClip hitSound;
    private float lastAttackTime;
    private float attackPrepTimer;
    private bool isReadyForAttack;
    private Vector3 lastPlayerPosition;
    private WeaponInputManager inputManager;
    private WeaponAttack weaponAttack;
    private Vector3 spawnPosition;
    private List<GameObject> spawnedHitEffects = new List<GameObject>();

    void Start()
    {
        lastPlayerPosition = transform.position;
        inputManager = GetComponentInParent<WeaponInputManager>();
        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found in the parent objects.");
        }

        weaponAttack = GetComponent<WeaponAttack>();
        if (weaponAttack == null)
        {
            Debug.LogError("WeaponAttack component not found on the object.");
        }

        // Subscribe to the scene unloaded event
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene unloaded event
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneUnloaded(Scene scene)
    {
        // Destroy all spawned hitEffectPrefabs when a scene is unloaded
        foreach (GameObject hitEffect in spawnedHitEffects)
        {
            Destroy(hitEffect);
        }
        spawnedHitEffects.Clear();
    }

    void Update()
    {
        if (transform.position != lastPlayerPosition)
        {
            //attackPrepTimer = 0f;
            //isReadyForAttack = false;
        }

        lastPlayerPosition = transform.position;

        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && CanAttack())
        {
            attackPrepTimer = 0f;
        }

        if (Input.GetMouseButton((int)inputManager.attackMouseButton) && attackPrepTimer < attackPrepTime)
        {
            attackPrepTimer += Time.deltaTime;

            if (attackPrepTimer >= attackPrepTime)
            {
                isReadyForAttack = true;
                lastAttackTime = Time.time;
                Debug.Log("gotowy");
            }
        }

        if (Input.GetMouseButtonUp((int)inputManager.attackMouseButton))
        {
            if (isReadyForAttack)
            {
                Detect(attackDamage);
                // Odtwórz dŸwiêk po 0.5 sekundy
                Invoke("PlayHitSound", 0.5f);
            }

            attackPrepTimer = 0f;
            isReadyForAttack = false;
        }
    }

    void PlayHitSound()
    {
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }

    void Detect(float attackDamage)
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrAttack");
            Debug.Log("Animacja!");
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);
        Debug.Log("raycastowano");

        // Nowa logika dla ustalenia spawnPosition
        spawnPosition = ray.origin + ray.direction * (raycastDistance - 1);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            spawnPosition = hit.point; // If hit, use hit point as spawn position
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Wykryto wroga");
                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();
                if (enemyHealth != null)
                {
                    weaponAttack.DealDamage(hit.collider.gameObject, attackDamage);
                    Debug.Log("zadano damage");
                }
            }
        }

        Debug.Log(weaponAttack.isBurning);

        // Spawn hit effect prefab only if isBurning is true
        if (hitEffectPrefab != null && weaponAttack != null && weaponAttack.isBurning)
        {
            // OpóŸnij spawn hit effect o 0,6 sekundy
            Invoke("DelayedSpawnHitEffect", 0.6f);
        }
    }

    void DelayedSpawnHitEffect()
    {
        GameObject newHitEffect = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
        spawnedHitEffects.Add(newHitEffect);
    }

    void CallSpawnHitEffect()
    {
        // Wywo³aj metodê SpawnHitEffect przekazuj¹c informacjê o miejscu spawnu
        SpawnHitEffect(spawnPosition);
    }

    void SpawnHitEffect(Vector3 position)
    {
        // Kod wczeœniejszy pozostaje bez zmian
        Instantiate(hitEffectPrefab, position, Quaternion.identity);
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}