using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int attackDamage = 10;

    private CooldownScript cooldownScript;
    private WeaponAudioVisual weaponAudioVisual;
    private ParentCheckScript parentCheckScript;
    private WeaponInputManager inputManager;
    private WeaponDetection weaponDetection; // Dodajemy referencjê do WeaponDetection

    void Start()
    {
        cooldownScript = GetComponent<CooldownScript>();
        weaponAudioVisual = GetComponentInChildren<WeaponAudioVisual>();
        parentCheckScript = GetComponent<ParentCheckScript>();
        inputManager = GetComponent<WeaponInputManager>();
        weaponDetection = GetComponentInChildren<WeaponDetection>(); // Pobieramy referencjê do WeaponDetection

        if (cooldownScript == null)
        {
            Debug.LogError("CooldownScript not found on the object.");
        }
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
        if (weaponDetection == null)
        {
            Debug.LogError("WeaponDetection not found on the object.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)inputManager.attackMouseButton) && cooldownScript.CanSpawn() && parentCheckScript.IsChildOfFirstSlot())
        {
            Attack();
        }
    }

    void Attack()
    {
        weaponAudioVisual.PlayAttackAnimation();
        weaponAudioVisual.PlayAttackSound();

        RaycastHit[] hits = weaponDetection.Detect(); // U¿ywamy metody FanDetect z WeaponDetection

        foreach (RaycastHit hit in hits)
        {
            UniversalHealth enemyHealth = hit.collider.gameObject.GetComponent<UniversalHealth>();

            if (enemyHealth != null)
            {
                // U¿ywamy metody DealDamage z WeaponAttack do zadawania obra¿eñ
                GetComponent<WeaponAttack>().DealDamage(hit.collider.gameObject, attackDamage);
            }
        }

        cooldownScript.ResetCooldown();
    }
}