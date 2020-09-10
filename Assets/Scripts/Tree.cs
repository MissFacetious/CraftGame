using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Tree : MonoBehaviour
{
    public GameObject Box;
    public GameObject Sphere;

    [SerializeField]
    private bool hasItemsToDrop = false;

    private void Awake()
    {
        // Register interaction event.
        Interactable i = GetComponent<Interactable>();
        if (i != null) {
            i.OnInteraction.AddListener(OnInteract);
        }
    }

    public void OnInteract()
    {
        Debug.Log(name + "'s interaction method");
        ShowerItems();
    }

    private void ShowerItems()
    {
        if (hasItemsToDrop)
        {
            GameObject branch;
            for (int i = 0; i < 10; i++)
            {
                branch = Instantiate(Sphere, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            }
            hasItemsToDrop = false;
        }
    }
}
