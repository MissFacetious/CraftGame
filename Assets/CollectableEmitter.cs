using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class CollectableEmitter : MonoBehaviour
{
    private bool hasItemsToDrop = true;
    private bool isReady = true;
    
    public GameObject collectable;
    
    [SerializeField]
    private int collectableAmount = 3;

    [SerializeField]
    private float emitDistance = 5f;

    private void Awake()
    {
        // Register interaction event
        Interactable interactable = GetComponent<Interactable>();
        if (interactable == null) {
            Debug.LogError("Interactable component not found!");
        }
        interactable.OnInteraction.AddListener(OnInteract);

        if (collectable == null)
        {
            Debug.LogError("Must assign a GameObject to be emitted.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract()
    {
        // emit items
        if (isReady)
        {
            //Debug.Log($"{name}'s EmitItems()");
            EmitItems();
        }
    }

    private void EmitItems()
    {
         if (hasItemsToDrop)
        {
            for (int i = 0; i < collectableAmount; i++)
            {
                GameObject item = Instantiate(collectable, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                Rigidbody rb = item.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * emitDistance);
            }

            hasItemsToDrop = false;
        }
    }
}
