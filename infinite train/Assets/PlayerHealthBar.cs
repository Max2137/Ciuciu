using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public UniversalHealth universalHealth;
    public Image healthBar;

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

        StartCoroutine(UpdateHealthBar());
    }

    IEnumerator UpdateHealthBar()
    {
        while (true)
        {
            float fillAmount = universalHealth.currentHealth / universalHealth.maxHealth;
            healthBar.fillAmount = fillAmount;

            yield return null;
        }
    }
}