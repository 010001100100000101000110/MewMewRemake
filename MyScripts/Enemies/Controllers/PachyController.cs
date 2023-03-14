using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class PachyController : MonoBehaviour
{
    EnemyHelper helper;
    Vector2 targetDestination;

    float range;
    float attackCooldown;
    float nextAttack;    
    [SerializeField] float attackDistance;    
    [SerializeField] float attackSpeed;
    float regMoveSpeed;

    bool attackStarted;

    [SerializeField] float chargingTime;
    void Start()
    {
        helper = GetComponent<EnemyHelper>();
        regMoveSpeed = helper.Stats.MoveSpeed;
        helper.Agent.speed = regMoveSpeed;
        helper.Agent.updateRotation = false;
        helper.Agent.updateUpAxis = false;
        range = helper.Stats.Range;
        attackCooldown = helper.Stats.AttackCooldown;
    }

    void Update()
    {
        Movement();
        Attack();
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
 
    void Movement()
    {
        if (helper.Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) helper.Agent.speed = regMoveSpeed;
        if (helper.DistanceToPlayer() > range && !attackStarted && helper.Animator.GetInteger("State") == 0) PlayerAsDestination();
        if (helper.DistanceToPlayer() <= range && !attackStarted && helper.Animator.GetInteger("State") == 0)
        {
            if (Time.time < nextAttack)
            {
                PlayerAsDestination();
                return;
            }
            StartCoroutine(Charging());
        }
    }

    
    void Attack()
    {
        if (helper.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (helper.Agent.isStopped) SetChargeDestination(out targetDestination);

            NavMeshHit hit;
            var areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
            if (NavMesh.SamplePosition(targetDestination, out hit, 20, areaMask))
            {
                helper.Agent.SetDestination(hit.position);
                Collider2D[] hits = Physics2D.OverlapCircleAll(hit.position, 2);
                
                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.gameObject.GetComponent<PachyController>())
                        {
                            nextAttack = Time.time + attackCooldown;
                            WalkState();
                        }
                    }
                }
            }
        }
    }

    //Vector2 PredictedSpot()
    //{
    //    Vector2 playerPos = helper.Player.position;
    //    Vector2 playerMovementDirection = helper.Player.GetComponent<PlayerMovement>().movementDirection;
    //    float playerSpeed = helper.Player.GetComponent<PlayerMovement>().activeMovementSpeed;
    //    Vector2 landingSpot = playerPos + playerMovementDirection.normalized * (playerSpeed -2);
    //    return landingSpot;
    //}

    void SetChargeDestination(out Vector2 tP)
    {
        helper.Agent.isStopped = false;
        helper.Agent.speed = attackSpeed;

        Vector3 direction = helper.Player.position - transform.position;
        Vector3 targetPos = helper.Player.transform.position + direction.normalized * attackDistance;

        //Predicted() vvvvv
        //Vector3 predicted = PredictedSpot();
        //Vector3 direction = predicted - transform.position;
        //Vector3 targetPos = predicted + direction.normalized * attackDistance;

        tP = targetPos;
    }
    void WalkState()
    {
        helper.Agent.speed = regMoveSpeed;
        helper.Animator.SetInteger("State", 0);
        attackStarted = false; 
    }
    void PlayerAsDestination()
    {
        helper.Agent.speed = regMoveSpeed;        
        helper.Agent.SetDestination(helper.Player.position);        
    }

    IEnumerator Charging()
    {
        attackStarted = true;
        helper.Animator.SetInteger("State", 1);
        helper.Agent.isStopped = true;
        helper.Agent.speed = 0;
        helper.Agent.SetDestination(transform.position);        
        yield return new WaitForSecondsRealtime(chargingTime);
        helper.Animator.SetInteger("State", 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (helper.Animator.GetInteger("State") == 2)
        {
            PlayerHealth playerHealth;
            if (collision.gameObject.TryGetComponent<PlayerHealth>(out playerHealth))
            {
                playerHealth.TakeDamage(helper.Stats.Damage, gameObject);
            }
        }        
    }
}
