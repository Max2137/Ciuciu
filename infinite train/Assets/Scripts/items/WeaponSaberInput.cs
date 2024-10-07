using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaberInput : MonoBehaviour
{
    public Animator mAnimator;

    public AudioClip hitSound; // Dodaj pole dla d�wi�ku

    public float raycastDistance = 5f;  // D�ugo�� raycasta
    public int attackDamage;
    public float attackCooldown = 1.0f;  // Czas oczekiwania mi�dzy atakami
    public int numberOfRays = 9;  // Ilo�� promieni w wachlarzu
    public float fanAngle = 90f;  // K�t wachlarza w stopniach

    private float lastAttackTime;  // Czas ostatniego ataku
    private List<GameObject> enemiesHitThisAttack = new List<GameObject>();  // Lista obiekt�w, kt�re ju� otrzyma�y obra�enia

    private WeaponInputManager inputManager;

    // Klasa efekt�w ataku
    [System.Serializable]
    public class AttackEffect
    {
        public GameObject effectObject;  // Obiekt efektu
        public float effectDelay;  // Op�nienie przed aktywacj� efektu
        public float effectTime;  // Czas dzia�ania efektu
    }

    // Lista efekt�w ataku
    public List<AttackEffect> attackEffects = new List<AttackEffect>();

    //INPUT
    public void Start()
    {
        // Uzyskaj referencj� do WeaponInputManager z obiektu r�ki (parent)
        inputManager = GetComponentInParent<WeaponInputManager>();

        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found in the parent objects.");
        }

        // Deaktywuj wszystkie efekty na starcie
        DeactivateAllEffects();
    }

    //INPUT
    public void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && CanAttack() && IsChildOfFirstSlot())
        {
            FanDetect(attackDamage);
            lastAttackTime = Time.time;
            enemiesHitThisAttack.Clear();  // Wyczy�� list� po ka�dym ataku

            // Aktywuj efekty ataku z op�nieniem
            ActivateAttackEffectsWithDelay();
        }
    }

    //DETECTION
    public void FanDetect(float attackDamage)
    {
        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrAttack");
            Debug.Log("Animacja!");
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        // Oblicz k�t pomi�dzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Iteruj przez ka�dy promie� w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            // Oblicz kierunek promienia wachlarza
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.up);
            Vector3 direction = rotation * transform.forward;

            // Wykonaj raycast
            RaycastHit hit;
            Ray ray = new Ray(transform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.green);

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Sprawd�, czy trafiony obiekt ma tag "Enemy" i nie otrzyma� jeszcze obra�e� w ramach tego ataku
                if (hit.collider.CompareTag("Enemy") && !enemiesHitThisAttack.Contains(hit.collider.gameObject))
                {
                    // Dodaj obiekt do listy, aby unikn�� wielokrotnego zadawania obra�e�
                    enemiesHitThisAttack.Add(hit.collider.gameObject);

                    // Sprawd�, czy obiekt ma skrypt UniversalHealth
                    UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

                    if (enemyHealth != null)
                    {
                        // Zadaj obra�enia obiektowi, przekazuj�c attackDamage
                        GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
                    }
                }
            }
        }
    }

    // Aktywuj efekty ataku z op�nieniem
    private void ActivateAttackEffectsWithDelay()
    {
        foreach (var effect in attackEffects)
        {
            if (effect.effectObject != null)
            {
                // Uruchom Coroutine, aby aktywowa� efekt po op�nieniu
                StartCoroutine(ActivateEffectAfterDelay(effect.effectObject, effect.effectDelay, effect.effectTime));
            }
        }
    }

    // Coroutine do aktywowania efektu po op�nieniu i deaktywacji po czasie trwania
    private IEnumerator ActivateEffectAfterDelay(GameObject effectObject, float delay, float effectTime)
    {
        // Poczekaj na op�nienie
        yield return new WaitForSeconds(delay);

        // Aktywuj obiekt efektu
        if (effectObject != null)
        {
            effectObject.SetActive(true);

            // Poczekaj na czas trwania efektu
            yield return new WaitForSeconds(effectTime);

            // Deaktywuj obiekt efektu
            effectObject.SetActive(false);
        }
    }

    // Deaktywuj wszystkie efekty na starcie
    private void DeactivateAllEffects()
    {
        foreach (var effect in attackEffects)
        {
            if (effect.effectObject != null)
            {
                effect.effectObject.SetActive(false);
            }
        }
    }

    // Sprawd�, czy mo�na wykona� atak z uwzgl�dnieniem cooldownu
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

    // Rysuj linie raycast�w w edytorze do cel�w wizualizacyjnych
    void OnDrawGizmos()
    {
        // Oblicz k�t pomi�dzy promieniami w wachlarzu
        float angleStep = fanAngle / (numberOfRays - 1);

        // Rysuj ka�dy promie� w wachlarzu
        for (int i = 0; i < numberOfRays; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(-fanAngle / 2 + i * angleStep, transform.up);
            Vector3 direction = rotation * transform.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction * raycastDistance);
        }
    }
}