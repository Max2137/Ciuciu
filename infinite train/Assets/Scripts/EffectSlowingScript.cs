using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class EffectSlowingScript : MonoBehaviour
{
    public float slowFactor = 0.5f; // Wspó³czynnik spowolnienia, np. 0.5 dla spowolnienia o po³owê

    private Dictionary<MonoBehaviour, float> originalSpeeds = new Dictionary<MonoBehaviour, float>();

    void OnEnable()
    {
        // Spowolnienie prêdkoœci w innych skryptach
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            if (script != this && script.enabled)
            {
                FieldInfo speedField = script.GetType().GetField("Speed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (speedField != null && speedField.FieldType == typeof(float))
                {
                    float currentSpeed = (float)speedField.GetValue(script);
                    originalSpeeds[script] = currentSpeed; // Zapisz oryginaln¹ wartoœæ
                    speedField.SetValue(script, currentSpeed * slowFactor);
                }

                PropertyInfo speedProperty = script.GetType().GetProperty("Speed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (speedProperty != null && speedProperty.PropertyType == typeof(float) && speedProperty.CanWrite)
                {
                    float currentSpeed = (float)speedProperty.GetValue(script);
                    originalSpeeds[script] = currentSpeed; // Zapisz oryginaln¹ wartoœæ
                    speedProperty.SetValue(script, currentSpeed * slowFactor);
                }
            }
        }
    }

    void OnDisable()
    {
        // Przywrócenie oryginalnej prêdkoœci w innych skryptach
        foreach (var entry in originalSpeeds)
        {
            MonoBehaviour script = entry.Key;
            float originalSpeed = entry.Value;

            FieldInfo speedField = script.GetType().GetField("Speed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (speedField != null && speedField.FieldType == typeof(float))
            {
                speedField.SetValue(script, originalSpeed);
            }

            PropertyInfo speedProperty = script.GetType().GetProperty("Speed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (speedProperty != null && speedProperty.PropertyType == typeof(float) && speedProperty.CanWrite)
            {
                speedProperty.SetValue(script, originalSpeed);
            }
        }
        originalSpeeds.Clear();
    }
}