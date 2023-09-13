using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Investigate : Action_Base
{
    List<System.Type> SupportedGoals = new List<System.Type>(new System.Type[] { typeof(Goal_Investigate) });


    Goal_Investigate InvestigateGoal;
    int wanders = 3;

    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override float GetCost()
    {
        return 0f;
    }

    public override void OnActivated(Goal_Base _linkedGoal)
    {
        InvestigateGoal = (Goal_Investigate)_linkedGoal;
        Agent.isWalking = true;
        base.OnActivated(_linkedGoal);
        Vector3 location = InvestigateGoal.Activity;
        Agent.MoveTo(location);
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        Agent.isWalking = false;
        InvestigateGoal = null;
    }

    public override void OnTick()
    {
        if (Agent.AtDestination)
        {
            Agent.isWalking = false;
        }
    }   
}
