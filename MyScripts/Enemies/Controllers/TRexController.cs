using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class TRexController : MonoBehaviour
{
    Vector3 pos;
    EnemyHelper helper;

    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        helper.Agent.speed = helper.Stats.MoveSpeed;
        helper.Agent.updateRotation = false;
        helper.Agent.updateUpAxis = false;
    }

    void Update()
    {
        Movement();
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Movement()
    {
        pos = helper.Player.position;
        helper.Agent.SetDestination(pos);
    }
}
