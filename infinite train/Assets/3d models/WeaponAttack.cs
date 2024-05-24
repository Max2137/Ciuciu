using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public bool isBurning;
    public string burningScript;

    public bool isDizzing;
    public string dizzyScript;

    // Lista boole'ów do zwrócenia
    private List<bool> boolList = new List<bool>();

    public void Start()
    {
        burningScript = "EffectBurningScript";
        dizzyScript = "EffectDizzingScript";
    }

    // Metoda zwracaj¹ca listê boole'ów
    public List<bool> GetBoolList()
    {
        boolList.Clear(); // Wyczyœæ listê przed ka¿dym u¿yciem

        // Dodaj wszystkie boole do listy
        boolList.Add(isBurning);
        boolList.Add(isDizzing); // Added the missing bool

        return boolList;
    }

    public void DealDamage(GameObject enemy, float attackDamage)
    {
        // SprawdŸ czy obiekt ma skrypt UniversalHealth
        UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();

        if (isBurning)
        {
            ApplyStatusEffect(enemy, burningScript);
        }
        if (isDizzing)
        {
            ApplyStatusEffect(enemy, dizzyScript);
        }

        if (enemyHealth != null)
        {
            // Zadaj obra¿enia obiektowi
            enemyHealth.TakeDamage(attackDamage, gameObject, EDamageType.MELEE);
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

        // SprawdŸ, czy obiekt ma ju¿ ten komponent
        var existingComponent = enemy.GetComponent(scriptType);

        // Jeœli komponent istnieje, usuñ go
        if (existingComponent != null)
        {
            Destroy(existingComponent);
        }

        // Dodaj komponent ponownie
        enemy.AddComponent(scriptType);
    }
}