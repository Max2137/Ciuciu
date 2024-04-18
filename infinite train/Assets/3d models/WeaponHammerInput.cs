using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHammerInput : MonoBehaviour
{
    public Animator mAnimator;

    public float raycastDistance = 5f;
    public int attackDamage;
    public float attackCooldown = 1.0f;
    public float attackPrepTime = 1.5f;

    public AudioClip hitSound; // Dodaj pole dla dŸwiêku

    private float lastAttackTime;
    private float attackPrepTimer;
    private bool isReadyForAttack;

    private Vector3 lastPlayerPosition;
    private WeaponInputManager inputManager;

    void Start()
    {
        lastPlayerPosition = transform.position;
        inputManager = GetComponentInParent<WeaponInputManager>();
        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found in the parent objects.");
        }
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

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Wykryto wroga");
                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();
                if (enemyHealth != null)
                {
                    GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
                    Debug.Log("zadano damage");
                }
            }
        }
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