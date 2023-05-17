using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriceratopsController : MonoBehaviour
{
    public EnemyHelper Helper { get; private set; }
    public TriceratopsEggLay EggLay { get; private set; }
    public TriceratopsShoot Shoot { get; private set; }

    Vector3 pos;
    Vector3 sprintPos;
    float regMoveSpeed;
    [SerializeField] float sprintSpeed;
    float range;    
    public bool EggTime { get; private set; }
    public bool Sprinting { get; private set; }
    [SerializeField] float blindSpotTimerMax;
    float timer;
    [SerializeField] float distanceAwayFromPlayer;

    #region states
    public EnemyBaseState CurrentState { get; private set; }
    public readonly TriceratopsIdleState IdleState = new TriceratopsIdleState();
    public readonly TriceratopsEggLayingState EggLayingState = new TriceratopsEggLayingState();
    public readonly TriceratopsShootState ShootState = new TriceratopsShootState();
    #endregion

    void Start()
    {
        Helper = GetComponent<EnemyHelper>();
        EggLay = GetComponent<TriceratopsEggLay>();
        Shoot = GetComponent<TriceratopsShoot>();

        range = Helper.Stats.Range;
        regMoveSpeed = Helper.Stats.MoveSpeed;
        timer = blindSpotTimerMax;

        Helper.Agent.speed = regMoveSpeed;
        Helper.Agent.updateRotation = false;
        Helper.Agent.updateUpAxis = false;
                
        ChangeState(IdleState);
        CurrentState = IdleState;
    }


    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        CurrentState.Update(this);
    }

    public void ChangeState(EnemyBaseState state)
    {
        if (CurrentState != null) CurrentState.ExitState(this);
        CurrentState = state;
        CurrentState.EnterState(this);
    }

    public void IdleMovement()
    {
        if (!Helper.TGManager.TopGunning) PlayerAsDestination();
        else
        {
            Helper.Agent.isStopped = false;
            Helper.Agent.speed = regMoveSpeed;

            Vector2 direction = (transform.position - Helper.TGManager.transform.position).normalized;
            //helper.Agent.destination = direction * 15;
            Vector2 newPos = (Vector2)transform.position + direction;
            Helper.Agent.SetDestination(newPos);
        }        
    }

    public void EggMovement()
    {
        if (!Helper.TGManager.TopGunning)
        {
            if (EggLay.IsLayingEggs) StopMovement();
            else PlayerAsDestination();
        }
        else ChangeState(IdleState);       
    }

    #region Shoot Movement
    public void ShootMovement()
    {
        if (!Sprinting)
        {
            if (gameObject.transform.position.x < Helper.Player.position.x) pos = new Vector3(Helper.Player.position.x - 4, Helper.Player.position.y, 0);
            else pos = new Vector3(Helper.Player.position.x + 4, Helper.Player.position.y, 0);
            Helper.Agent.SetDestination(pos);
        }
        else
        {
            NavMeshHit hit;
            var areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
            if (NavMesh.SamplePosition(sprintPos, out hit, 20, areaMask))
            {
                Helper.Agent.SetDestination(hit.position);
                Collider2D[] hits = Physics2D.OverlapCircleAll(hit.position, 2);

                if (hits != null)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.gameObject.GetComponent<TriceratopsController>())
                        {
                            Sprinting = false;
                            StopMovement();
                            Helper.Agent.speed = regMoveSpeed;
                            Helper.Agent.isStopped = false;
                        }
                    }
                }
            }
        }

        if (NotInRange())
        {
            timer -= Time.deltaTime;
        }
        else timer = blindSpotTimerMax;
        if (timer <= 0) Sprint();
        
    }

    void Sprint()
    {
        if (!Sprinting)
        {
            Sprinting = true;
            Helper.Agent.speed = sprintSpeed;
            Vector3 pos;
            if (gameObject.transform.position.x < Helper.Player.position.x) pos = new Vector3(Helper.Player.position.x - distanceAwayFromPlayer, Helper.Player.position.y, 0);
            else pos = new Vector3(Helper.Player.position.x + distanceAwayFromPlayer, Helper.Player.position.y, 0);
            sprintPos = pos;            
        }
        
        Helper.Agent.SetDestination(sprintPos);
    }

    public bool NotInRange()
    {
        if (Helper.Player.position.x > transform.position.x - 4 && Helper.Player.position.x < transform.position.x + 3) return true;
        else return false;
    }
    #endregion


    public bool InRange()
    {
        if (!Helper.TGManager.TopGunning)
        {
            if (Helper.DistanceToPlayer() < range) return true;
            else return false;
        }
        else return false;
    }

    
    public void StopMovement()
    {
        pos = transform.position;
        Helper.Agent.SetDestination(pos);
        Helper.Agent.isStopped = true;
        Helper.Agent.speed = 0;
    }
    public void PlayerAsDestination()
    {
        Helper.Agent.isStopped = false;
        Helper.Agent.speed = regMoveSpeed;
        pos = Helper.Player.position;
        Helper.Agent.SetDestination(pos);
    }

    public void ResetProperties()
    {
        Helper.Agent.isStopped = false;
        Helper.Agent.speed = regMoveSpeed;
    }

    public void EggTimeTrue()
    {
        EggTime = true;
    }

    public void EggTimeFalse()
    {
        EggTime = false;
    }

    //EnemyHelper helper;
    //Vector3 pos;
    //float regMoveSpeed;
    //public TriceratopsEggLay EggLay { get; private set; }
    //public TriceratopsShoot Shoot { get; private set; }
    //float range;


    //#region states
    //public EnemyBaseState CurrentState { get; private set; }
    //public readonly TriceratopsIdleState IdleState = new TriceratopsIdleState();
    //public readonly TriceratopsEggLayingState EggLayingState = new TriceratopsEggLayingState();
    //public readonly TriceratopsShootState ShootState = new TriceratopsShootState();
    //#endregion

    //void Start()
    //{
    //    helper = GetComponent<EnemyHelper>();
    //    EggLay = GetComponent<TriceratopsEggLay>();
    //    Shoot = GetComponent<TriceratopsShoot>();
    //    regMoveSpeed = helper.Stats.MoveSpeed;
    //    helper.Agent.speed = regMoveSpeed;
    //    helper.Agent.updateRotation = false;
    //    helper.Agent.updateUpAxis = false;
    //    range = helper.Stats.Range;
    //}


    //void Update()
    //{
    //    this.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    //    Movement();
    //}

    //public void ChangeState(EnemyBaseState state)
    //{
    //    if (CurrentState != null) CurrentState.ExitState(this);
    //    CurrentState = state;
    //    CurrentState.EnterState(this);
    //}

    //void Movement()
    //{
    //    if (!helper.TGManager.TopGunning)
    //    {
    //        if (EggLay.IsLayingEggs) StopMovement();
    //        else PlayerAsDestination();
    //    }
    //    else
    //    {
    //        helper.Agent.isStopped = false;
    //        helper.Agent.speed = helper.Stats.MoveSpeed;

    //        Vector2 direction = (transform.position - helper.TGManager.transform.position).normalized;
    //        //helper.Agent.destination = direction * 15;
    //        Vector2 newPos = (Vector2)transform.position + direction;
    //        helper.Agent.SetDestination(newPos);
    //    }
    //}

    //public bool InRange()
    //{
    //    if (!helper.TGManager.TopGunning)
    //    {
    //        if (helper.DistanceToPlayer() < range) return true;
    //        else return false;
    //    }
    //    else return false;
    //}

    //void PlayerAsDestination()
    //{
    //    //if (gameObject.transform.position.x < helper.Player.position.x) pos = new Vector3(helper.Player.position.x - 2, helper.Player.position.y, 0);
    //    //else pos = new Vector3(helper.Player.position.x + 2, helper.Player.position.y, 0);

    //    pos = helper.Player.position;
    //    helper.Agent.SetDestination(pos);
    //    helper.Agent.isStopped = false;
    //    helper.Agent.speed = helper.Stats.MoveSpeed;
    //}
    //void StopMovement()
    //{
    //    pos = transform.position;
    //    helper.Agent.SetDestination(pos);
    //    helper.Agent.isStopped = true;
    //    helper.Agent.speed = 0;
    //}
}
