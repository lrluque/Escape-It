using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Kill : Action_Base
{
    List<System.Type> SupportedGoals = new List<System.Type>(new System.Type[] { typeof(Goal_Kill) });

    Goal_Kill KillGoal;

    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override float GetCost()
    {
        return 0.95f;
    }

    public override void OnActivated(Goal_Base _linkedGoal)
    {
        base.OnActivated(_linkedGoal);
        
        // cache the kill goal
        KillGoal = (Goal_Kill)LinkedGoal;
        Agent.IsRunning = false;
        //Activate jumpscare
        this.StartCoroutine(this.GetComponent<Jumpscare>().JumpScarePlayer());
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        KillGoal = null;
    }

     
}
