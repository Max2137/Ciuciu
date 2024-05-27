using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public bool isBurning;
    public string burningScript;

    public bool isDizzing;
    public string dizzyScript;

    public bool isBloodPoisoning;
    public string bloodPoisoningScript;

    public bool isElectrifying;
    public string electrifyingObjectName; // Nazwa prefabrykatu elektryfikacji

    public EDamageType damageType;

    // Lista boole'�w do zwr�cenia
    private List<bool> boolList = new List<bool>();

    public void Start()
    {
        burningScript = "EffectBurningScript";
        dizzyScript = "EffectDizzingScript";
        bloodPoisoningScript = "EffectBloodPoisoning";
        electrifyingObjectName = "ElectrifyingObj";
    }

    // Metoda zwracaj�ca list� boole'�w
    public List<bool> GetBoolList()
    {
        boolList.Clear(); // Wyczy�� list� przed ka�dym u�yciem

        // Dodaj wszystkie boole do listy
        boolList.Add(isBurning);
        boolList.Add(isDizzing);
        boolList.Add(isBloodPoisoning);
        boolList.Add(isElectrifying);

        return boolList;
    }

    public void DealDamage(GameObject enemy, float attackDamage)
    {
        // Sprawd� czy obiekt ma skrypt UniversalHealth
        UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();

        if (isBurning)
        {
            ApplyStatusEffect(enemy, burningScript);
        }
        if (isDizzing)
        {
            ApplyStatusEffect(enemy, dizzyScript);
        }
        if (isBloodPoisoning)
        {
            ApplyStatusEffect(enemy, bloodPoisoningScript);
        }
        if (isElectrifying)
        {
            // Znajd� prefab na scenie po jego nazwie
            GameObject electrifyingObject = GameObject.Find(electrifyingObjectName);
            if (electrifyingObject != null)
            {
                // Zespawnuj prefabrykat
                GameObject spawnedElectrifyingObject = Instantiate(electrifyingObject, enemy.transform.position, Quaternion.identity);
                // Aktywuj zespawnowane komponenty
                ActivateComponents(spawnedElectrifyingObject);
            }
            else
            {
                Debug.LogError("Prefab not found on scene: " + electrifyingObjectName);
            }
        }

        if (enemyHealth != null)
        {
            // Zadaj obra�enia obiektowi
            enemyHealth.TakeDamage(attackDamage, gameObject, damageType);
        }
    }

    private void ApplyStatusEffect(GameObject enemy, string scriptName)
    {
        System.Type scriptType = System.Type.GetType(scriptName);
        if (scriptType == null)
        {
            Debug.LogError($"Script type {scriptName} not found");
            return;
        }

        // Sprawd�, czy obiekt ma ju� ten komponent
        var existingComponent = enemy.GetComponent(scriptType);

        // Je�li komponent istnieje, usu� go
        if (existingComponent == null)
        {
            // Dodaj komponent ponownie
            enemy.AddComponent(scriptType);
        }
    }

    private void ActivateComponents(GameObject obj)
    {
        // Pobierz wszystkie komponenty z obiektu
        Component[] components = obj.GetComponentsInChildren<Component>();

        // Iteruj przez komponenty i aktywuj je, je�li s� nieaktywne
        foreach (var component in components)
        {
            if (component is MonoBehaviour)
            {
                MonoBehaviour monoBehaviour = component as MonoBehaviour;
                if (!monoBehaviour.isActiveAndEnabled)
                {
                    monoBehaviour.enabled = true;
                }
            }
        }
    }
}