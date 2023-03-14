using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PteroController : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] float flyDistance;
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
        if (helper.DistanceToPlayer() >= (flyDistance - 1) || helper.Agent.velocity.magnitude == 0) SetPteroDestination(helper.Player.position);
    }

    void SetPteroDestination(Vector3 playerPos)
    {
        float distance;
        if (transform.position.x < playerPos.x) distance = flyDistance;
        else distance = -flyDistance;

        Vector3 newPos = new Vector3(playerPos.x + distance, helper.Player.position.y + 4, 0);
        pos = newPos;
        helper.Agent.SetDestination(pos);
    }
}
