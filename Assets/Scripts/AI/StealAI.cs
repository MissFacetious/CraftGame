using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class StealAI : MonoBehaviour
{
    private AudioSource[] audio;
    private Animator anim;

    private NavMeshAgent agent;

    public PlayerController playerController;
    public GameObject[] waypoints;
    public float remainingDistance;
    public float followDistance = 3f;

    private float CloseEnoughToTarget = 3f;
    private float timer;
    private float wanderRadius;
    private float maxStateTime = 10f;
    private Vector3 roamToLocation;
    public int currWaypoint;

    public enum AIState
    {
        StealObjects,
        Roam,
        ChasePlayer
    };

    public AIState aiState;

    void Start ()
    {
        audio = GetComponents<AudioSource>();
        currWaypoint = -1;
        CloseEnoughToTarget = 3f;
        aiState = AIState.StealObjects;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 3f;
        roamToLocation = RandomMovement(agent.transform.position, 10f);
        goToObject();
    }

    void Update()
    {
        if (playerController != null && playerController.canMove)
        {
            timer += Time.deltaTime;

            checkChangeStateTimer();

            anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);

            switch (aiState)
            {
                case AIState.StealObjects:
                    goToObject();
                    break;

                case AIState.Roam:
                    if (StealableObject.FindNearest(transform.position) != null)
                    {
                        aiState = AIState.StealObjects;
                    }
                    else
                    {
                        goToRandom(10f);
                    }
                    break;

                case AIState.ChasePlayer:
                    if (StealableObject.FindNearest(transform.position) != null)
                    {
                        aiState = AIState.StealObjects;
                    }
                    else
                    {
                        goToPlayer();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    // If a collectable is on the map, seek it out.
    private void goToObject()
    {
        agent.stoppingDistance = 0f;

        var nearestObject = StealableObject.FindNearest(transform.position);
        
        anim.enabled = true;

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
        agent.stoppingDistance = 3f;

        if (followDistance <= Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position))
        {
            anim.enabled = true;
            if (audio != null && audio.Length > 0 && !audio[0].isPlaying)
            {
                audio[0].Play();
            }
            agent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        }
        else
        {
            anim.enabled = false;
            if (audio != null && audio.Length > 0)
            {
                audio[0].Stop();
            }
        }
    }

    private void goToRandom(float radius)
    {

        anim.enabled = true;
        
        // Pick another random location
        if (Vector3.Distance(transform.position, roamToLocation) <= CloseEnoughToTarget)
        {
            roamToLocation = RandomMovement(transform.position, radius);
        }

        agent.SetDestination(roamToLocation);
    }

    // Find a location on the nav mesh within a radius of the agent
    public Vector3 RandomMovement(Vector3 position, float radius)
    {
        NavMesh.SamplePosition(Random.insideUnitSphere * radius + position, out NavMeshHit hit, radius, -1);

        return hit.position;
    }

    private void checkChangeStateTimer()
    {
        if (timer >= maxStateTime && aiState == AIState.ChasePlayer)
        {
            roamToLocation = RandomMovement(transform.position, 20f);
            aiState = AIState.Roam;
            timer = 0f;
        }
        else if (timer >= maxStateTime && aiState == AIState.Roam)
        {
            aiState = AIState.ChasePlayer;
            timer = 0f;
        }
    }
}
