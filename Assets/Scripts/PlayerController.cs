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
    private CharacterController controller;
    private Interactor interactor;
    private PlayerInput playerInput;
    private Rigidbody rb;
    private InventoryManager inventoryManager;

    private bool canMove = true;
    private Vector2 movementVector;

    private void Awake()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Camera not found.");
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
            //Debug.Log(playerInput.devices[0].name);
            interactor.UpdateIconSprite(playerInput.devices[0].name);
        }
    }

    private void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
        animator.SetBool("walking", true);
    }

    private void OnLook(InputValue lookValue)
    {
        cameraController.lookVector = lookValue.Get<Vector2>();
    }

    private void OnJump()
    {
        rb.AddForce(new Vector3(rb.velocity.x, 3f, rb.velocity.z));
    }

    private void OnInteract(InputValue interactValue)
    {
        interactor.PerformInteraction();
        animator.SetBool("walking", false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Apple"))
        {
            Apple apple = other.gameObject.GetComponent<Apple>();

            if (apple != null)
            {
                // kick off collection animations
                // this animation seems to slow down the game
                animator.SetTrigger("collect");
                apple.Collect(gameObject);
                menuActions.increaseCurrentCounter();
                inventoryManager.CreateNewItem(Recipes.RecipeEnum.GOLDEN_APPLE, false);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Collectible"))
        {
            Collectible collectible = other.gameObject.GetComponent<Collectible>();
            if (collectible != null)
            {
                animator.SetTrigger("collect");
                collectible.Collect(gameObject);
                menuActions.increaseCurrentCounter();
                inventoryManager.CreateNewItem(collectible.recipe, false);
            }
            else
            {
                Destroy(other.gameObject);
            }
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
            Vector3 playerMovement = new Vector3(movementVector.x, 0f, movementVector.y);
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
