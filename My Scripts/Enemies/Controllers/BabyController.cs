using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour
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
            Vector2 direction = (transform.position - helper.TGManager.transform.position).normalized;
            //helper.Agent.destination = direction * 15;
            Vector2 newPos = (Vector2)transform.position + direction;
            helper.Agent.SetDestination(newPos);
        }
        else
        {
            if (helper.Animator.GetCurrentAnimatorStateInfo(0).IsName("Explode")) StopMovement();
            else PlayerAsDestination();
        }
    }

    void PlayerAsDestination()
    {
        pos = helper.Player.position;
        helper.Agent.SetDestination(pos);
        helper.Agent.isStopped = false;
    }

    void StopMovement()
    {
        pos = gameObject.transform.position;
        helper.Agent.SetDestination(pos);
        helper.Agent.isStopped = true;
    }
}
