using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class NPCAIw : MonoBehaviour
{

    private Animator anim;

    private UnityEngine.AI.NavMeshAgent agent;

    public GameObject[] waypoints;

    //public GameObject movingWaypoint;

    public int currWaypoint;

    public enum AIState
    {
        ChaseStationaryWaypoints,
        ChaseMovingWaypoint
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
        anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
        switch (aiState)
        {
            case AIState.ChaseStationaryWaypoints:
                //if (currWaypoint == waypoints.Length)
                //{
                //    aiState = AIState.ChaseMovingWaypoint;

                //    // initializes destination so agent.pathPending is true
                //    agent.SetDestination(movingWaypoint.transform.position);
                //}
                //else if (!agent.pathPending && agent.remainingDistance == 0)
                //{
                //    setNextWaypoint();
                //}
                if (!agent.pathPending && agent.remainingDistance == 0)
                {
                    setNextWaypoint();
                }
                break;
            case AIState.ChaseMovingWaypoint:
                //if (!agent.pathPending && agent.remainingDistance - agent.stoppingDistance < 0.3)
                //{
                //    aiState = AIState.ChaseStationaryWaypoints;
                //    currWaypoint = -1;
                //}
                //else
                //{
                //    float dist = (movingWaypoint.transform.position - agent.transform.position).magnitude;

                //    float lookAheadT = dist / agent.speed;

                //    Vector3 futureTarget = movingWaypoint.transform.position + lookAheadT * movingWaypoint.GetComponent<VelocityReporter>().velocity;
                //    agent.SetDestination(futureTarget);
                //}
                break;
            default:
                break;
        }

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
