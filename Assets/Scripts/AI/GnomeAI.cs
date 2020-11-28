using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class GnomeAI : MonoBehaviour
{

    private Animator anim;

    private UnityEngine.AI.NavMeshAgent agent;

    public GameObject[] waypoints;
    public float remainingDistance;
    public float followDistance = 3f;

    public int currWaypoint;

    public enum AIState
    {
        StealObjects,
        ChasePlayer,
        Idle,
        FollowPath
    };

    public AIState aiState;

    void Start()
    {
        currWaypoint = -1;
        //aiState = AIState.StealObjects;
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //goToObject();
    }

    void Update()
    {
        anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
        switch (aiState)
        {
            case AIState.StealObjects:
                anim.SetBool("isIdle", false);
                agent.stoppingDistance = 0f;
                goToObject();
                break;

            case AIState.ChasePlayer:
                anim.SetBool("isIdle", false);
                if (StealableObject.FindNearest(transform.position) != null)
                {
                    aiState = AIState.StealObjects;
                }
                else
                {
                    agent.stoppingDistance = 3f;
                    goToPlayer();
                }
                break;
            case AIState.Idle: // when a waypoint or player is reached
                if (!anim.GetBool("isIdle"))
                {
                    anim.SetBool("isIdle", true);
                    // change state after 2 seconds
                    StartCoroutine(WaitAMomentThenGo());
                }
                
                break;
            case AIState.FollowPath:
                anim.SetBool("isIdle", false);
                anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
                if (!agent.pathPending && agent.remainingDistance == 0)
                {
                    //setNextWaypoint();
                    aiState = AIState.Idle;
                }

                break;
            default:
                break;
        }

    }

    private void goToObject()
    {
        var nearestObject = StealableObject.FindNearest(transform.position);
        if (nearestObject != null)
        {
            remainingDistance = agent.remainingDistance;
            agent.SetDestination(nearestObject.transform.position);
        }
        else
        {
            aiState = AIState.ChasePlayer;
        }
    }

    private void goToPlayer()
    {
        var distance = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);
        if (followDistance <= distance)
        {
            agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        }
        else
        {
            aiState = AIState.Idle;
            StartCoroutine(WaitAMomentThenGo());
        }
    }

    private IEnumerator WaitAMomentThenGo()
    {
        float time = Random.Range(1f, 10f);
        yield return new WaitForSeconds(time);
        // if there is an item nearby, steal it
        var nearestObject = StealableObject.FindNearest(transform.position);
        var playerDistance = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);


        if (nearestObject != null)
        {
            aiState = AIState.StealObjects;
        }

        // if there is a player nearby, follow them
        else if (followDistance <= playerDistance) {
            aiState = AIState.ChasePlayer;
        }

        // otherwise, go to nearest waypoint
        else
        {
            aiState = AIState.FollowPath;
            setNextWaypoint();

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
