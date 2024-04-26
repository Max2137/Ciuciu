using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Sprawd�, czy obiekt ma tag "Player"
        if (other.CompareTag("Player"))
        {
            // Znajd� komponent UniversalHealth na obiekcie gracza
            UniversalHealth playerHealth = other.GetComponent<UniversalHealth>();

            // Je�li uda�o si� znale�� komponent UniversalHealth
            if (playerHealth != null)
            {
                // Zadaj graczowi dodatkowe zdrowie
                playerHealth.Heal(5f);

                // Usu� ten obiekt (kul� leczenia)
                Destroy(gameObject);
            }
        }
    }
}