using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransactionScript : MonoBehaviour
{
    public enum RewardType
    {
        Item,
        Prefab
    }

    [System.Serializable]
    public class TransactionOffer
    {
        public List<ItemQuantity> costItems;
        public List<ItemReward> rewardItems;
    }

    [System.Serializable]
    public class ItemQuantity
    {
        public string itemName;
        public int quantity;
    }

    [System.Serializable]
    public class ItemReward
    {
        public RewardType rewardType;
        public string itemName;
        public int quantity;
        public GameObject rewardPrefab;
    }

    public List<TransactionOffer> transactionOffers;
    public TextMeshProUGUI transactionText;
    public Transform RewardSpawnPositioner;  // Dodane pole

    private bool isActive;
    private int selectedTransactionIndex;

    private itemsListScript itemsList;

    void Start()
    {
        isActive = false;
        selectedTransactionIndex = -1;
        UpdateTransactionText();

        // Znajdü i zainicjalizuj itemsListScript
        FindItemsListScript();
    }

    void Update()
    {
        if (isActive)
        {
            // Debugowanie klawisza do wyboru transakcji
            for (int i = 0; i < transactionOffers.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    selectedTransactionIndex = i; // Ustawia index transakcji na wybrany
                    Debug.Log($"Press key '{i + 1}' to execute transaction {i + 1}");
                    ExecuteTransaction(selectedTransactionIndex); // Wykonuje transakcjÍ od razu po jej wybraniu
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left!");
            isActive = false;
        }
    }

    private void UpdateTransactionText()
    {
        if (transactionText != null)
        {
            string text = "Transaction Offers:\n";
            for (int i = 0; i < transactionOffers.Count; i++)
            {
                text += $"{i + 1}. Cost: ";
                foreach (var costItem in transactionOffers[i].costItems)
                {
                    if (costItem.quantity > 0 && !string.IsNullOrEmpty(costItem.itemName))
                    {
                        text += $"{costItem.quantity}x {costItem.itemName}, ";
                    }
                }
                text += "Reward: ";
                foreach (var rewardItem in transactionOffers[i].rewardItems)
                {
                    if (rewardItem.rewardType == RewardType.Item)
                    {
                        if (rewardItem.quantity > 0 && !string.IsNullOrEmpty(rewardItem.itemName))
                        {
                            text += $"{rewardItem.quantity}x {rewardItem.itemName}, ";
                        }
                    }
                    else if (rewardItem.rewardType == RewardType.Prefab && rewardItem.rewardPrefab != null)
                    {
                        WeaponStatsData weaponStats = rewardItem.rewardPrefab.GetComponent<WeaponStatsData>();
                        if (weaponStats != null)
                        {
                            text += $"{weaponStats.Name}, ";
                        }
                        else
                        {
                            text += $"{rewardItem.rewardPrefab.name}, ";
                        }
                    }
                }
                text = text.TrimEnd(',', ' '); // UsuniÍcie zbÍdnego przecinka i spacji
                text += "\n";
            }
            transactionText.text = text;
            Debug.Log("Transaction text updated!");
        }
    }

    private void ExecuteTransaction(int index)
    {
        if (index >= 0 && index < transactionOffers.Count)
        {
            Debug.Log($"Executing transaction: {index + 1}");

            bool requirementsMet = true;

            // Sprawdü, czy gracz ma wystarczajπcπ iloúÊ przedmiotÛw na transakcjÍ
            foreach (var costItem in transactionOffers[index].costItems)
            {
                if (!HasItem(costItem.itemName, costItem.quantity))
                {
                    requirementsMet = false;
                    int missingQuantity = costItem.quantity - itemsList.GetQuantity(costItem.itemName);
                    Debug.Log($"Not enough {costItem.itemName} for transaction! Missing: {missingQuantity}");
                    break;
                }
            }

            if (requirementsMet)
            {
                // UsuÒ przedmioty kosztu i dodaj nagrody
                foreach (var costItem in transactionOffers[index].costItems)
                {
                    itemsList.Pay(costItem.itemName, costItem.quantity);
                }
                foreach (var rewardItem in transactionOffers[index].rewardItems)
                {
                    if (rewardItem.rewardType == RewardType.Item)
                    {
                        itemsList.Gain(rewardItem.itemName, rewardItem.quantity);
                    }
                    else if (rewardItem.rewardType == RewardType.Prefab && rewardItem.rewardPrefab != null)
                    {
                        Instantiate(rewardItem.rewardPrefab, RewardSpawnPositioner.position, RewardSpawnPositioner.rotation);
                        Debug.Log($"Spawned reward prefab: {rewardItem.rewardPrefab.name}");
                    }
                }

                Debug.Log("Transaction successful!");
            }
            else
            {
                Debug.Log("Transaction requirements not met!");
            }
        }
        else
        {
            Debug.Log("Invalid transaction index!");
        }
    }

    private bool HasItem(string itemName, int quantity)
    {
        // Sprawdü, czy gracz ma wystarczajπcπ iloúÊ przedmiotÛw
        return itemsList.GetQuantity(itemName) >= quantity;
    }

    private void FindItemsListScript()
    {
        // Znajdü itemsListScript w za≥adowanych scenach
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
                        Debug.Log("itemsListScript found!");
                        return;
                    }
                }
            }
        }

        Debug.LogError("itemsListScript not found in any loaded scene.");
    }
}