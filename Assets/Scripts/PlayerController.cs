using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera), typeof(Interactor))]
public class PlayerController : MonoBehaviour
{
    public MenuActions menuActions;
    public Animator animator;
    public float rotationSmoothing = 0.05f;
    public float rotationSmoothingVelocity;
    public float speed = 6;
    private bool running = false;
    private bool jumping = false;

    [SerializeField]
    private Camera playerCamera;
    private CameraController cameraController;
    private Interactor interactor;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private InventoryManager inventoryManager;

    private bool canMove = true;
    private float movementX;
    private float movementY;

    private void Awake()
    {
        if (playerCamera == null)
        {
            Debug.Log("Camera not found.");
        }
        if (inventoryManager == null)
        {
            getInventoryManager();
        }

        // using the Scene Name in MenuActions, 
        // we should be able to place the character in the right spot based on 
        // where they currently are, and where they've currently been

    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        interactor = GetComponent<Interactor>();
        cameraController = playerCamera.GetComponent<CameraController>();
        // change sprite controller on start
        OnControlsChanged();
    }

    void getInventoryManager()
    {
        if (GameObject.FindGameObjectWithTag("InventoryManager") != null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }
        else
        {
            Debug.Log("InventoryManager is not hooked up.");
        }
    }

    private void OnControlsChanged()
    {
        if (playerInput.devices.Count > 0)
        {
            int lastPluggedIn = playerInput.devices.Count - 1;
            interactor.UpdateIconSprite(playerInput.devices[lastPluggedIn].name, Interactor.buttons.okay);
        }
        menuActions.OnControlsChanged();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        if (running)
        {
            speed = 12;
            if (animator != null)
            {
                animator.SetTrigger("running");
            }
        }
        else if (jumping)
        {
            speed = 0;
             if (animator != null)
            {
                animator.SetTrigger("jumping");
            }
        }
        else // just walking
        {
            speed = 6;
            if (animator != null)
            {
                animator.SetTrigger("walking");
            }
        }
    }

    private void OnLook(InputValue lookValue)
    {
        if (cameraController != null)
        {
            cameraController.lookVector = lookValue.Get<Vector2>();
        }
    }

    private void OnJump(InputValue inputValue)
    {
        Debug.Log("hit jump");
        Debug.Log(inputValue);
        jumping = true;
    }

    private void OnRun(InputValue inputValue)
    {
        if (inputValue.Get().Equals((System.Single)1)) // only way I can figure out pressing button down/up
        {
            running = true;
        }
        else
        {
            running = false;
        }
    }

    private void OnCancel(InputValue inputValue)
    {
        menuActions.OnCancel(inputValue);
    }

    private void OnMenu(InputValue inputValue)
    {
        menuActions.OnControlsChanged();
        menuActions.OnMenu(inputValue);
    }

    private void OnInteract(InputValue interactValue)
    {
        if (interactor != null)
        {
            interactor.PerformInteraction();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Collectible collectible = other.gameObject.GetComponent<Collectible>();
            if (collectible != null)
            {
                collectible.Collect(gameObject);
                menuActions.increaseCurrentCounter();
                if (inventoryManager)
                {
                    inventoryManager.CreateNewItem(collectible.recipe, false);
                }
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("ExtraTime"))
        {
            //increase time
            menuActions.addTime(1.0f);

            //play the particle system.
            other.gameObject.GetComponent<TimerCrystal>().collectMe();

            //destroy
            //Destroy(other.gameObject);
        }
    }

    void FixedUpdate()
    {
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //rb.AddForce(movement * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryManager == null)
        {
            getInventoryManager();
        }
        if (canMove)
        {
            Vector3 playerMovement = new Vector3(movementX, 0f, movementY);
            if (playerMovement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(playerMovement.x, playerMovement.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
                float smoothedRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothingVelocity, rotationSmoothing);

                Vector3 movementDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rb.MovePosition(rb.position + movementDir * Time.deltaTime * speed);
                rb.MoveRotation(Quaternion.Euler(0f, smoothedRotationAngle, 0f));
                //transform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
                //transform.Translate(movementDir * speed * Time.deltaTime, Space.World);
            }
            else
            {
                animator.SetTrigger("idle");
            }
        }
    }
}
