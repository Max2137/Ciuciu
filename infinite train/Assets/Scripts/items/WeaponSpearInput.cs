using System.Collections;
using UnityEngine;

public class WeaponSpearInput : MonoBehaviour
{
    public Animator mAnimator;
    public GameObject attackSource; // Obiekt AudioSource
    public AudioClip attackClip; // D�wi�k ataku

    public float raycastDistance = 5f;
    [SerializeField] public int attackDamage;
    [SerializeField] public int attackPuncture = 1;
    [SerializeField] public float attackCooldown = 1.0f;

    private float lastAttackTime;
    private WeaponInputManager inputManager;

    //INPUT
    public void Start()
    {
        //mAnimator = GetComponent<Animator>();

        // Uzyskaj referencj� do WeaponInputManager z obiektu r�ki (parent)
        inputManager = GetComponentInParent<WeaponInputManager>();

        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found in the parent objects.");
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && CanAttack() && IsChildOfFirstSlot())
        {
            SpearDetect(attackDamage, attackPuncture);
            Debug.Log("ZadanoDamage");
            lastAttackTime = 0;

            // Odtw�rz d�wi�k ataku
            if (attackSource != null && attackClip != null)
            {
                AudioSource audioSource = attackSource.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(attackClip);
                }
            }
        }

        lastAttackTime += Time.deltaTime;
    }

    //DETECTION
    public void SpearDetect(float attackDamage, int attackPuncture)
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrAttack");
            Debug.Log("Animacja!");
        }

        // Wykonaj raycast
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, raycastDistance);
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green);

        // Iteruj przez trafienia z uwzgl�dnieniem attackPuncture
        for (int i = 0; i < Mathf.Min(hits.Length, attackPuncture); i++)
        {
            // Sprawd� czy trafiony obiekt ma tag "Enemy"
            if (hits[i].collider.CompareTag("Enemy"))
            {
                // Sprawd� czy obiekt ma skrypt UniversalHealth
                UniversalHealth enemyHealth = hits[i].collider.gameObject.GetComponent<UniversalHealth>();

                if (enemyHealth != null)
                {
                    // Zadaj obra�enia obiektowi, przekazuj�c attackDamage
                    GetComponent<WeaponAttack>().DealDamage(hits[i].collider.gameObject, attackDamage);
                }
            }
        }
    }

    // Sprawd� czy mo�na wykona� atak z uwzgl�dnieniem cooldownu
    private bool CanAttack()
    {
        return lastAttackTime >= attackCooldown;
    }

    // Dodatkowa metoda do sprawdzania, czy obiekt jest dzieckiem obiektu z tagiem "1stSlot"
    private bool IsChildOfFirstSlot()
    {
        Transform parent = transform.parent;
        return parent != null && parent.CompareTag("1stSlot");
    }

    // Rysuj linie raycasta w edytorze do cel�w wizualizacyjnych
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}