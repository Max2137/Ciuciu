using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Statystyki gracza
    private int attackMeleeStat;
    private int attackMagicStat;
    private int defenseGeneralStat;

    // W�a�ciwo�ci do dost�pu do statystyk
    public int AttackMeleeStat
    {
        get { return attackMeleeStat; }
        set { attackMeleeStat = value; }
    }

    public int AttackMagicStat
    {
        get { return attackMagicStat; }
        set { attackMagicStat = value; }
    }

    public int DefenseGeneralStat
    {
        get { return defenseGeneralStat; }
        set { defenseGeneralStat = value; }
    }

    void Start()
    {
        // Inicjalizuj statystyki gracza
        AttackMeleeStat = 0;
        AttackMagicStat = 0;
        DefenseGeneralStat = 0;
    }
}