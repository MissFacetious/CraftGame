using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class CollectibleEmitter : MonoBehaviour
{
    private bool hasItemsToDrop = true;
    private bool isReady = true;
    
    public GameObject collectible;
    
    [SerializeField]
    private int collectibleAmount = 3;

    private void Awake()
    {
        // Register interaction event
        Interactable interactable = GetComponent<Interactable>();
        if (interactable == null) {
            Debug.LogError("Interactable component not found!");
        }
        interactable.OnInteraction.AddListener(OnInteract);

        if (collectible == null)
        {
            Debug.LogError($"{name}: Must assign a collectible GameObject to be emitted.");
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
            for (int i = 0; i < collectibleAmount; i++)
            {
                GameObject item = Instantiate(collectible, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddForce(Vector3.up * 100f);
                }
            }

            hasItemsToDrop = false;
        }
    }
}
