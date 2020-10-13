using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    private Rigidbody rigidBody;

    private bool canMove = true;
    private float movementX;
    private float movementY;

    private void Awake()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Camera not found.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        interactor = GetComponent<Interactor>();
        cameraController = playerCamera.GetComponent<CameraController>();
        // change sprite controller on start
        OnControlsChanged();
    }

    private void OnControlsChanged()
    { 
        if (playerInput.devices.Count > 0)
        {
            //Debug.Log(playerInput.devices[0].name);
            InputDevice dev = playerInput.devices[0];
            interactor.UpdateIconSprite(dev.name);
        }
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        animator.SetBool("walking", true);
    }

    private void OnLook(InputValue lookValue)
    {
        cameraController.lookVector = lookValue.Get<Vector2>();
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
                Debug.Log("apple");
                // kick off collection animations
                // this animation seems to slow down the game
                animator.SetTrigger("collect");
                apple.Collect(gameObject);
                menuActions.increaseCurrentCounter();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            // ++groundContact
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // --groundContact
        }
    }

    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {   
            Vector3 playerMovement = new Vector3(movementX, 0f, movementY);
            if (playerMovement.magnitude >= 0.1f) {
                float targetAngle = Mathf.Atan2(playerMovement.x, playerMovement.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
                float smoothedRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothingVelocity, rotationSmoothing);
                Vector3 movementDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rigidBody.MovePosition(rigidBody.position + movementDir * Time.deltaTime * speed);
                rigidBody.MoveRotation(Quaternion.Euler(0f, smoothedRotationAngle, 0f));
            }
            else
            {
                animator.SetBool("walking", false);
            }
        }        
    }
}
