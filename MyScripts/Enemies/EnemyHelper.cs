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

    private int id;

    public int ID => id;

    void Awake()
    {        
        Agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Animator = GetComponent<Animator>();
        Manager = FindObjectOfType<EnemyManager>();
        id = (int)Stats.Type;
    }
    public float DistanceToPlayer()
    {
        float distance = Vector2.Distance(transform.position, Player.position);
        return distance;
    }
}
