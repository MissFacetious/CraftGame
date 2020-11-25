using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    public UnityEvent OnInteraction;

    public bool isReady { get; private set; }
    public bool withinRange { get; private set; }

    [SerializeField]
    private Transform interactionTransform;
    private Transform player;

    [SerializeField, Range(1f, 100f)]
    private float interactRadius = 3f;
    private bool isFocused = false;

    private AudioSource audio;

    private void Awake()
    {
        isReady = true;
        // Use default transform if no custom is provided
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }
        audio = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (OnInteraction != null)
        {
            if (isReady) // && withinRange
            {
                isReady = false;
                if (interactionTransform.gameObject.tag == "Interactable")
                {
                    player.gameObject.GetComponent<PlayerController>().animator.SetTrigger("interact");
                    if (audio != null)
                    {
                        audio.Play();
                    }
                }
                OnInteraction.Invoke();
            }
        }
    }

    //need this because Fungus CallMethod does not support parameters in function calls.
    public void MakeReady()
    {
       // Debug.Log("Resetting interactable to ready.");
        isReady = true;
    }

    public void SetReady(bool value)
    {
       // Debug.Log("Setting ready to:" + value);
        isReady = value;
    }

    private void Update()
    {
        if (isFocused)
        {
            float dist = Vector3.Distance(player.position, interactionTransform.position);
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
        if (isReady)
        {
            isFocused = true;
            player = interactorTransform;
        }
    }

    public void OnUnfocused()
    {
        isFocused = false;
        player = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interactRadius);
    }
}
