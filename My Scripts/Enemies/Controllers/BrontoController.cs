using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrontoController : MonoBehaviour
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
            Vector2 direction = (transform.position - helper.TGManager.transform.position).normalized;
            //helper.Agent.destination = direction * 15;
            Vector2 newPos = (Vector2)transform.position + direction;
            helper.Agent.SetDestination(newPos);
        }
        else
        {
            pos = helper.Player.position;
            helper.Agent.SetDestination(pos);
        }
    }
}
