using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHelper : MonoBehaviour
{
    [field: SerializeField] public EnemyStatsSO Stats { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform Player { get; private set; }
    public Animator Animator { get; private set; }

    public EnemyManager Manager { get; private set; }
    public TopGunManager TGManager { get; private set; }

    private int id;

    public int ID => id;

    private int enemyValue;
    public int EnemyValue 
    {
        get => enemyValue;
        set
        {
            if (value > 0 && value <= 30) enemyValue = value;
            else Debug.LogWarning("Tried to set enemy value as less than 0 or more than 10");
        }
    }



    public void Initialize(Transform player, EnemyManager enemyManager, TopGunManager topGunManager, int enemyValue)
    {
        this.Player = player;
        Manager = enemyManager;
        TGManager = topGunManager;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        id = (int)Stats.Type;
        EnemyValue = enemyValue;
    }

    public float DistanceToPlayer()
    {
        float distance = Vector2.Distance(transform.position, Player.position);
        return distance;
    }
}
