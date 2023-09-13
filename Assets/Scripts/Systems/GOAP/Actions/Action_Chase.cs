using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Chase : Action_Base
{
    List<System.Type> SupportedGoals = new List<System.Type>(new System.Type[] { typeof(Goal_Kill) });

    private float _cost = 0f;

    Goal_Kill KillGoal;

    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override float GetCost()
    {
        return _cost;
    }

    public override void OnActivated(Goal_Base _linkedGoal)
    {
        base.OnActivated(_linkedGoal);
        
        // cache the chase goal
        KillGoal = (Goal_Kill)LinkedGoal;
        Agent.IsRunning = true;
        Agent.MoveTo(KillGoal.MoveTarget);
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        Agent.IsRunning = false;
        KillGoal = null;
        _cost = 0f;
    }

    private float CalculateCost()
    {
        //Calculate cost based on distance to target, the closer the target the higher the cost. When it reaches a distance < 1.5f the cost is 2
        float distance = Vector3.Distance(Agent.transform.position, KillGoal.MoveTarget);
        return 1.5f/distance;
    }

    public override void OnTick()
    {
        Debug.Log("Im going to " + KillGoal.MoveTarget.ToString());
        Agent.MoveTo(KillGoal.MoveTarget);
        // If target is in range, cost is 2 because it is more important to kill the player than to chase him
        _cost = CalculateCost();
        
    }    
}