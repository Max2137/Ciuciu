using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDizzingScript : MonoBehaviour
{
    // Czas trwania efektu w sekundach
    public float effectTime = 5f;

    // Lista przechowuj¹ca referencje do wy³¹czonych skryptów
    private List<MonoBehaviour> disabledScripts = new List<MonoBehaviour>();

    // Lista nazw skryptów, które nie maj¹ byæ dezaktywowane
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

        // ZnajdŸ komponent Rigidbody na obiekcie
        rb = GetComponent<Rigidbody>();

        // Jeœli obiekt ma Rigidbody, ustaw jego prêdkoœæ i moment obrotowy na 0
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // ZnajdŸ wszystkie skrypty na obiekcie
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            // Dezaktywuj skrypt, jeœli nie jest to ten skrypt i nie jest na liœcie skryptów do pozostawienia aktywnymi
            if (script != this && !scriptsToKeepEnabled.Contains(script.GetType().Name))
            {
                script.enabled = false;
                disabledScripts.Add(script);
            }
        }

        // Uruchom korutynê przywracaj¹c¹ skrypty po okreœlonym czasie
        StartCoroutine(RestoreScriptsAfterTime(effectTime));
    }

    private IEnumerator RestoreScriptsAfterTime(float time)
    {
        // Czekaj przez podany czas
        yield return new WaitForSeconds(time);

        // Przywróæ wszystkie wy³¹czone skrypty
        foreach (MonoBehaviour script in disabledScripts)
        {
            script.enabled = true;
        }

        // Usuñ ten skrypt z obiektu
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