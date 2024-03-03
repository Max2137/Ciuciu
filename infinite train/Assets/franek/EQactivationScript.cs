using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class EQactivationScript : MonoBehaviour
{
    private bool areChildrenActive;

    private void Start()
    {
        areChildrenActive = false;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Sprawd� czy klawisz "Tab" zosta� naci�ni�ty
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Prze��cz stan dla wszystkich dzieci obiektu
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!areChildrenActive);
            }

            // Zmie� stan dla nast�pnego klikni�cia
            areChildrenActive = !areChildrenActive;
        }
    }
}
