using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeInfoScript : MonoBehaviour
{
    public UpgradeScript upgradeScript; // Referencja do skryptu UpgradeScript
    public TextMeshProUGUI costText; // TextMeshProUGUI, gdzie wyœwietlimy koszt
    public TextMeshProUGUI descriptionText; // TextMeshProUGUI, gdzie wyœwietlimy opis
    public RawImage uiRawImage;

    private void Start()
    {
        // Sprawdzamy, czy skrypt UpgradeScript jest przypisany
        if (upgradeScript == null)
        {
            Debug.LogError("Nie przypisano skryptu UpgradeScript!");
            return;
        }

        // Wy³¹czamy tekst na start
        costText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        uiRawImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Sprawdzamy, czy gracz jest wykrywany przez skrypt UpgradeScript
        if (upgradeScript.isActive)
        {
            var selectedUpgrade = upgradeScript.GetSelectedUpgrade();
            if (selectedUpgrade != null)
            {
                // Wyœwietlamy koszt ulepszenia
                string costString = "Koszt ulepszenia: ";
                foreach (var requirement in selectedUpgrade.itemRequirements)
                {
                    costString += $"{requirement.itemName}: {requirement.amount}, ";
                }
                costText.text = costString.TrimEnd(' ', ',');

                // Wyœwietlamy opis
                descriptionText.text = selectedUpgrade.description;

                // W³¹czamy tekst
                costText.gameObject.SetActive(true);
                descriptionText.gameObject.SetActive(true);
                uiRawImage.gameObject.SetActive(true);
            }
        }
        else
        {
            // Wy³¹czamy tekst, gdy gracz nie jest wykrywany
            costText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            uiRawImage.gameObject.SetActive(false);
        }
    }
}