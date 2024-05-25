using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjScript : MonoBehaviour
{
    // Typ skryptu, kt�ry chcemy doda� do wykrywanego obiektu
    public string scriptToAdd;

    // Tagi obiekt�w, kt�re nie b�d� podlega� dodawaniu skryptu
    public List<string> excludedTags = new List<string>();

    // Okre�la, czy obiekty z wykluczonymi tagami s� "bezpieczne"
    public bool isSafe = true;

    // Typ obra�e�, kt�ry chcemy przypisa� do dodawanego skryptu
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
                // Je�li obiekty z tagiem wykluczenia s� oznaczone jako "bezpieczne", nie dodajemy skryptu
                if (isSafe)
                {
                    return;
                }
            }
        }

        // Sprawdzamy, czy obiekt nie ma ju� tego skryptu
        if (other.gameObject.GetComponent(System.Type.GetType(scriptToAdd)) == null)
        {
            other.gameObject.AddComponent(System.Type.GetType(scriptToAdd));
            EffectBurningScript burningScript = other.gameObject.GetComponent<EffectBurningScript>();
            burningScript.damageType = damageType; // lub EDamageType.MAGIC w zale�no�ci od potrzeb

        }
    }
}