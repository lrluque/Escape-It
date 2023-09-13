using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class AwarenessSystem : MonoBehaviour
{
    [SerializeField] AnimationCurve VisionSensitivity;
    [SerializeField] float VisionMinimumAwareness = 1f;
    [SerializeField] float VisionAwarenessBuildRate = 10f;

    [SerializeField] float HearingMinimumAwareness = 0f;
    [SerializeField] float HearingAwarenessBuildRate = 0.5f;

    [SerializeField] float ProximityMinimumAwareness = 0f;
    [SerializeField] float ProximityAwarenessBuildRate = 1f;

    [SerializeField] float AwarenessDecayDelay = 0.1f;
    [SerializeField] float AwarenessDecayRate = 0.1f;

    Dictionary<GameObject, TrackedTarget> Targets = new Dictionary<GameObject, TrackedTarget>();
    EnemyAI LinkedAI;

    public Dictionary<GameObject, TrackedTarget> ActiveTargets => Targets;

    // Start is called before the first frame update
    void Start()
    {
        LinkedAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> toCleanup = new List<GameObject>();
        foreach(var targetGO in Targets.Keys)
        {
            Debug.Log("Awareness: " + Targets[targetGO].Awareness);
            if (Targets[targetGO].DecayAwareness(AwarenessDecayDelay, AwarenessDecayRate * Time.deltaTime))
            {
                if (Targets[targetGO].Awareness <= 0f)
                {
                    LinkedAI.OnFullyLost();
                    toCleanup.Add(targetGO);
                }
                else
                {
                    if (Targets[targetGO].Awareness >= 1f)
                        LinkedAI.OnLostDetect(targetGO);
                    else
                        LinkedAI.OnLostSuspicion();
                }
            }
        }

        // cleanup targets that are no longer detected
        foreach(var target in toCleanup)
            Targets.Remove(target);
    }

    void UpdateAwareness(GameObject targetGO, DetectableTarget target, Vector3 position, float awareness, float minAwareness)
    {
        // not in targets
        if (!Targets.ContainsKey(targetGO))
            Targets[targetGO] = new TrackedTarget();

        // update target awareness
        if (Targets[targetGO].UpdateAwareness(target, position, awareness, minAwareness))
        {
            if (Targets[targetGO].Awareness >= 2f)
                LinkedAI.OnFullyDetected(targetGO);
            else if (Targets[targetGO].Awareness >= 1f)
                LinkedAI.OnDetected(targetGO);
            else if (Targets[targetGO].Awareness >= 0f)
                LinkedAI.OnSuspicious();
        }
    }

    public void ReportCanSee(DetectableTarget seen)
    {
        // determine where the target is in the field of view
        var vectorToTarget = (seen.transform.position - LinkedAI.EyeLocation).normalized;
        var dotProduct = Vector3.Dot(vectorToTarget, LinkedAI.EyeDirection);

        // determine the awareness contribution
        var awareness = VisionSensitivity.Evaluate(dotProduct) * VisionAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(seen.gameObject, seen, seen.transform.position, awareness, VisionMinimumAwareness);
    }

    public void ReportCanHear(GameObject source, Vector3 location, EHeardSoundCategory category, float intensity)
    {
        var awareness = intensity * HearingAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(source, null, location, awareness, HearingMinimumAwareness);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        var awareness = ProximityAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(target.gameObject, target, target.transform.position, awareness, ProximityMinimumAwareness);
    }    
}