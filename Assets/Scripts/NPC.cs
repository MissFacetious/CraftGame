using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class NPC : MonoBehaviour
{

    public Flowchart flowchart;

    private void Awake()
    {
        // Register interaction event.
        Interactable i = GetComponent<Interactable>();
        if (i != null)
        {
            i.OnInteraction.AddListener(OnInteract);
        }
    }

    public void OnInteract()
    {
        playDialogue();
    }

    private void playDialogue()
    {
        Debug.Log("Playing Dialogue.");
        flowchart.ExecuteBlock("Start");
    
    }
}
