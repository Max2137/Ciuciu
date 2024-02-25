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
        // SprawdŸ czy klawisz "Tab" zosta³ naciœniêty
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Prze³¹cz stan dla wszystkich dzieci obiektu
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(!areChildrenActive);
            }

            // Zmieñ stan dla nastêpnego klikniêcia
            areChildrenActive = !areChildrenActive;
        }
    }
}
