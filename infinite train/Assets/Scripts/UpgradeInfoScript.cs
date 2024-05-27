using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeInfoScript : MonoBehaviour
{
    public UpgradeScript upgradeScript; // Referencja do skryptu UpgradeScript
    public TextMeshProUGUI costText; // TextMeshProUGUI, gdzie wy�wietlimy koszt
    public TextMeshProUGUI descriptionText; // TextMeshProUGUI, gdzie wy�wietlimy opis
    public RawImage uiRawImage;

    private void Start()
    {
        // Sprawdzamy, czy skrypt UpgradeScript jest przypisany
        if (upgradeScript == null)
        {
            Debug.LogError("Nie przypisano skryptu UpgradeScript!");
            return;
        }

        // Wy��czamy tekst na start
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
                // Wy�wietlamy koszt ulepszenia
                string costString = "Koszt ulepszenia: ";
                foreach (var requirement in selectedUpgrade.itemRequirements)
                {
                    costString += $"{requirement.itemName}: {requirement.amount}, ";
                }
                costText.text = costString.TrimEnd(' ', ',');

                // Wy�wietlamy opis
                descriptionText.text = selectedUpgrade.description;

                // W��czamy tekst
                costText.gameObject.SetActive(true);
                descriptionText.gameObject.SetActive(true);
                uiRawImage.gameObject.SetActive(true);
            }
        }
        else
        {
            // Wy��czamy tekst, gdy gracz nie jest wykrywany
            costText.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            uiRawImage.gameObject.SetActive(false);
        }
    }
}