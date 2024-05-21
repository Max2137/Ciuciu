using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            // Sprawdzanie czy naci�ni�to klawisz Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Naci�ni�to klawisz Q w strefie 1stSlot!");
            }

            // Sprawdzanie czy naci�ni�to klawisz E
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Naci�ni�to klawisz E w strefie 1stSlot!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tutaj dodaj kod, kt�ry zostanie wykonany, gdy obiekt o tagu "1stSlot" wejdzie w kolizj� z tym obiektem
            Debug.Log("Obiekt o tagu '1stSlot' zosta� wykryty! "+ other.name);
            isActive = true;
        }
    }
    private void OnTrigger(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tutaj dodaj kod, kt�ry zostanie wykonany, gdy obiekt o tagu "1stSlot" wejdzie w kolizj� z tym obiektem
            Debug.Log("Obiekt o tagu '1stSlot' zosta� odkryty!");
            isActive = false;
        }
    }
}