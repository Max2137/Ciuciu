using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[System.Serializable]
public class BasicStat
{
    public string BasicStatName;
    public string BasicStatDisplay;
}

public class WeaponStatsData : MonoBehaviour
{
    public string Name;
    public string Description;
    public List<BasicStat> BasicStats;

    // Metoda do uzyskiwania informacji na podstawie BasicStatName
    public Dictionary<string, object> GetStats()
    {
        Dictionary<string, object> stats = new Dictionary<string, object>();

        // Pobierz wszystkie skrypty na obiekcie
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (BasicStat basicStat in BasicStats)
        {
            foreach (MonoBehaviour script in scripts)
            {
                Type type = script.GetType();

                // Uzyskaj dost�p do pola, uwzgl�dniaj�c prywatne pola
                FieldInfo field = type.GetField(basicStat.BasicStatName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (field != null)
                {
                    // Dodaj do s�ownika (nazwa, warto��)
                    stats.Add(basicStat.BasicStatDisplay, field.GetValue(script));
                    break; // Przerwij p�tl�, gdy znajdziesz pierwszy pasuj�cy skrypt
                }
            }
        }

        return stats;
    }
}