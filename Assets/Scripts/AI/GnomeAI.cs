using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GnomeAI : MonoBehaviour
{
    private AudioSource[] audio;
    private Animator anim;

    private NavMeshAgent agent;

    public PlayerController playerController;
    public GameObject[] waypoints;
    public float remainingDistance;
    public float followDistance; // = 3f;
    public float detectionRadius; // = 10f;

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
        audio = GetComponents<AudioSource>();
        currWaypoint = -1;
        aiState = AIState.FollowPath;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerController != null && playerController.canMove)
        {
            switch (aiState)
            {
                case AIState.StealObjects:
                    anim.SetBool("isIdle", false);
                    anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
                    agent.stoppingDistance = 0f;
                    goToObject();
                    break;

                case AIState.ChasePlayer:
                    anim.SetBool("isIdle", false);
                    anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
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
                case AIState.Idle:
                    if (!anim.GetBool("isIdle"))
                        anim.SetFloat("vely", 0f);
                    {
                        anim.SetBool("isIdle", true);
                        // change state after 1 second
                        StartCoroutine(WaitAMomentThenGo());

                        var nearestObject = StealableObject.FindNearest(transform.position);
                        var playerDistance = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);


                        // if there is an item nearby, steal it
                        if (nearestObject != null)
                        {
                            aiState = AIState.StealObjects;
                            if (audio != null && audio.Length > 0)
                            {
                                audio[0].Stop();
                            }
                        }

                        // if there is a player nearby and the gnome hasn't already been following them, follow them
                        else if (playerDistance <= detectionRadius)
                        {
                            if (playerDistance > followDistance)
                            {
                                aiState = AIState.ChasePlayer;
                                if (audio != null && audio.Length > 0 && !audio[0].isPlaying)
                                {
                                    audio[0].Play();
                                }
                            }
                        }

                        // otherwise, go to nearest waypoint
                        else
                        {
                            setNextWaypoint();
                            aiState = AIState.FollowPath;
                            if (audio != null && audio.Length > 0 && !audio[0].isPlaying)
                            {
                                audio[0].Play();
                            }
                        }
                    }

                    break;
                case AIState.FollowPath:
                    anim.SetBool("isIdle", false);
                    anim.SetFloat("vely", agent.velocity.magnitude / agent.speed);
                    if (!agent.pathPending && agent.remainingDistance == 0)
                    {
                        aiState = AIState.Idle;
                        if (audio != null && audio.Length > 0)
                        {
                            audio[0].Stop();
                        }
                    }
                    else if (Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) <= detectionRadius)
                    {
                        aiState = AIState.ChasePlayer;
                        if (audio != null && audio.Length > 0 && !audio[0].isPlaying)
                        {
                            audio[0].Play();
                        }
                    }
                    break;
                default:
                    break;
            }
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
