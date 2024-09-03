using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StatModifier
{
    public string scriptName;
    public float statValue;

    public void HalveStat()
    {
        statValue *= 0.5f;
    }
}

public class DestructionEffectMultiplication : MonoBehaviour
{
    public int copiesNumber = 2;
    public int generationAmount = 3;
    public List<StatModifier> statModifiers = new List<StatModifier>();

    private int currentGeneration = 0;

    void Start()
    {
        // Inicjalizacja, jeœli potrzebna
    }

    void OnDestroy()
    {
        if (currentGeneration < generationAmount)
        {
            for (int i = 0; i < copiesNumber; i++)
            {
                SpawnCopy();
            }
        }
    }

    void SpawnCopy()
    {
        GameObject copy = Instantiate(gameObject, transform.position, transform.rotation);
        DestructionEffectMultiplication copyScript = copy.GetComponent<DestructionEffectMultiplication>();

        // Zmniejszamy statystyki kopii
        for (int i = 0; i < copyScript.statModifiers.Count; i++)
        {
            copyScript.statModifiers[i].HalveStat();
        }

        // Zmniejszamy rozmiar kopii
        copy.transform.localScale = transform.localScale * 0.5f;

        // Zwiêkszamy licznik generacji
        copyScript.currentGeneration = currentGeneration + 1;
    }
}