using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteraction;

    public bool isReady { get; private set; }
    public bool withinRange { get; private set; }

    private Vector3 interactionVector;
    private Transform player;

    [SerializeField, Range(1f, 100f)]
    private float interactRadius = 3f;
    private bool isFocused = false;

    private void Awake()
    {
        isReady = true;
        // Use default transform if no custom is specified
        interactionVector = transform.position;
    }

    public void Interact()
    {
        //Debug.Log("Base interaction method.");
        if (OnInteraction != null)
        {
            if (isReady) // && withinRange
            {
                isReady = false;
                OnInteraction.Invoke();
            }
        }
    }

    //need this because Fungus CallMethod does not support parameters in function calls.
    public void MakeReady()
    {
        isReady = true;
    }

    public void SetReady(bool value)
    {
        isReady = value;
    }

    private void Update()
    {
        if (isFocused)
        {
            float dist = Vector3.Distance(player.position, interactionVector);

            if (dist <= interactRadius)
            {
                // within interactable range
                withinRange = true;
            }
            else
            {
                withinRange = false;
            }
        }
    }

    public void OnFocused(Transform interactorTransform)
    {
        isFocused = true;
        player = interactorTransform;
    }

    public void OnUnfocused()
    {
        isFocused = false;
        player = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionVector, interactRadius);
    }
}
