using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public bool isBurning;
    public string burningScript;

    // Lista boole'ów do zwrócenia
    private List<bool> boolList = new List<bool>();

    // Metoda zwracaj¹ca listê boole'ów
    public List<bool> GetBoolList()
    {
        boolList.Clear(); // Wyczyœæ listê przed ka¿dym u¿yciem

        // Dodaj wszystkie boole do listy
        boolList.Add(isBurning); // Dodaj ka¿dy inny bool w taki sam sposób

        return boolList;
    }

    public void DealDamage(GameObject enemy, float attackDamage)
    {
        // SprawdŸ czy obiekt ma skrypt UniversalHealth
        UniversalHealth enemyHealth = enemy.GetComponent<UniversalHealth>();

        if (isBurning)
        {
            System.Type burningScriptType = System.Type.GetType(burningScript);

            // SprawdŸ, czy obiekt ma ju¿ ten komponent
            var existingComponent = enemy.GetComponent(burningScriptType);

            // Jeœli komponent istnieje, usuñ go
            if (existingComponent != null)
            {
                Destroy(existingComponent);
            }

            // Dodaj komponent ponownie
            enemy.AddComponent(burningScriptType);
        }

        //Debug.Log("Wywo³ano atak");

        if (enemyHealth != null)
        {
            // Zadaj obra¿enia obiektowi
            enemyHealth.TakeDamage(attackDamage, gameObject);
        }
    }
}