using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbScript : MonoBehaviour
{
    public AudioClip healSound; // AudioClip, kt�ry chcesz odtworzy� podczas uzdrawiania

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

                // Odtw�rz d�wi�k uzdrawiania
                PlayHealSound();

                // Usu� ten obiekt (kul� leczenia)
                Destroy(gameObject);
            }
        }
    }

    // Metoda do odtwarzania d�wi�ku uzdrawiania
    private void PlayHealSound()
    {
        GameObject audioPlayer = GameObject.FindGameObjectWithTag("AudioPlayer");

        if (audioPlayer != null)
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();

            if (audioSource != null && healSound != null)
            {
                audioSource.PlayOneShot(healSound);
            }
            else
            {
                Debug.LogWarning("AudioSource component or healSound AudioClip is missing!");
            }
        }
        else
        {
            Debug.LogWarning("AudioPlayer GameObject with tag 'AudioPlayer' not found!");
        }
    }
}