using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjScript : MonoBehaviour
{
    // Typ skryptu, który chcemy dodaæ do wykrywanego obiektu
    public string scriptToAdd;

    // Tagi obiektów, które nie bêd¹ podlegaæ dodawaniu skryptu
    public List<string> excludedTags = new List<string>();

    // Okreœla, czy obiekty z wykluczonymi tagami s¹ "bezpieczne"
    public bool isSafe = true;

    // Typ obra¿eñ, który chcemy przypisaæ do dodawanego skryptu
    public EDamageType damageType;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy obiekt ma wykluczony tag
        foreach (string excludedTag in excludedTags)
        {
            if (other.gameObject.CompareTag(excludedTag))
            {
                // Jeœli obiekty z tagiem wykluczenia s¹ oznaczone jako "bezpieczne", nie dodajemy skryptu
                if (isSafe)
                {
                    return;
                }
            }
        }

        // Sprawdzamy, czy obiekt nie ma ju¿ tego skryptu
        if (other.gameObject.GetComponent(System.Type.GetType(scriptToAdd)) == null)
        {
            other.gameObject.AddComponent(System.Type.GetType(scriptToAdd));
            EffectBurningScript burningScript = other.gameObject.GetComponent<EffectBurningScript>();
            burningScript.damageType = damageType; // lub EDamageType.MAGIC w zale¿noœci od potrzeb

        }
    }
}