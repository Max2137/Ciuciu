using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Typ skryptu, który chcemy dodaæ do wykrywanego obiektu
    public string scriptToAdd;

    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy obiekt nie ma ju¿ tego skryptu
        if (other.gameObject.GetComponent(scriptToAdd) == null)
        {
            // Dodajemy skrypt do wykrywanego obiektu
            other.gameObject.AddComponent(System.Type.GetType(scriptToAdd));
        }
    }
}