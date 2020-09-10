using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Camera), typeof(Interactor))]
public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI brachesCount;
   
    public int branches = 0;
    public float rotationSmoothing = 0.05f;
    public float rotationSmoothingVelocity;
    public float speed = 10;

    [SerializeField]
    private Camera playerCamera;
    private CameraController cameraController;
    private Interactor interactor;
    private PlayerInput playerInput;
    private Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        interactor = GetComponent<Interactor>();
        cameraController = playerCamera.GetComponent<CameraController>();
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
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnLook(InputValue lookValue)
    {
        cameraController.lookVector = lookValue.Get<Vector2>();
    }

    private void OnInteract(InputValue interactValue)
    {
        interactor.PerformInteraction();
    }

    void FixedUpdate()
    {
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //rb.AddForce(movement * speed);
    }

    // Update is called once per frame
    void Update()
    {
        //brachesCount.GetComponent<TextMeshProUGUI>().text = "Apples Collected: " + branches;
        
        if (canMove)
        {
            Vector3 playerMovement = new Vector3(movementX, 0f, movementY);
            if (playerMovement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(playerMovement.x, playerMovement.z) * Mathf.Rad2Deg + playerCamera.transform.eulerAngles.y;
                float smoothedRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothingVelocity, rotationSmoothing);

                transform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
                Vector3 movementDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                transform.Translate(movementDir * speed * Time.deltaTime, Space.World);
            }
        }        
    }
}
