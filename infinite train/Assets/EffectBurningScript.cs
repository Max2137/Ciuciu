using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EffectBurningScript : MonoBehaviour
{
    public float damagePerTick = 10f; // Ilo�� obra�e� na tick
    public float tickCooldown = 1f; // Czas pomi�dzy tickami w sekundach
    private UniversalHealth targetHealth;

    private void Start()
    {
        // Znajd� komponent UniversalHealth na tym obiekcie
        targetHealth = FindObjectOfType<UniversalHealth>();

        // Sprawd�, czy targetHealth zosta� znaleziony
        if (targetHealth != null)
        {
            // Zadaj obra�enia
            targetHealth.TakeDamage(damagePerTick, gameObject);

            // Rozpocznij zadawanie obra�e�
            //StartCoroutine(ApplyBurningEffect());
        }
        else
        {
            Debug.LogError("Brak komponentu UniversalHealth na obiekcie " + gameObject.name);
        }
    }


    //private IEnumerator ApplyBurningEffect()
    //{
    //    // Repeat the burning effect
    //    while (true)
    //    {
    //        // Sprawd�, czy targetHealth nie jest nullem
    //        if (targetHealth != null)
    //        {

    //        }
    //        else
    //        {
    //            // Log an error and break the loop if targetHealth is null
    //            Debug.LogError("targetHealth is null during ApplyBurningEffect on " + gameObject.name);
    //            yield break; // Exit the coroutine
    //        }

    //        // Poczekaj na cooldown
    //        yield return new WaitForSeconds(tickCooldown);
    //    }
    //}

}