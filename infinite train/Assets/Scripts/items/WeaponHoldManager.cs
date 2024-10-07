using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHoldManager : MonoBehaviour
{
    public float attackDamage = 10;
    public List<string> holdingEffectScriptsNames; // Zmodyfikowano HoldingEffectScript na listê nazw skryptów

    private WeaponAudioVisual weaponAudioVisual;
    private ParentCheckScript parentCheckScript;
    private WeaponInputManager inputManager;
    private WeaponDetection detection;
    private List<MonoBehaviour> holdingEffects = new List<MonoBehaviour>(); // Zmieniono HoldingEffect na listê skryptów

    private bool isHolding = false;
    private HashSet<GameObject> enemiesHitThisFrame = new HashSet<GameObject>();
    private HashSet<GameObject> enemiesHitDuringHold = new HashSet<GameObject>();

    public bool IsHolding()
    {
        return isHolding;
    }

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

        // Dodawanie wszystkich skryptów HoldingEffect do listy
        foreach (var scriptName in holdingEffectScriptsNames)
        {
            MonoBehaviour effect = GetComponent(scriptName) as MonoBehaviour;
            if (effect == null)
            {
                Debug.LogError($"HoldingEffect script {scriptName} not found on the object.");
            }
            else
            {
                holdingEffects.Add(effect);
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

        enemiesHitThisFrame.Clear();
    }

    void StartHolding()
    {
        if (!isHolding)
        {
            isHolding = true;
            enemiesHitDuringHold.Clear();
            // W³¹cz wszystkie efekty trzymania z listy
            foreach (var effect in holdingEffects)
            {
                effect.enabled = true;
            }
        }
    }

    void StopHolding()
    {
        isHolding = false;
        // Wy³¹cz wszystkie efekty trzymania z listy
        foreach (var effect in holdingEffects)
        {
            effect.enabled = false;
        }
    }

    public void ReportHit(GameObject enemy)
    {
        if (isHolding && !enemiesHitDuringHold.Contains(enemy) && attackDamage > 0)
        {
            enemiesHitDuringHold.Add(enemy);
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
                if (!enemiesHitThisFrame.Contains(enemy))
                {
                    ReportHit(enemy);
                    enemiesHitThisFrame.Add(enemy);
                }
            }
        }

        enemiesHitDuringHold.RemoveWhere(enemy => !enemiesHitThisFrame.Contains(enemy));
    }
}