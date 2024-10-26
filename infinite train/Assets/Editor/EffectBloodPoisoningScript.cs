using UnityEngine;
using UnityEditor;

public class EffectBloodPoisoning : MonoBehaviour
{
    public string effectPrefabName = "BloodPoisoningObj"; // Nazwa prefabu efektu úmierci
    private GameObject effectPrefab; // Prefab efektu úmierci

    void Start()
    {
        // Znajdü prefab w assetach
        effectPrefab = FindPrefab(effectPrefabName);
        if (effectPrefab == null)
        {
            Debug.LogError($"Prefab with name {effectPrefabName} not found in assets.");
        }
    }

    void OnDestroy()
    {
        Debug.Log("Zespawnowano");
        SpawnEffect();
    }

    void SpawnEffect()
    {
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }

    private GameObject FindPrefab(string name)
    {
        string[] guids = AssetDatabase.FindAssets(name);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (asset != null && asset.name == name)
            {
                return asset;
            }
        }
        return null;
    }
}