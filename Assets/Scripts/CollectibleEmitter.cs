using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class CollectibleEmitter : MonoBehaviour
{
    private bool hasItemsToDrop = true;
    private bool isReady = true;
    
    public GameObject collectible;
    public ParticleSystem particles;
    
    [SerializeField]
    private int collectibleAmount = 3;

    [SerializeField]
    private float emitterSpeed = 3f;
    private float emitterTimeElapsed = 0f;

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
            GameObject item;
            for (int i = 0; i < collectibleAmount; i++)
            {
                item = Instantiate(collectible, new Vector3(transform.position.x, transform.position.y+3f, transform.position.z), Quaternion.identity);
            }

            hasItemsToDrop = false;
        }
        if (particles)
        {
            particles.gameObject.SetActive(false);
        }
    }
}
