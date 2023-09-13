using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EOffmeshLinkStatus
{
    NotStarted,
    InProgress
}

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterAgent : CharacterBase
{
    [SerializeField] float NearestPointSearchRange = 5f;

    NavMeshAgent Agent;
    bool DestinationSet = false;
    bool ReachedDestination = false;
    EOffmeshLinkStatus OffMeshLinkStatus = EOffmeshLinkStatus.NotStarted;

    public bool IsMoving => Agent.velocity.magnitude > float.Epsilon;
    public bool IsRunning = false;
    public bool isWalking = false;

    public bool AtDestination => ReachedDestination;

    protected void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    protected void Update()
    {
        if (!Agent.pathPending && !Agent.isOnOffMeshLink && DestinationSet && (Agent.remainingDistance <= Agent.stoppingDistance))
        {
            DestinationSet = false;
            ReachedDestination = true;
        }

        if (Agent.isOnOffMeshLink)
        {
            if (OffMeshLinkStatus == EOffmeshLinkStatus.NotStarted)
                StartCoroutine(FollowOffmeshLink());
        }
    }

    /*
    The FollowOffmeshLink coroutine handles the traversal of the OffMeshLink. It sets the OffMeshLinkStatus to "InProgress" and disables the position, rotation, and up axis updates of the NavMeshAgent. 
    It then moves the character towards the end position of the OffMeshLink until it reaches it. 
    Once the traversal is complete, the OffMeshLinkStatus is set back to "NotStarted" and the NavMeshAgent updates are re-enabled.
    */

    IEnumerator FollowOffmeshLink()
    {
        OffMeshLinkStatus = EOffmeshLinkStatus.InProgress;
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        Vector3 newPosition = transform.position;
        while (!Mathf.Approximately(Vector3.Distance(newPosition, Agent.currentOffMeshLinkData.endPos), 0f))
        {
            newPosition = Vector3.MoveTowards(transform.position, Agent.currentOffMeshLinkData.endPos, Agent.speed * Time.deltaTime);
            transform.position = newPosition;

            yield return new WaitForEndOfFrame();
        }

        OffMeshLinkStatus = EOffmeshLinkStatus.NotStarted;
        Agent.CompleteOffMeshLink();

        Agent.updatePosition = true;
        Agent.updateRotation = true;
        Agent.updateUpAxis = true;    
    }

    /*
    The PickLocationInRange method is used to randomly pick a location within a specified range around the character's current position. 
    It adds random offsets to the forward and right vectors of the character's position and samples the NavMesh to find the nearest valid position.
    */

    public Vector3 PickLocationInRange(float range)
    {
        Vector3 searchLocation = transform.position;
        searchLocation += Random.Range(-range, range) * Vector3.forward;
        searchLocation += Random.Range(-range, range) * Vector3.right;

        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(searchLocation, out hitResult, NearestPointSearchRange, NavMesh.AllAreas))
            return hitResult.position;

        return transform.position;
    }

    protected virtual void CancelCurrentCommand()
    {
        Agent.ResetPath();

        DestinationSet = false;
        ReachedDestination = false;
        OffMeshLinkStatus = EOffmeshLinkStatus.NotStarted;
    }

    public virtual void MoveTo(Vector3 destination)
    {
        CancelCurrentCommand();

        SetDestination(destination);
    }

    public virtual void SetDestination(Vector3 destination)
    {
        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(destination, out hitResult, NearestPointSearchRange, NavMesh.AllAreas))
        {
            Agent.SetDestination(hitResult.position);
            DestinationSet = true;
            ReachedDestination = false;
        }
    }
}