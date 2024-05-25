using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UpgradeScript : MonoBehaviour
{
    public bool isActive;
    private GameObject detectedObject;
    public ParticleSystem upgradeEffect;

    public bool isExclusive;
    private bool wasUsed;

    // Nazwa zmiennej bool pobrana z Inspectora
    public string boolVariableName;

    [System.Serializable]
    public class ItemRequirement
    {
        public string itemName;
        public int amount;
    }

    // Lista wymaganych przedmiotów do ulepszenia
    public List<ItemRequirement> itemRequirements;

    private itemsListScript itemsList;

    // Inspector fields for TextMeshPro and colors
    public TextMeshProUGUI upgradeText;
    public Color colorNormal;
    public Color colorUsed;

    // Start is called before the first frame update
    void Start()
    {
        upgradeEffect.Stop();
        isActive = false;
        FindItemsListScript();

        // Set the initial color of the upgradeText to colorNormal with alpha value 1
        if (upgradeText != null)
        {
            colorNormal.a = 1f;
            upgradeText.color = colorNormal;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HandleKeyPress("Hand2");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                HandleKeyPress("Hand1");
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                HandleKeyPress("Boots Slot");
            }
        }
    }

    private void HandleKeyPress(string handName)
    {
        Debug.Log($"Naciœniêto klawisz w strefie 1stSlot, sprawdzanie obiektu {handName}!");

        if (detectedObject != null)
        {
            Transform handTransform = detectedObject.transform.Find(handName);
            if (handTransform != null)
            {
                Debug.Log($"Znaleziono obiekt '{handName}' jako childa wykrytego obiektu: {handTransform.name}");

                if (handTransform.childCount >= 2)
                {
                    Transform secondChild = handTransform.GetChild(1); // Indeks 1 oznacza drugiego childa
                    Debug.Log($"Nazwa drugiego childa obiektu '{handName}': {secondChild.name}");

                    CheckForBoolVariable(secondChild.gameObject);
                }
                else
                {
                    Debug.Log($"Obiekt '{handName}' nie ma dwóch childów.");
                }
            }
            else
            {
                Debug.Log($"Nie znaleziono obiektu '{handName}' jako childa wykrytego obiektu.");
            }
        }
    }

    private void CheckForBoolVariable(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            System.Reflection.FieldInfo field = script.GetType().GetField(boolVariableName);
            if (field != null && field.FieldType == typeof(bool))
            {
                bool isAlreadyUpgraded = (bool)field.GetValue(script);
                if (!isAlreadyUpgraded)
                {
                    if (AreRequirementsMet())
                    {
                        if (isExclusive == false || wasUsed == false)
                        {
                            if (isExclusive == true)
                            {
                                wasUsed = true;
                                // Change text color to colorUsed with alpha value 1
                                if (upgradeText != null)
                                {
                                    colorUsed.a = 1f;
                                    upgradeText.color = colorUsed;
                                }
                            }

                            foreach (var requirement in itemRequirements)
                            {
                                itemsList.Pay(requirement.itemName, requirement.amount);
                            }
                            field.SetValue(script, true);
                            Debug.Log($"Zmienna bool '{boolVariableName}' zosta³a ustawiona na true w skrypcie: {script.GetType().Name}");

                            if (!upgradeEffect.isPlaying)
                            {
                                upgradeEffect.Play();
                            }
                            else
                            {
                                upgradeEffect.Stop();
                                upgradeEffect.Play();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Nie masz wystarczaj¹cej iloœci wymaganych przedmiotów do ulepszenia broni.");
                    }
                }
                else
                {
                    Debug.Log("Broñ jest ju¿ ulepszona.");
                }
                return;
            }
        }
        Debug.Log($"Nie znaleziono zmiennej bool o nazwie '{boolVariableName}' w skryptach obiektu {obj.name}.");
    }

    private bool AreRequirementsMet()
    {
        foreach (var requirement in itemRequirements)
        {
            if (itemsList.GetQuantity(requirement.itemName) < requirement.amount)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Obiekt o tagu '1stSlot' zosta³ wykryty! " + other.name);
            isActive = true;
            detectedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Obiekt o tagu '1stSlot' zosta³ odkryty!");
            isActive = false;
            detectedObject = null;
        }
    }

    private void FindItemsListScript()
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                foreach (GameObject go in scene.GetRootGameObjects())
                {
                    itemsList = go.GetComponentInChildren<itemsListScript>();
                    if (itemsList != null)
                    {
                        Debug.Log("ItemsListScript znaleziony!");
                        return;
                    }
                }
            }
        }

        Debug.LogError("Nie znaleziono skryptu ItemsListScript w ¿adnej za³adowanej scenie.");
    }
}