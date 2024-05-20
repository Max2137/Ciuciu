using UnityEngine;
using TMPro;

public class PlayerStatsScript : MonoBehaviour
{
    public PlayerXpBar xpBar;  // Referencja do skryptu PlayerXpBar
    public TMP_Text attackMeleeText, attackMagicText, defenseGeneralText;  // Referencje do tekst�w UI (TextMeshProUGUI)

    private PlayerStats playerStats;  // Referencja do skryptu PlayerStats

    void Start()
    {
        // Pobierz skrypt PlayerStats przypisany do tego obiektu
        playerStats = GetComponent<PlayerStats>();

        // Zaktualizuj teksty UI
        UpdateUITexts();
    }

    void UpdateUITexts()
    {
        // Aktualizuj teksty UI z warto�ciami statystyk
        attackMeleeText.text = "Attack Melee:  " + playerStats.AttackMeleeStat.ToString();
        attackMagicText.text = "Attack Magic:  " + playerStats.AttackMagicStat.ToString();
        defenseGeneralText.text = "Defense Gnr:   " + playerStats.DefenseGeneralStat.ToString();
    }

    public void UpgradeAttackMelee()
    {
        // Zwi�ksz statystyk� Attack Melee o 1
        playerStats.AttackMeleeStat++;

        // Zaktualizuj teksty UI
        UpdateUITexts();
    }

    public void UpgradeAttackMagic()
    {
        // Zwi�ksz statystyk� Attack Magic o 1
        playerStats.AttackMagicStat++;

        // Zaktualizuj teksty UI
        UpdateUITexts();
    }

    public void UpgradeDefenseGeneral()
    {
        // Zwi�ksz statystyk� Defense General o 1
        playerStats.DefenseGeneralStat++;

        // Zaktualizuj teksty UI
        UpdateUITexts();
    }
}