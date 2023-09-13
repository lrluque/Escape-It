using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedTarget
{
    public DetectableTarget Detectable;
    public Vector3 RawPosition;

    public float LastSensedTime = -1f;
    public float Awareness; // 0     = not aware (will be culled); 
                            // 0-1   = rough idea (no set location); 
                            // 1-2   = likely target (location)
                            // 2     = fully detected



    // UpdateAwareness is called by the Report functions to update the awareness of a target
    // returns true if the awareness has changed

    public bool UpdateAwareness(DetectableTarget target, Vector3 position, float awareness, float minAwareness) 
    {
        var oldAwareness = Awareness;

        if (target != null)
            Detectable      = target;
            RawPosition     = position;
            LastSensedTime  = Time.time;
            Awareness       = Mathf.Clamp(Mathf.Max(Awareness, minAwareness) + awareness, 0f, 2f);
        
        // return true if awareness has changed
        if (oldAwareness < 2f && Awareness >= 2f)
            return true;
        if (oldAwareness < 1f && Awareness >= 1f)
            return true;
        if (oldAwareness <= 0f && Awareness >= 0f)
            return true;

        return false;

    }

    public bool DecayAwareness(float decayTime, float amount)
    {
        // detected too recently - no change
        if ((Time.time - LastSensedTime) < decayTime)
            return false;

        var oldAwareness = Awareness;

        Awareness -= amount;

        if (oldAwareness >= 2f && Awareness < 2f)
            return true;
        if (oldAwareness >= 1f && Awareness < 1f)
            return true;
        return Awareness <= 0f;
    }
}