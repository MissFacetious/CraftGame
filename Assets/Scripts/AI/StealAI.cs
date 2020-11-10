using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class StealAI : MonoBehaviour
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
        ChasePlayer
    };

    public AIState aiState;

    void Start ()
    {
        currWaypoint = -1;
        aiState = AIState.StealObjects;
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        goToObject();
    }

    void Update()
    {
        anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
        switch (aiState)
        {
            case AIState.StealObjects:
                agent.stoppingDistance = 0f;
                goToObject();
                break;

            case AIState.ChasePlayer:
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
    }
}
