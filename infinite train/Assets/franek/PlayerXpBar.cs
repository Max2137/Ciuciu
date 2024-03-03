using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXpBar : MonoBehaviour
{
    public int initialExperience = 0;
    public int experienceToNextLevel = 100;
    public float levelMultiplier = 1.5f;

    private int experience;
    private int level = 1;

    public TMP_Text levelText;
    public TMP_Text experienceText;
    public UnityEngine.UI.Image experienceFillImage;

    public PlayerStatsScript playerStatsScript;  // Referencja do skryptu PlayerStatsScript

    public GameObject attackMeleePanel, attackMagicPanel, defenseGeneralPanel;

    void Start()
    {
        experience = initialExperience;
        UpdateUI();
        SetupUpgradeButtons();

        attackMeleePanel.SetActive(false);
        attackMagicPanel.SetActive(false);
        defenseGeneralPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //GainExperience(30);
        }
    }

    void SetupUpgradeButtons()
    {
        attackMeleePanel.SetActive(false);
        attackMagicPanel.SetActive(false);
        defenseGeneralPanel.SetActive(false);

        // Dodaj nasłuchiwacze kliknięć do przycisków
        attackMeleePanel.GetComponent<Button>().onClick.AddListener(UpgradeAttackMelee);
        attackMagicPanel.GetComponent<Button>().onClick.AddListener(UpgradeAttackMagic);
        defenseGeneralPanel.GetComponent<Button>().onClick.AddListener(UpgradeDefenseGeneral);
    }

    void EnableUpgradeButtons()
    {
        Time.timeScale = 0;

        attackMeleePanel.SetActive(true);
        attackMagicPanel.SetActive(true);
        defenseGeneralPanel.SetActive(true);
    }

    void DisableUpgradeButtons()
    {
        Time.timeScale = 1;

        attackMeleePanel.SetActive(false);
        attackMagicPanel.SetActive(false);
        defenseGeneralPanel.SetActive(false);
    }

    public void GainExperience(int amount)
    {
        experience += amount;
        CheckLevelUp();
        UpdateUI();
    }

    void CheckLevelUp()
    {
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
            EnableUpgradeButtons();  // Po zdobyciu nowego poziomu, włącz przyciski
        }
    }

    void LevelUp()
    {
        level++;
        experience = 0;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * levelMultiplier);
    }

    float GetExperienceRatio()
    {
        return Mathf.Clamp01((float)experience / experienceToNextLevel);
    }

    void UpdateUI()
    {
        levelText.text = "Level: " + level;
        experienceText.text = "Experience: " + experience;

        experienceFillImage.fillAmount = GetExperienceRatio();
    }

    // Metody do obsługi przycisków
    void UpgradeAttackMelee()
    {
        playerStatsScript.UpgradeAttackMelee();
        DisableUpgradeButtons();  // Po ulepszeniu wyłącz przyciski
    }

    void UpgradeAttackMagic()
    {
        playerStatsScript.UpgradeAttackMagic();
        DisableUpgradeButtons();  // Po ulepszeniu wyłącz przyciski
    }

    void UpgradeDefenseGeneral()
    {
        playerStatsScript.UpgradeDefenseGeneral();
        DisableUpgradeButtons();  // Po ulepszeniu wyłącz przyciski
    }
}