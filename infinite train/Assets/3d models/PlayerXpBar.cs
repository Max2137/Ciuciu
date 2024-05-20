using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    public List<GameObject> Decorations;

    public PlayerStatsScript playerStatsScript;  // Reference to the PlayerStatsScript

    public GameObject attackMeleePanel, attackMagicPanel, defenseGeneralPanel;

    public AudioClip LevelUpSound;
    public AudioClip PickingPowerSound;

    private AudioSource audioSource; // Declare audioSource variable here

    void Start()
    {
        experience = initialExperience;
        UpdateUI();
        SetupUpgradeButtons();

        attackMeleePanel.SetActive(false);
        attackMagicPanel.SetActive(false);
        defenseGeneralPanel.SetActive(false);
        foreach (GameObject obiekt in Decorations)
        {
            obiekt.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>(); // Assign audioSource here
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }



    void SetupUpgradeButtons()
    {
        attackMeleePanel.SetActive(false);
        attackMagicPanel.SetActive(false);
        defenseGeneralPanel.SetActive(false);
        foreach (GameObject obiekt in Decorations)
        {
            obiekt.SetActive(false);
        }

        // Dodaj nasłuchiwacze kliknięć do przycisków
        attackMeleePanel.GetComponent<Button>().onClick.AddListener(UpgradeAttackMelee);
        attackMagicPanel.GetComponent<Button>().onClick.AddListener(UpgradeAttackMagic);
        defenseGeneralPanel.GetComponent<Button>().onClick.AddListener(UpgradeDefenseGeneral);
    }

    float decorationsActivationTime; // Dodajemy zmienną do przechowywania czasu aktywacji decorations
    bool isWaiting;

    void EnableUpgradeButtons()
    {
        Time.timeScale = 0;

        isWaiting = true;

        // Aktywacja decorations
        foreach (GameObject obiekt in Decorations)
        {
            obiekt.SetActive(true);
        }
    }

    private void Update()
    {
        if (isWaiting)
        {
            decorationsActivationTime += 1;
            {
                if (decorationsActivationTime >= 500)
                {
                    attackMeleePanel.SetActive(true);
                    attackMagicPanel.SetActive(true);
                    defenseGeneralPanel.SetActive(true);

                    isWaiting = false;
                    decorationsActivationTime = 0;
                }
            }
        }
    }


    void DisableUpgradeButtons()
    {
        if (PickingPowerSound != null)
        {
            audioSource.PlayOneShot(PickingPowerSound); // Odtwarzanie dźwięku otwierania drzwi
        }

        Time.timeScale = 1;

        attackMeleePanel.SetActive(false);
        attackMagicPanel.SetActive(false);
        defenseGeneralPanel.SetActive(false);
        foreach (GameObject obiekt in Decorations)
        {
            obiekt.SetActive(false);
        }
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

        if (LevelUpSound != null)
        {
            audioSource.PlayOneShot(LevelUpSound); // Odtwarzanie dźwięku otwierania drzwi
        }
    }

    float GetExperienceRatio()
    {
        return Mathf.Clamp01((float)experience / experienceToNextLevel);
    }

    void UpdateUI()
    {
        levelText.text = "" + level;
        experienceText.text = "" + experience;

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