using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BoolDisplayNamePair
{
    public string boolName;
    public string displayName;
}

public class WeaponStatsDisplayScript : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI NameSpace;
    public TextMeshProUGUI DescriptionSpace;
    public RawImage uiRawImage;
    public TextMeshProUGUI effects;
    public string itemTag = "Item";
    public float searchingDistance = 10f;
    public List<BoolDisplayNamePair> boolDisplayNamePairs = new List<BoolDisplayNamePair>();

    private WeaponStatsData currentWeaponStatsData;
    private WeaponAttack currentWeaponAttack;
    private Dictionary<string, string> boolDisplayNameMap = new Dictionary<string, string>();

    void Start()
    {
        InitializeBoolDisplayNameMap();
    }

    void Update()
    {
        SearchForNearestItem();
        UpdateUI();
        UpdateUIImage();
        UpdateEffectsText();
    }

    public void InitializeBoolDisplayNameMap()
    {
        boolDisplayNameMap.Clear();
        foreach (var pair in boolDisplayNamePairs)
        {
            boolDisplayNameMap[pair.boolName] = pair.displayName;
        }
    }

    void SearchForNearestItem()
    {
        GameObject nearestItem = null;
        float nearestDistance = float.MaxValue;

        Collider[] colliders = Physics.OverlapSphere(transform.position, searchingDistance);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(itemTag))
            {
                Transform itemTransform = collider.transform;
                bool isParentPlayer = false;

                while (itemTransform.parent != null)
                {
                    itemTransform = itemTransform.parent;
                }

                if (itemTransform.CompareTag("Player"))
                {
                    isParentPlayer = true;
                }

                if (!isParentPlayer)
                {
                    Vector3 itemCenter = collider.bounds.center;
                    float distance = Vector3.Distance(transform.position, itemCenter);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestItem = collider.gameObject;
                    }
                }
            }
        }

        if (nearestItem != null)
        {
            WeaponStatsData weaponStatsData = nearestItem.GetComponent<WeaponStatsData>();
            if (weaponStatsData != null)
            {
                currentWeaponStatsData = weaponStatsData;
            }

            WeaponAttack weaponAttack = nearestItem.GetComponent<WeaponAttack>();
            if (weaponAttack != null)
            {
                currentWeaponAttack = weaponAttack;
            }
        }
        else
        {
            currentWeaponStatsData = null;
            currentWeaponAttack = null;
        }
    }

    void UpdateUI()
    {
        if (displayText == null)
        {
            Debug.LogError("Nie przypisano obiektu TextMeshProUGUI.");
            return;
        }

        displayText.text = "";

        if (currentWeaponStatsData != null)
        {
            if (NameSpace != null)
            {
                NameSpace.text = currentWeaponStatsData.Name;
            }

            if (DescriptionSpace != null)
            {
                DescriptionSpace.text = currentWeaponStatsData.Description;
            }

            Dictionary<string, object> stats = currentWeaponStatsData.GetStats();

            foreach (var stat in stats)
            {
                displayText.text += stat.Key + ": " + stat.Value + "\n";
            }
        }
        else
        {
            if (NameSpace != null)
            {
                NameSpace.text = "";
            }

            if (DescriptionSpace != null)
            {
                DescriptionSpace.text = "";
            }
        }
    }

    void UpdateUIImage()
    {
        if (uiRawImage != null)
        {
            uiRawImage.gameObject.SetActive(currentWeaponStatsData != null);
        }
    }

    void UpdateEffectsText()
    {
        if (effects == null)
        {
            Debug.LogError("Nie przypisano obiektu TextMeshProUGUI dla efektów.");
            return;
        }

        effects.text = "";

        if (currentWeaponAttack != null)
        {
            List<string> trueEffects = new List<string>();

            foreach (var field in currentWeaponAttack.GetType().GetFields())
            {
                if (field.FieldType == typeof(bool))
                {
                    bool value = (bool)field.GetValue(currentWeaponAttack);
                    if (value)
                    {
                        // SprawdŸ, czy pole boola ma przypisany displayName
                        string displayName;
                        if (boolDisplayNameMap.TryGetValue(field.Name, out displayName))
                        {
                            trueEffects.Add(displayName);
                        }
                    }
                }
            }

            if (trueEffects.Count > 0)
            {
                effects.text = "Effects:\n";
                foreach (var effect in trueEffects)
                {
                    effects.text += "- " + effect + "\n";
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponStatsDisplayScript))]
public class WeaponStatsDisplayScriptEditor : Editor
{
    SerializedProperty boolDisplayNamePairs;

    private void OnEnable()
    {
        boolDisplayNamePairs = serializedObject.FindProperty("boolDisplayNamePairs");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        WeaponStatsDisplayScript weaponStatsDisplayScript = (WeaponStatsDisplayScript)target;

        EditorGUILayout.PropertyField(boolDisplayNamePairs, true);

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Refresh Bool -> DisplayName Map"))
        {
            weaponStatsDisplayScript.InitializeBoolDisplayNameMap();
        }
    }
}
#endif