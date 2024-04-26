using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // SprawdŸ, czy obiekt ma tag "Player"
        if (other.CompareTag("Player"))
        {
            // ZnajdŸ komponent UniversalHealth na obiekcie gracza
            UniversalHealth playerHealth = other.GetComponent<UniversalHealth>();

            // Jeœli uda³o siê znaleŸæ komponent UniversalHealth
            if (playerHealth != null)
            {
                // Zadaj graczowi dodatkowe zdrowie
                playerHealth.Heal(5f);

                // Usuñ ten obiekt (kulê leczenia)
                Destroy(gameObject);
            }
        }
    }
}