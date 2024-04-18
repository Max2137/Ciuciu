using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public UniversalHealth universalHealth;
    public UnityEngine.UI.Image healthBar; // Jednoznaczne okreœlenie Image jako UnityEngine.UI.Image

    public AudioSource audioSource;
    public AudioClip HeartBeat1;
    public AudioClip HeartBeat2;

    private bool isPlayingHeartBeat1 = false;
    private bool isPlayingHeartBeat2 = false;

    // Start is called before the first frame update
    void Start()
    {
        if (universalHealth == null)
        {
            Debug.LogError("UniversalHealth reference not set in PlayerHealthBar!");
            return;
        }

        if (healthBar == null)
        {
            Debug.LogError("HealthBar reference not set in PlayerHealthBar!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource reference not set in PlayerHealthBar!");
            return;
        }

        StartCoroutine(UpdateHealthBar());
    }

    IEnumerator UpdateHealthBar()
    {
        while (true)
        {
            float fillAmount = universalHealth.currentHealth / universalHealth.maxHealth;
            healthBar.fillAmount = fillAmount;

            // Sprawdzanie warunków dla odtwarzania dŸwiêków
            if (fillAmount < 0.5f && !isPlayingHeartBeat1)
            {
                isPlayingHeartBeat1 = true;
                audioSource.loop = true;
                audioSource.clip = HeartBeat1;
                audioSource.Play();
            }
            else if (fillAmount >= 0.5f && isPlayingHeartBeat1)
            {
                isPlayingHeartBeat1 = false;
                audioSource.loop = false;
                audioSource.Stop();
            }

            if (fillAmount < 0.2f && !isPlayingHeartBeat2)
            {
                isPlayingHeartBeat2 = true;
                audioSource.loop = true;
                audioSource.clip = HeartBeat2;
                audioSource.Play();
            }
            else if (fillAmount >= 0.2f && isPlayingHeartBeat2)
            {
                isPlayingHeartBeat2 = false;
                audioSource.loop = false;
                audioSource.Stop();
            }

            yield return null;
        }
    }
}