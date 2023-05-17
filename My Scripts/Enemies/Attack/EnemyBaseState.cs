using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(TriceratopsController controller);
    public abstract void Update(TriceratopsController controller);
    public abstract void ExitState(TriceratopsController controller);
}
public class TriceratopsIdleState : EnemyBaseState
{
    public override void EnterState(TriceratopsController controller)
    {        
        controller.ResetProperties();
        //Debug.Log("ENTER IDLE STATE");
    }

    public override void ExitState(TriceratopsController controller)
    {
        //Debug.Log("EXIT IDLE STATE");
    }

    public override void Update(TriceratopsController controller)
    {
        controller.IdleMovement();

        if (controller.InRange())
        {
            if (controller.EggTime) controller.ChangeState(controller.EggLayingState);
            else controller.ChangeState(controller.ShootState);
        }
    }
}
public class TriceratopsEggLayingState : EnemyBaseState
{
    public override void EnterState(TriceratopsController controller)
    {
        controller.ResetProperties();
        //Debug.Log("ENTER EGG STATE");
    }

    public override void ExitState(TriceratopsController controller)
    {
        controller.EggTimeFalse();
        controller.EggLay.ResetEggLay();
        //Debug.Log("EXIT EGG STATE");
    }

    public override void Update(TriceratopsController controller)
    {
        controller.EggMovement();
        controller.EggLay.EggLayUpdate();       
    }
}

public class TriceratopsShootState : EnemyBaseState
{
    public override void EnterState(TriceratopsController controller)
    {
        controller.ResetProperties();
        //Debug.Log("ENTER SHOOT STATE");
    }

    public override void ExitState(TriceratopsController controller)
    {        
        controller.EggTimeTrue();
        controller.Shoot.ResetAll();
        //Debug.Log("EXIT SHOOT STATE");
    }

    public override void Update(TriceratopsController controller)
    {
        controller.ShootMovement();
        controller.Shoot.ShootUpdate();
    }
}