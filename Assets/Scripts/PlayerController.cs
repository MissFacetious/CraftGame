using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera), typeof(Interactor))]
public class PlayerController : MonoBehaviour
{

    public enum inputControls
    {
        Keyboard,
        XInputControllerWindows,
        DualShock4GamepadHID
    }

    public static inputControls currentControls;

    public MenuActions menuActions;
    public Animator animator;
    public float rotationSmoothing = 0.05f;
    public float groundDistanceMargin = 0.3f;
    public float rotationSmoothingVelocity;
    public float speed = 6;

    private bool canMove = true;
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isFlying = false;

    [SerializeField]
    private Camera playerCamera;
    private CameraController cameraController;
    private Interactor interactor;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private InventoryManager inventoryManager;
    private bool addJumpForce = false;
    
    private float movementX;
    private float movementY;

    private Collider playerCollider;

    public float rotationSpeed;

    private void Awake()
    {
        //controls = new CraftGame();
        
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

    private void OnEnable()
    {
        //controls.Player.Enable();
    }
    private void OnDisable()
    {
        //controls.Player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
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
            if (currentControls == inputControls.Keyboard)
            {
                currentControls = interactor.UpdateIcons(playerInput.devices[lastPluggedIn].name);
                interactor.UpdateIconSprite(currentControls.ToString(), Interactor.buttons.okay);
            }
        }
        menuActions.OnControlsChanged();
    }

    // Adding resetTrigger appears to resolve the floaty walking from idle. Can clean it up later.
    private void OnMove(InputValue movementValue)
    {
        if (canMove)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            if (isFlying)
            {
                speed = 4;
            }
            else if (!isJumping && !isFlying)
            {
                if (isRunning) // starting to run
                {
                    speed = 12;
                    if (animator != null)
                    {
                        animator.SetTrigger("running");
                    }
                }
                else if (isJumping) // starting to jump
                {
                    if (animator != null)
                    {
                        animator.ResetTrigger("idle");
                        animator.ResetTrigger("walking");
                        animator.ResetTrigger("flying");
                        animator.SetTrigger("jumping");
                    }
                }
                else // just walking
                {
                    speed = 6;
                    if (animator != null)
                    {
                        animator.ResetTrigger("idle");
                        animator.ResetTrigger("jumping");
                        animator.ResetTrigger("flying");
                        animator.SetTrigger("walking");
                    }
                }
            }
        }
    }

    private void OnLook(InputValue lookValue)
    {
        if (canMove)
        {
            if (cameraController != null)
            {
                cameraController.lookVector = lookValue.Get<Vector2>();
            }
        }
    }

    public float jumpHeight = 9f;
    private void OnJump(InputValue inputValue)
    {
        if (canMove && !isJumping && !isFlying)
        {
            addJumpForce = true;
            animator.ResetTrigger("idle");
            animator.ResetTrigger("walking");
            animator.ResetTrigger("flying");
            animator.SetTrigger("jumping");
        }
    }

    private void OnRun(InputValue inputValue)
    {
        if (canMove)
        {
            if (inputValue.Get().Equals((System.Single)1)) // only way I can figure out pressing button down/up
            {
                isRunning = true;
                if (isJumping && !isFlying)
                {
                    animator.ResetTrigger("jumping");
                    animator.ResetTrigger("walking");
                    animator.ResetTrigger("running");
                    animator.SetTrigger("flying");
                }
                else if (IsGrounded())
                {
                    animator.SetTrigger("running");
                }
            }
            else
            {
                isRunning = false;
            }
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
        if (addJumpForce && IsGrounded())
        {
            rb.AddForce(Vector3.up * 600, ForceMode.Impulse);
            isJumping = true;
        }
        else if (!Mathf.Approximately(rb.velocity.y, 0) && rb.velocity.y < 0)
        {
            if (!IsGrounded())
            {
                rb.AddForce(Vector3.down * 250);
            }
        }

        addJumpForce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Dialog") != null ||
            GameObject.FindGameObjectWithTag("Selection") != null ||
            GameObject.FindGameObjectWithTag("Menu") != null)
        {
            // make sure we are in idle
            canMove = false;
            isRunning = false;
            isJumping = false;
            isFlying = false;

            movementX = 0;
            movementY = 0;

            animator.SetTrigger("idle");
        }
        else
        {
            canMove = true;
        }
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
            }
            else if ((Mathf.Approximately(rb.velocity.x, 0)) && (Mathf.Approximately(rb.velocity.y, 0)) && (Mathf.Approximately(rb.velocity.z, 0)))
            {
                if (IsGrounded())
                { 
                   animator.SetTrigger("idle");
                }
            }
        }
    }

    // Check if the player is contacting the ground, or within the ground distance margin
    private bool IsGrounded()
    {
        if (Physics.Raycast(playerCollider.bounds.center, Vector3.down, out RaycastHit hitInfo, playerCollider.bounds.extents.y + groundDistanceMargin))
        {
            //has collided
            if (isJumping || isFlying) 
            {
                animator.ResetTrigger("jumping");
                animator.ResetTrigger("flying");
                animator.SetTrigger("idle");

                isJumping = false;
                isFlying = false;
            }
            return true;
        }
        return false;
    }
}
