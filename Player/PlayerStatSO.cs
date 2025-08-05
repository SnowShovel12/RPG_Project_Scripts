using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Stat System/New Player Stat")]
public class PlayerStatSO : ScriptableObject
{
    public ModifiableValue[] stats;

    [SerializeField]
    private int level;
    [SerializeField]
    private int exp;
    private int RequireExp;

    public int Health { get; set; }
    public float Stamina {  get; set; }

    public float HealthPercent
    {
        get
        {
            if (GetModifiedValue(StatType.Health) != 0)
            {
                return (float)Health / GetModifiedValue(StatType.Health);
            }
            return 0;
        }
    }
    public float StaminaPercent
    {
        get
        {
            if (GetModifiedValue(StatType.Stamina) != 0)
            {
                return (float)Stamina / GetModifiedValue(StatType.Stamina);
            }
            return 0;
        }
    }

    public Action<PlayerStatSO> OnChangedStat;

    private void OnEnable()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        level = 1;
        exp = 0;
        RequireExp = 100;

        SetBaseValue(StatType.Health, 100);
        SetBaseValue(StatType.Stamina, 100);
        SetBaseValue(StatType.Strengh, 10);
        SetBaseValue(StatType.Defense, 10);

        foreach (ModifiableValue stat in stats)
        {
            stat.ClearModifier();
            stat.ModifyValue();
        }

        Health = GetModifiedValue(StatType.Health);
        Stamina = GetModifiedValue(StatType.Stamina);
    }

    public int GetModifiedValue(StatType type)
    {
        foreach (ModifiableValue stat in stats)
        {
            if (stat.Type == type)
            {
                return stat.ModifiedValue;
            }
        }

        return -1;
    }

    private void SetBaseValue (StatType type, int value)
    {
        foreach (ModifiableValue stat in stats)
        {
            if (stat.Type == type)
            {
                stat.baseValue = value;
            }
        }

        OnChangedStat?.Invoke(this);
    }

    public void AddHealth (int value)
    {
        Health += value;
        Health = Mathf.Clamp(Health, 0, GetModifiedValue(StatType.Health));

        OnChangedStat?.Invoke(this);
    }

    public void AddStamina (float value)
    {
        Stamina += value;
        Stamina = Mathf.Clamp(Stamina, 0, GetModifiedValue(StatType.Stamina));

        OnChangedStat?.Invoke(this);
    }

    public void LevelUp()
    {
        level += 1;
        RequireExp = level * 100;

        OnChangedStat?.Invoke(this);
    }

    public void AddExp (int value)
    {
        exp += value;

        while (exp >= RequireExp)
        {
            exp -= RequireExp;
            LevelUp();
        }

        OnChangedStat?.Invoke(this);
    }

    public void HealPlayer()
    {
        Health = GetModifiedValue(StatType.Health);
        Stamina = GetModifiedValue(StatType.Stamina);
    }
}
