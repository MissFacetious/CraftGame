using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Camera), typeof(Interactor))]
public class PlayerController : MonoBehaviour
{

    public enum inputDevice
    {
        Keyboard,
        XInputControllerWindows,
        DualShock4GamepadHID
    }

    public enum inputMovement
    {
        idle, // 0
        walking, // 1
        running, // 2
        jumping, // 3
        flying // 4
    }

    CraftGame controls = null;
    public static inputDevice currentControls;
    public static inputMovement currentMovement;

    public MenuActions menuActions;
    public Animator animator;
    public float rotationSmoothing = 0.05f;
    public float groundDistanceMargin = 0.3f;
    public float rotationSmoothingVelocity;
    public float speed = 6;

    private bool canMove = true;

    //private bool isRunning = false;
    //private bool isJumping = false;
    //private bool isFlying = false;

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
        controls = new CraftGame();
        
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
        controls.Player.Enable();
        //controls.Player.Attack.performed += context => OnAttackPerformed(context); // Bind a function to the attack performed event.
        //controls.Player.Attack.canceled += context => OnAttackCanceled(context); // Bind a function to the attack canceled event.

        controls.Player.Move.started += context => OnMoveEnter(context); 
        controls.Player.Move.performed += context => OnMoving(context); 
        controls.Player.Move.canceled += context => OnMoveExit(context);

        controls.Player.Run.started += context => OnRunEnter(context);
        controls.Player.Run.performed += context => OnRunning(context);
        controls.Player.Run.canceled += context => OnRunExit(context);

        controls.Player.Jump.started += context => OnJumpEnter(context);
        controls.Player.Jump.performed += context => OnJumping(context);
        controls.Player.Jump.canceled += context => OnJumpExit(context);

    }
    private void OnDisable()
    {
        controls.Player.Disable();
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
            if (currentControls == inputDevice.Keyboard)
            {
                currentControls = interactor.UpdateIcons(playerInput.devices[lastPluggedIn].name);
                interactor.UpdateIconSprite(currentControls.ToString(), Interactor.buttons.okay);
            }
        }
        menuActions.OnControlsChanged();
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



    private void OnMoveEnter(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            if (currentMovement == inputMovement.jumping)
            {
                speed = 4;
                currentMovement = inputMovement.flying;
                Animate();
            }
            else if (IsGrounded())
            {
                if (currentMovement == inputMovement.running) // starting to run
                {
                    speed = 12;
                    currentMovement = inputMovement.running;
                    Animate();
                }
                else // just walking
                {
                    speed = 6;
                    currentMovement = inputMovement.walking;
                    Animate();
                }
            }
        }
    }

    private void OnMoving(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            if (IsGrounded())
            {
                if (currentMovement == inputMovement.running) // starting to run
                {
                    speed = 12;
                    currentMovement = inputMovement.running;
                    Animate();
                }
                else // just walking
                {
                    speed = 6;
                    currentMovement = inputMovement.walking;
                    Animate();
                }
            }
        }
    }

    private void OnMoveExit(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            Vector2 movementVector = context.ReadValue<Vector2>();
            movementX = movementVector.x;
            movementY = movementVector.y;
            if (IsGrounded())
            {
                // just stopped
                currentMovement = inputMovement.idle;
                Animate();
                
            }
        }
    }

    private void OnRunEnter(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            if (currentMovement == inputMovement.jumping)
            {
                currentMovement = inputMovement.flying;
                Animate();
            }
            else if (IsGrounded() && currentMovement == inputMovement.walking)
            {
                currentMovement = inputMovement.running;
                Animate();
            }
            else
            {
                currentMovement = inputMovement.idle;
                Animate();
            }
        }
    }

    private void OnRunning(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            if (currentMovement == inputMovement.flying)
            {
                // you are flying
            }
            else if (currentMovement == inputMovement.running) {
                // already running
            }
            else if (IsGrounded() && currentMovement == inputMovement.walking)
            {
                currentMovement = inputMovement.running;
                Animate();
            }
            else
            {
                currentMovement = inputMovement.idle;
                Animate();
            }
        }
    }

    private void OnRunExit(InputAction.CallbackContext context)
    {
        // done running
        if (IsGrounded() && currentMovement != inputMovement.jumping && currentMovement != inputMovement.flying) {
            if (currentMovement != inputMovement.walking)
            {
                currentMovement = inputMovement.idle;
                Animate();
            }
        }
    }

    private void OnJumpEnter(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            addJumpForce = true;
            currentMovement = inputMovement.jumping;
            Animate();
        }
    }

    private void OnJumping(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            addJumpForce = true;
            currentMovement = inputMovement.jumping;
            Animate();
        }
    }

    private void OnJumpExit(InputAction.CallbackContext context)
    {

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

    void Animate()
    {
        if (animator != null)
        {
            if (currentMovement == inputMovement.flying)
            {
                animator.ResetTrigger("jumping");
                animator.ResetTrigger("walking");
                animator.ResetTrigger("running");
                animator.SetTrigger("flying");
            }
            else if (currentMovement == inputMovement.jumping)
            {
                animator.ResetTrigger("flying");
                animator.ResetTrigger("walking");
                animator.ResetTrigger("running");
                animator.SetTrigger("jumping");
            }
            else if (currentMovement == inputMovement.running)
            {
                animator.ResetTrigger("jumping");
                animator.ResetTrigger("walking");
                animator.ResetTrigger("flying");
                animator.SetTrigger("running");
            }
            else if (currentMovement == inputMovement.walking)
            {
                animator.ResetTrigger("jumping");
                animator.ResetTrigger("flying");
                animator.ResetTrigger("running");
                animator.SetTrigger("walking");
            }
            else if (currentMovement == inputMovement.idle)
            {
                animator.ResetTrigger("jumping");
                animator.ResetTrigger("walking");
                animator.ResetTrigger("running");
                animator.SetTrigger("idle");
            }
        }
    }

    void FixedUpdate()
    {
        GameObject.Find("action").GetComponent<Text>().text = currentMovement.ToString();

        if (GameObject.FindGameObjectWithTag("Dialog") != null ||
            GameObject.FindGameObjectWithTag("Selection") != null ||
            GameObject.FindGameObjectWithTag("Menu") != null)
        {
            // make sure we are in idle
            canMove = false;
            currentMovement = inputMovement.idle;

            movementX = 0;
            movementY = 0;

            Animate();
        }
        else
        {
            canMove = true;
        }
        if (inventoryManager == null)
        {
            getInventoryManager();
        }

        if (addJumpForce && IsGrounded())
        {
            rb.AddForce(Vector3.up * 600, ForceMode.Impulse);
        }
        else if (!Mathf.Approximately(rb.velocity.y, 0) && rb.velocity.y < 0)
        {
            if (!IsGrounded())
            {
                if (currentMovement == inputMovement.flying)
                {
                    rb.AddForce(Vector3.down * 0.5f);
                }
                else {
                    rb.AddForce(Vector3.down * 250f);
                }
            }
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
            else if ((Mathf.Approximately(rb.velocity.x, 0)) && (Mathf.Approximately(rb.velocity.y, 0)) && (Mathf.Approximately(rb.velocity.z, 0))
                && IsGrounded())
            {
                if (!addJumpForce)
                {
                    currentMovement = inputMovement.idle;
                    Animate();
                }
            }
        }

        addJumpForce = false;
    }

    // Check if the player is contacting the ground, or within the ground distance margin
    private bool IsGrounded()
    {
        if (Physics.Raycast(playerCollider.bounds.center, Vector3.down, out RaycastHit hitInfo, playerCollider.bounds.extents.y + groundDistanceMargin))
        {
            //has collided
            return true;
        }
        return false;
    }
}
