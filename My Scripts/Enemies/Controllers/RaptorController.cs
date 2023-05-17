using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RaptorController : MonoBehaviour
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
        if (helper.TGManager.TopGunning)
        {
            helper.Agent.isStopped = false;
            helper.Agent.speed = helper.Stats.MoveSpeed;
            Vector2 direction = (transform.position - helper.TGManager.transform.position).normalized;
            //helper.Agent.destination = direction * 15;
            Vector2 newPos = (Vector2)transform.position + direction;
            helper.Agent.SetDestination(newPos);
        }
        else
        {
            if (helper.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                helper.Agent.isStopped = true;
                helper.Agent.SetDestination(transform.position);
            }
            else PlayerAsDestination();
        }
    }

    void PlayerAsDestination()
    {
        pos = helper.Player.position;
        helper.Agent.SetDestination(pos);
        helper.Agent.isStopped = false;
    }   
}
