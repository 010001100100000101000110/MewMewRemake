using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] PlayerBaseStatsSO baseStats;
    [SerializeField] public SO_SentryMode sentryMode;
    [SerializeField] float movementSpeed;
    [SerializeField] float damage;
    [SerializeField] int projectileAmount;
    [SerializeField] float projectileSpread;
    [SerializeField] int projectilePenetrationAmount;
    [SerializeField] int dashCharges;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLength;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileRange;
    [SerializeField] float maxHealth;
    [SerializeField] float shotsPerMinute;

    public float MaxHealth => maxHealth;
    public float MoveSpeed =>  sentryModeActive ? sentryMode.MoveSpeed : movementSpeed;
    public float Damage => instakill ? 99999 : damage;
    public int ProjectileAmount => sentryModeActive ? 1 : projectileAmount;
    public float ProjectileSpread => projectileSpread;
    public int ProjectilePenetrationAmount => projectilePenetrationAmount;

    public int DashCharges => dashCharges;
    public float DashSpeed => dashSpeed;
    public float DashLength => dashLength;
    public float ProjectileSpeed => projectileSpeed;
    public float ShotsPerMinute => sentryModeActive ? shotsPerMinute + sentryMode.FireRate : shotsPerMinute;
    public float ProjectileRange => sentryModeActive ? sentryMode.ProjectileRange : projectileRange;

    #region Stat Increments
    [SerializeField] float movevementSpeedMultiplier;
    [SerializeField] float damageMultiplier;
    [SerializeField] int projectileAmountIncrement;
    [SerializeField] int projectilePenetrationIncrement;
    [SerializeField] float projectileSpeedMultiplier;
    [SerializeField] float shotsPerMinuteMultiplier;
    [SerializeField] float projectileRangeMultiplier;
    [SerializeField] float DPS;
    #endregion

    float baseMoveSpeed;
    float baseDamage;
    int baseProjectileAmount;
    int basePenetrationAmount;
    float baseShotsPerMinute;
    float baseProjectileSpeed;
    float baseProjectileRange;

    public float BaseMovementSpeed => baseMoveSpeed;
    public float BaseDamage => baseDamage;
    public int BaseProjectileAmount => baseProjectileAmount;
    public int BasePenetrationAmount => basePenetrationAmount;
    public float BaseShotsPerMinute => baseShotsPerMinute;

    public float BaseProjectileSpeed => baseProjectileSpeed;
    public float BaseProjectileRange => baseProjectileRange;


    private void Awake()
    {
        InitializeBaseStats();
    }

    public bool instakill { get; private set; }
    private bool sentryModeActive;

    public void TurnOnSentryMode() => sentryModeActive = true;
    public void TurnOffSentryMode() => sentryModeActive = false;
    public void TurnOnInstakill() => instakill = true;
    public void TurnOffInstakill() => instakill = false;

    private void InitializeBaseStats()
    {
        baseMoveSpeed = baseStats.MoveSpeed;
        baseDamage = baseStats.Damage;
        baseProjectileAmount = baseStats.ProjectileAmount;
        basePenetrationAmount = 0;
        baseShotsPerMinute = baseStats.ShotsPerMinute;
        dashCharges = baseStats.DashCharges;
        dashLength = baseStats.DashLength;
        dashSpeed = baseStats.DashSpeed;
        baseProjectileSpeed = baseStats.ProjectileSpeed;
        baseProjectileRange = baseStats.Range;
        projectileSpread = 20;
        maxHealth = baseStats.Health;
        DPS = (ShotsPerMinute * Damage) / 60;
        UpdateStats();
    }


    private void UpdateStats()
    {
        movementSpeed = baseMoveSpeed + (baseMoveSpeed * (movevementSpeedMultiplier * 0.01f));
        damage = baseDamage + (baseDamage * (damageMultiplier * 0.01f));
        projectileAmount = baseProjectileAmount + projectileAmountIncrement;
        projectilePenetrationAmount = basePenetrationAmount + projectilePenetrationIncrement;
        shotsPerMinute = baseShotsPerMinute + (baseShotsPerMinute * (shotsPerMinuteMultiplier * 0.01f));
        dashCharges = baseStats.DashCharges;
        dashLength = baseStats.DashLength;
        dashSpeed = baseStats.DashSpeed;
        projectileSpeed = baseProjectileSpeed + (baseProjectileSpeed * (projectileSpeedMultiplier * 0.01f));
        projectileRange = baseProjectileRange + (baseProjectileRange * (projectileRangeMultiplier * 0.01f));
        projectileSpread = 20;
        maxHealth = baseStats.Health;
        DPS = (ShotsPerMinute * Damage) / 60;
    }
    
    public void UpgradeMultiplier(Stat stat, float multiplier)
    {
        switch (stat)
        {
            case Stat.Damage:
            {
                damageMultiplier += multiplier;
                break;
            }

            case Stat.FireRate:
                {
                    shotsPerMinuteMultiplier += multiplier;
                    break;
                }

            case Stat.MovementSpeed:
                {
                    movevementSpeedMultiplier += multiplier;
                    break;
                }

            case Stat.Range:
                {
                    projectileRangeMultiplier += multiplier;
                    break;
                }

            case Stat.ProjectilePenetrationCount:
                {
                    projectilePenetrationIncrement += Mathf.RoundToInt(multiplier);
                    break;
                }

            case Stat.ProjectileAmount:
                {
                    projectileAmountIncrement += Mathf.RoundToInt(multiplier);
                    break;
                }
        }
        
        UpdateStats();
    }

    public void UpgradeStat(StatUpgrade statUpgrade)
    {
        if (statUpgrade.PercentageValue) ApplyPercentageStat(statUpgrade.Type, statUpgrade.Amount);
        else ApplyFixedStat(statUpgrade.Type, statUpgrade.Amount);
        UpdateStats();
    }

    public void UpgradeStat(StatUpgrade statUpgrade, float overrideAmount)
    {
        if (statUpgrade.PercentageValue) ApplyPercentageStat(statUpgrade.Type, overrideAmount);
        else ApplyFixedStat(statUpgrade.Type, overrideAmount);
        UpdateStats();
    }

    private void ApplyFixedStat(Stat stat, float amount)
    {
        switch (stat)
        {
            case Stat.ProjectileAmount:
                {
                    baseProjectileAmount += Mathf.RoundToInt(amount);
                    if (baseProjectileAmount < 1) baseProjectileAmount = 1;
                    break;
                }

            case Stat.Damage:
                {
                    baseDamage += amount;
                    break;
                }

            case Stat.FireRate:
                {
                    baseShotsPerMinute += amount;
                    break;
                }

            case Stat.MovementSpeed:
                {
                    baseMoveSpeed += amount;
                    break;
                }

            case Stat.Range:
                {
                    baseProjectileRange += amount;
                    break;
                }
            case Stat.ProjectilePenetrationCount:
                {
                    basePenetrationAmount += Mathf.RoundToInt(amount);
                    if (basePenetrationAmount < 0) basePenetrationAmount = 0;
                    break;
                }
        }
    }

    private void ApplyPercentageStat(Stat stat, float amount)
    {
        switch (stat)
        {
            case Stat.ProjectileAmount:
                {
                    int value = Mathf.RoundToInt(percentageValue(baseProjectileAmount, amount));
                    baseProjectileAmount = baseProjectileAmount + value;
                    if (baseProjectileAmount < 1) baseProjectileAmount = 1;
                    //if (projectileAmountIncrement < 0) projectileAmountIncrement = 0;
                    break;
                }

            case Stat.Damage:
                {
                    float dmgIncrease = percentageValue(baseDamage, amount);
                    baseDamage += dmgIncrease;
                    //if (damageIncrement < -99) damageIncrement = -99;
                    break;
                }

            case Stat.FireRate:
                {
                    baseShotsPerMinute += percentageValue(baseShotsPerMinute, amount);
                    //if (shotsPerMinuteIncrement < -99) shotsPerMinuteIncrement = -99;
                    break;
                }

            case Stat.MovementSpeed:
                {
                    baseMoveSpeed += percentageValue(baseMoveSpeed, amount);
                    //if (movevementSpeedIncrement < -99) movevementSpeedIncrement = -99;
                    break;
                }
            case Stat.Range:
                {
                    baseProjectileRange += percentageValue(baseProjectileRange, amount);
                    //if (projectileRangeIncrement < -99) projectileRangeIncrement = -99;
                    break;
                }
            case Stat.ProjectilePenetrationCount:
                {
                    int value = Mathf.RoundToInt(percentageValue(basePenetrationAmount, amount));
                    basePenetrationAmount += value;
                    if (basePenetrationAmount < 0) basePenetrationAmount = 0;
                    //if (projectileAmountIncrement < 0) projectileAmountIncrement = 0;
                    break;
                }
        }
    }

    private float percentageValue(float originalValue, float percentage)
    {
        percentage = percentage * 0.01f;
        return originalValue * percentage;
    }

    private float percentageValue(float originalValue)
    {
        
        return originalValue * 0.01f;
    }
}

public enum Stat { FireRate, Damage, ProjectileAmount, MovementSpeed, Range, ProjectilePenetrationCount }
