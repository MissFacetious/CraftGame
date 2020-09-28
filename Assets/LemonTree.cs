using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class LemonTree : MonoBehaviour
{

    private bool hasItemsToDrop = true;
    private List<Transform> lemons;

    private void Awake()
    {
        // Register interaction event
        Interactable i = GetComponent<Interactable>();
        if (i != null) {
            i.OnInteraction.AddListener(OnInteract);
        }

        // Populate lemons List
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.name == "LoftLemon") {
                lemons.Add(child);
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
            GameObject lemon;
        }
    }
}
