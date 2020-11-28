using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NPCAI : MonoBehaviour
{

    private Animator anim;

    private UnityEngine.AI.NavMeshAgent agent;

    public GameObject[] waypoints;

    //public GameObject movingWaypoint;

    public int currWaypoint;

    public enum AIState
    {
        ChaseStationaryWaypoints,
        ChaseMovingWaypoint,
        StopAndTalk
    };

    public AIState aiState;

    // Use this for initialization
    void Start ()
    {
        currWaypoint = -1;
        aiState = AIState.ChaseStationaryWaypoints;
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        setNextWaypoint();
    }

    // TODO: update with more states for gnome AI
    void Update()
    {
        
        switch (aiState)
        {
            case AIState.ChaseStationaryWaypoints:
                anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
                if (!agent.pathPending && agent.remainingDistance == 0)
                {
                    setNextWaypoint();
                }
                break;
            case AIState.ChaseMovingWaypoint:
                break;
            case AIState.StopAndTalk:
                break;
            default:
                break;
        }

    }

    public void setNpcToStopAndTalk()
    {
        aiState = AIState.StopAndTalk;
        agent.isStopped = true;
    }

    public void setNpcToChaseWaypoints()
    {
        aiState = AIState.ChaseStationaryWaypoints;
        agent.isStopped = false;
    }

    private void setNextWaypoint()
    {
        currWaypoint++;
        if (currWaypoint == waypoints.Length)
        {
            currWaypoint = 0;
        }
        if (waypoints.Length > 0 && currWaypoint < waypoints.Length)
        {
            agent.SetDestination(waypoints[currWaypoint].transform.position);
        }
    }
}
