using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHoldManager : MonoBehaviour
{
    public int attackDamage = 10;
    public string holdingEffectScriptName; // Nazwa skryptu efektu trzymania

    private WeaponAudioVisual weaponAudioVisual;
    private ParentCheckScript parentCheckScript;
    private WeaponInputManager inputManager;
    private WeaponDetectionCollider detectionCollider;
    private MonoBehaviour holdingEffect; // Zmienna do przechowywania komponentu skryptu

    private bool isHolding = false;

    void Start()
    {
        weaponAudioVisual = GetComponentInChildren<WeaponAudioVisual>();
        parentCheckScript = GetComponent<ParentCheckScript>();
        inputManager = GetComponent<WeaponInputManager>();
        detectionCollider = GetComponentInChildren<WeaponDetectionCollider>();

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
        if (detectionCollider == null)
        {
            Debug.LogError("WeaponDetectionCollider not found on the object.");
        }
        else
        {
            detectionCollider.holdManager = this;
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
    }

    void StartHolding()
    {
        if (!isHolding)
        {
            isHolding = true;
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
        if (isHolding)
        {
            GetComponentInParent<WeaponAttack>().DealDamage(enemy, attackDamage);
            Debug.Log("Damage dealt");
        }
    }
}