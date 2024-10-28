using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Serializable]
    public class Element
    {
        public string ObjectName;   // Nazwa obiektu na scenie
        public string ScriptName;   // Nazwa skryptu na obiekcie
        public string StatName;     // Nazwa statystyki w skrypcie
        public float StatValue;     // Wartoœæ, któr¹ chcemy ustawiæ dla statystyki
        public float StatMin;       // Minimalna wartoœæ dla statystyki
        public float StatMax;       // Maksymalna wartoœæ dla statystyki
        public Slider StatSlider;   // Referencja do suwaka w UI
    }

    public List<Element> elements = new List<Element>();  // Lista elementów

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ustawienia pocz¹tkowe dla suwaków
        foreach (var element in elements)
        {
            if (element.StatSlider != null)
            {
                element.StatSlider.minValue = element.StatMin;
                element.StatSlider.maxValue = element.StatMax;
                element.StatSlider.value = element.StatValue;
                element.StatSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(element, value));
            }
        }
    }

    private void Update()
    {
        foreach (var element in elements)
        {
            UpdateStatValue(element);
        }
    }

    private void UpdateStatValue(Element element)
    {
        GameObject targetObject = GameObject.Find(element.ObjectName);
        if (targetObject == null)
        {
            Debug.LogWarning($"Object '{element.ObjectName}' not found in the scene.");
            return;
        }

        Type scriptType = Type.GetType(element.ScriptName);
        if (scriptType == null)
        {
            Debug.LogWarning($"Script '{element.ScriptName}' not found. Ensure the script name is fully qualified.");
            return;
        }

        Component targetScript = targetObject.GetComponent(scriptType);
        if (targetScript == null)
        {
            Debug.LogWarning($"Script '{element.ScriptName}' not found on object '{element.ObjectName}'.");
            return;
        }

        // ZnajdŸ i ustaw wartoœæ statystyki
        var field = scriptType.GetField(element.StatName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        if (field == null)
        {
            Debug.LogWarning($"Stat '{element.StatName}' not found in script '{element.ScriptName}' on object '{element.ObjectName}'.");
            return;
        }

        // Ustaw wartoœæ w skrypcie na podstawie wartoœci z suwaka
        if (field.FieldType == typeof(float))
        {
            field.SetValue(targetScript, element.StatValue);
        }
        else if (field.FieldType == typeof(int))
        {
            field.SetValue(targetScript, (int)element.StatValue);
        }
        else
        {
            Debug.LogWarning($"Stat '{element.StatName}' in script '{element.ScriptName}' must be of type float or int.");
        }
    }

    private void OnSliderValueChanged(Element element, float value)
    {
        element.StatValue = value;  // Zaktualizuj wartoœæ statystyki na podstawie suwaka
    }
}