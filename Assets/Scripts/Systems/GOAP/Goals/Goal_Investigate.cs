using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Investigate : Goal_Base
{

    [SerializeField] int InvestigatePriority = 40;

    [SerializeField] float MinAwarenessToInvestigate = 1f;



    int CurrentPriority = 0;

    public TrackedTarget CurrentTarget;
    public Vector3 Activity;

    public override void OnTickGoal()
    {

        //CurrentPriority = 0;

        // no targets
        if (Sensors.ActiveTargets == null || Sensors.ActiveTargets.Count == 0)
            return;
        // At destination
        if (CurrentTarget != null && Agent.isWalking == false)
        {
            CurrentTarget.Awareness = 0;
            CurrentPriority = 0;
            return;
        }

        
        if (CurrentTarget == null)
        {
            // acquire a new target if possible
            foreach (var candidate in Sensors.ActiveTargets.Values)
            {
                // find sounds to investigate (DetectableTarget == false)
                if (candidate.Detectable == false && candidate.Awareness >= MinAwarenessToInvestigate)
                {
                    CurrentTarget = candidate;
                    Activity = candidate.RawPosition;
                    CurrentPriority = InvestigatePriority;
                    MinAwarenessToInvestigate = 0f;
                    return;
                }
            }
        }

    }

    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        MinAwarenessToInvestigate = 1f;
        CurrentTarget = null;        
    }

    public override int CalculatePriority()
    {
        return CurrentPriority;
    }

    public override bool CanRun()
    {
        // no targets
        if (Sensors.ActiveTargets == null || Sensors.ActiveTargets.Count == 0)
            return false;

        // check if we have anything we are aware of
        foreach(var candidate in Sensors.ActiveTargets.Values)
        {
            if (candidate.Awareness >= MinAwarenessToInvestigate)
                return true;
        }

        return false;
    }
}
