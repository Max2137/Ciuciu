using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbScript : MonoBehaviour
{
    public AudioClip healSound; // AudioClip, który chcesz odtworzyæ podczas uzdrawiania

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

                // Odtwórz dŸwiêk uzdrawiania
                PlayHealSound();

                // Usuñ ten obiekt (kulê leczenia)
                Destroy(gameObject);
            }
        }
    }

    // Metoda do odtwarzania dŸwiêku uzdrawiania
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