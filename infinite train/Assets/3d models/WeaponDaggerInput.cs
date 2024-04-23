using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDaggerInput : MonoBehaviour
{
    public Animator mAnimator;
    public GameObject attackSource; // Obiekt AudioSource
    public AudioClip attackClip; // DŸwiêk ataku

    public float raycastDistance = 5f;  // D³ugoœæ raycasta
    public int attackDamage;
    public float attackCooldown = 1.0f;  // Czas oczekiwania miêdzy atakami

    private float lastAttackTime;  // Czas ostatniego ataku
    private WeaponInputManager inputManager;

    //INPUT
    public void Start()
    {
        // Uzyskaj referencjê do WeaponInputManager z obiektu rêki (parent)
        inputManager = GetComponentInParent<WeaponInputManager>();

        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found in the parent objects.");
        }
    }

    //INPUT
    public void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && CanAttack() && IsChildOfFirstSlot())
        {
            Detect(attackDamage);
            lastAttackTime = Time.time;

            // Odtwórz dŸwiêk ataku
            if (attackSource != null && attackClip != null)
            {
                AudioSource audioSource = attackSource.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(attackClip);
                }
            }
        }
    }

    //DETECTION
    public void Detect(float attackDamage)
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrAttack");
            Debug.Log("Animacja!");
        }


        // Wykonaj raycast
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // SprawdŸ czy trafiony obiekt ma tag "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // SprawdŸ czy obiekt ma skrypt UniversalHealth
                UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null)
                {
                    // Zadaj obra¿enia obiektowi, przekazuj¹c attackDamage
                    GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
                }
            }
        }
    }

    // SprawdŸ czy mo¿na wykonaæ atak z uwzglêdnieniem cooldownu
    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    // Dodatkowa metoda do sprawdzania, czy obiekt jest dzieckiem obiektu z tagiem "1stSlot"
    private bool IsChildOfFirstSlot()
    {
        Transform parent = transform.parent;
        return parent != null && parent.CompareTag("1stSlot");
    }

    // Rysuj linie raycasta w edytorze do celów wizualizacyjnych
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}