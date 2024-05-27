using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHoldManager : MonoBehaviour
{
    public float attackDamage = 10;
    public string holdingEffectScriptName; // Nazwa skryptu efektu trzymania

    private WeaponAudioVisual weaponAudioVisual;
    private ParentCheckScript parentCheckScript;
    private WeaponInputManager inputManager;
    private WeaponDetection detection;
    private MonoBehaviour holdingEffect; // Zmienna do przechowywania komponentu skryptu

    private bool isHolding = false;
    private HashSet<GameObject> enemiesHitThisFrame = new HashSet<GameObject>(); // Zestaw przechowuj�cy przeciwnik�w trafionych w bie��cej ramce
    private HashSet<GameObject> enemiesHitDuringHold = new HashSet<GameObject>(); // Zestaw przechowuj�cy przeciwnik�w trafionych podczas trzymania

    void Start()
    {
        weaponAudioVisual = GetComponentInChildren<WeaponAudioVisual>();
        parentCheckScript = GetComponent<ParentCheckScript>();
        inputManager = GetComponent<WeaponInputManager>();
        detection = GetComponentInChildren<WeaponDetection>();

        if (weaponAudioVisual == null)
        {
            Debug.LogError("WeaponAudioVisual not found on the child objects.");
        }
        if (parentCheckScript == null)
        {
            Debug.LogError("ParentCheckScript not found on the object.");
        }
        if (inputManager == null)
        {
            Debug.LogError("WeaponInputManager not found on the object.");
        }
        if (detection == null)
        {
            Debug.LogError("WeaponDetection not found on the child objects.");
        }

        if (string.IsNullOrEmpty(holdingEffectScriptName))
        {
            Debug.LogError("HoldingEffect script name is not set.");
        }
        else
        {
            // Wyszukaj komponent skryptu po nazwie
            holdingEffect = GetComponent(holdingEffectScriptName) as MonoBehaviour;
            if (holdingEffect == null)
            {
                Debug.LogError("HoldingEffect script not found on the object.");
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton((int)inputManager.attackMouseButton) && parentCheckScript.IsChildOfFirstSlot())
        {
            StartHolding();
        }
        else
        {
            StopHolding();
        }

        if (isHolding)
        {
            DetectAndReportHits();
        }

        // Clear the set of enemies hit this frame after processing
        enemiesHitThisFrame.Clear();
    }

    void StartHolding()
    {
        if (!isHolding)
        {
            isHolding = true;
            enemiesHitDuringHold.Clear(); // Wyczy�� zestaw trafionych przeciwnik�w na pocz�tku trzymania
            if (holdingEffect != null)
            {
                holdingEffect.enabled = true;
            }
        }
    }

    void StopHolding()
    {
        isHolding = false;
        if (holdingEffect != null)
        {
            holdingEffect.enabled = false;
        }
    }

    public void ReportHit(GameObject enemy)
    {
        if (isHolding && !enemiesHitDuringHold.Contains(enemy)) // Sprawd�, czy przeciwnik nie by� ju� trafiony
        {
            enemiesHitDuringHold.Add(enemy); // Dodaj przeciwnika do zestawu trafionych podczas trzymania
            GetComponentInParent<WeaponAttack>().DealDamage(enemy, attackDamage);
            Debug.Log("Damage dealt");
        }
    }

    private void DetectAndReportHits()
    {
        RaycastHit[] hits = detection.Detect();
        foreach (RaycastHit hit in hits)
        {
            GameObject enemy = hit.collider.gameObject;
            UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();
            if (enemyHealth != null)
            {
                // Je�li przeciwnik nie by� ju� trafiony w bie��cej ramce
                if (!enemiesHitThisFrame.Contains(enemy))
                {
                    ReportHit(enemy);
                    enemiesHitThisFrame.Add(enemy); // Dodaj przeciwnika do zestawu trafionych w bie��cej ramce
                }
            }
        }

        // Usu� przeciwnik�w, kt�rzy nie zostali trafieni w tej ramce z zestawu trafionych podczas trzymania
        enemiesHitDuringHold.RemoveWhere(enemy => !enemiesHitThisFrame.Contains(enemy));
    }
}