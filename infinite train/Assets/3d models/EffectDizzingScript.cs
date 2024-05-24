using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDizzingScript : MonoBehaviour
{
    // Czas trwania efektu w sekundach
    public float effectTime = 5f;

    // Lista przechowuj�ca referencje do wy��czonych skrypt�w
    private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();

    // Lista nazw skrypt�w, kt�re nie maj� by� dezaktywowane
    public List<string> scriptsToKeepEnabled = new List<string>();

    // Referencja do komponentu Rigidbody
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (!scriptsToKeepEnabled.Contains("UniversalHealth"))
        {
            scriptsToKeepEnabled.Add("UniversalHealth");
        }

        // Znajd� komponent Rigidbody na obiekcie
        rb = GetComponent<Rigidbody>();

        // Je�li obiekt ma Rigidbody, ustaw jego pr�dko�� i moment obrotowy na 0
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Znajd� wszystkie skrypty na obiekcie
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            // Dezaktywuj skrypt, je�li nie jest to ten skrypt i nie jest na li�cie skrypt�w do pozostawienia aktywnymi
            if (script != this && !scriptsToKeepEnabled.Contains(script.GetType().Name))
            {
                script.enabled = false;
                disabledScripts.Add(script);
            }
        }

        // Uruchom korutyn� przywracaj�c� skrypty po okre�lonym czasie
        StartCoroutine(RestoreScriptsAfterTime(effectTime));
    }

    private IEnumerator RestoreScriptsAfterTime(float time)
    {
        // Czekaj przez podany czas
        yield return new WaitForSeconds(time);

        // Przywr�� wszystkie wy��czone skrypty
        foreach (MonoBehaviour script in disabledScripts)
        {
            script.enabled = true;
        }

        // Usu� ten skrypt z obiektu
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}