using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Scriptable Objects/EnemyStat")]
public class EnemyStatsSO : ScriptableObject
{
    [Header("Basics")]
    
    public EnemyType Type;
    public float MoveSpeed;
    public int Health;

    [Header("Shooting/Attack")]
    public float Damage;
    public float Range;
    public float ProjectileRange;
    public float ShotSpeed;
    public float ShotsPerMinute;
    public float AttackCooldown;
}
public enum EnemyType { T_Rex, Raptor, Pterodactyl, Pachycephalosaurus, Brontosaurus, Ankylosaurus, Triceratops, Triceratops_Baby }
