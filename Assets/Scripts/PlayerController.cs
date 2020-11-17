using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Interactor))]
public class PlayerController : MonoBehaviour
{

    public enum controls
    {
        Keyboard,
        XInputControllerWindows,
        DualShock4GamepadHID
    }

    const float WALK_SPEED = 6f;
    const float RUN_SPEED = 12f;

    public static controls currentControls;
    public MenuActions menuActions;
    public Animator animator;
    public float speed = 6;
    private bool running = false;
    private bool jumping = false;

    private Transform playerCamera;
    private Interactor interactor;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private InventoryManager inventoryManager;
    private Vector2 movementInput;

    private bool canMove = true;

    private void Awake()
    {
        playerCamera = Camera.main.transform;
        if (playerCamera == null)
        {
            Debug.LogError("Camera not found.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator not found!");
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
            if (currentControls == controls.Keyboard)
            {
                currentControls = interactor.UpdateIcons(playerInput.devices[lastPluggedIn].name);
                interactor.UpdateIconSprite(currentControls.ToString(), Interactor.buttons.okay);
            }
        }
        menuActions.OnControlsChanged();
    }

    private void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if (canMove)
        {
            Debug.Log("hit jump");
            Debug.Log(inputValue);
            jumping = true;
        }
    }

    private void OnRun(InputValue inputValue)
    {
        if (canMove)
        {
            if (inputValue.isPressed)
            {
                running = true;
            }
            else
            {
                running = false;
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

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Dialog") != null ||
            GameObject.FindGameObjectWithTag("Selection") != null ||
            GameObject.FindGameObjectWithTag("Menu") != null)
        {
            // make sure we are in idle
            canMove = false;
            running = false;
            jumping = false;

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
            Vector3 playerMovement = new Vector3(movementInput.x, 0f, movementInput.y);
            playerMovement = playerCamera.forward * playerMovement.z + playerCamera.right * playerMovement.x;
            if (playerMovement.magnitude >= 0.1f)
            {
                const float turnSpeed = 3f;
                float targetAngle = Mathf.Atan2(movementInput.x, movementInput.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                Quaternion rot = Quaternion.Euler(0f, targetAngle, 0f);

                Animate();
                rb.MovePosition(rb.position + playerMovement * Time.deltaTime * speed);
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, rot, Time.deltaTime * turnSpeed));
            }
            else
            {
                animator.SetTrigger("idle");
            }
        }
    }

    void Animate()
    {
        if (running) {
            animator.SetTrigger("running");
            speed = RUN_SPEED;
        }
        else if (jumping)
        {
            animator.SetTrigger("jumping");
            speed = 0;
        }
        else
        {
            animator.SetTrigger("walking");
            speed = WALK_SPEED;
        }
    }
}
