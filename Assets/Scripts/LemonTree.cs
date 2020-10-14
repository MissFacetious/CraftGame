using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class LemonTree : MonoBehaviour
{
    private bool hasItemsToDrop = true;
    private bool isReady = true;
    private Transform[] lemons;
    private List<Transform> lst;
    private Stack<Transform> stk;

    private void Awake()
    {
        // Register interaction event
        Interactable interactable = GetComponent<Interactable>();
        if (interactable == null) {
            Debug.LogError("Interactable component not found!");
        }
        interactable.OnInteraction.AddListener(OnInteract);

        // Populate lemons List
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.name != "LoftyLemon") {
                stk.Push(child);
            }
        }
    }

    public void OnInteract()
    {
        DropLemon();
    }

    private void DropLemon()
    {
        if (hasItemsToDrop)
        {
            Transform lemon = stk.Pop();
            // lemon.FallToTheGround()

            if (stk.Count == 0)
            {
                hasItemsToDrop = false;
            }
        }
    }
}
