using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera), typeof(Interactor))]
public class PlayerController : MonoBehaviour
{
    public MenuActions menuActions;
    public Animator animator;
    public float rotationSmoothing = 0.05f;
    public float rotationSmoothingVelocity;
    public float speed = 10;

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
            interactor.UpdateIconSprite(playerInput.devices[0].name, Interactor.buttons.okay);
        }
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        if (animator != null)
        {
            animator.SetBool("walking", true);
        }
    }

    private void OnLook(InputValue lookValue)
    {
        if (cameraController != null)
        {
            cameraController.lookVector = lookValue.Get<Vector2>();
        }
    }

    private void OnInteract(InputValue interactValue)
    {
        interactor.PerformInteraction();
        if (animator != null)
        {
            animator.SetBool("walking", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Collectible collectible = other.gameObject.GetComponent<Collectible>();
            if (collectible != null)
            {
                animator.SetTrigger("collect");
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

            //destroy
            Destroy(other.gameObject);
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
                animator.SetBool("walking", false);
            }
        }
    }
}
