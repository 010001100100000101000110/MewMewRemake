using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatSO", menuName = "Scriptable Objects/Player Base Stat")]

public class PlayerBaseStatsSO : ScriptableObject
{
    [Header("General Stats")]
    public int Health;

    [Header("Movement")]
    public float MoveSpeed;
    public int DashCharges;
    public float DashLength;
    public float DashSpeed;

    [Header("Weapon Stats")]
    public int Damage;
    public float Range;
    public float ShotsPerMinute;
    public float ProjectileSpeed;
    public int ProjectileAmount;
    
}
