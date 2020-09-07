using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Security.AccessControl;
using UnityEngine.Assertions.Must;
using System.ComponentModel;

[RequireComponent(typeof(Camera))]
public class PlayerController : MonoBehaviour
{

    public Camera cam;
    public TextMeshProUGUI brachesCount;
   
    public int branches = 0;
    public float rotationSmoothing = 0.05f;
    public float rotationSmoothingVelocity;
    public float speed = 10;


    private Rigidbody rb;
    //private Vector3 playerMovement;
    
    private bool canMove = true;
    private float movementX;
    private float movementY;


    private void Awake()
    {
        if (cam.Equals(null))
        {
            Debug.LogError("Camera not found.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerMovement = Vector3.zero;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnLook(InputValue lookValue)
    {
        //Debug.Log(lookValue.Get<Vector2>());
        cam.GetComponent<CameraController>().lookVector = lookValue.Get<Vector2>();
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.tag.Equals("Branch"))
        {
            branches++;
            Destroy(other.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag.Equals("Tree"))
        {
            other.GetComponent<Tree>().showButton = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log(other);
        if (other.tag.Equals("Tree"))
        {
            other.GetComponent<Tree>().showButton = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //rb.AddForce(movement * speed);
    }

    // Update is called once per frame
    void Update()
    {
        brachesCount.GetComponent<TextMeshProUGUI>().text = "Apples Collected: " + branches;

        if (canMove)
        {
            var playerMovement = new Vector3(movementX, 0f, movementY);
            if (playerMovement.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(playerMovement.x, playerMovement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                float smoothedRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothingVelocity, rotationSmoothing);

                transform.rotation = Quaternion.Euler(0f, smoothedRotationAngle, 0f);
                Vector3 movementDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                transform.Translate(movementDir * speed * Time.deltaTime, Space.World);
            }
        }        
    }

    public static bool CheckGroundNear(
       Vector3 charPos,
       float jumpableGroundNormalMaxAngle,
       float rayDepth, //how far down from charPos will we look for ground?
       float rayOriginOffset, //charPos near bottom of collider, so need a fudge factor up away from there
       out bool isJumpable
    )
    {

        bool ret = false;
        bool _isJumpable = false;


        float totalRayLen = rayOriginOffset + rayDepth;

        Ray ray = new Ray(charPos + Vector3.up * rayOriginOffset, Vector3.down);

        int layerMask = 1 << LayerMask.NameToLayer("Default");


        RaycastHit[] hits = Physics.RaycastAll(ray, totalRayLen, layerMask);

        RaycastHit groundHit = new RaycastHit();

        foreach (RaycastHit hit in hits)
        {

            if (hit.collider.gameObject.CompareTag("ground"))
            {

                ret = true;

                groundHit = hit;

                _isJumpable = Vector3.Angle(Vector3.up, hit.normal) < jumpableGroundNormalMaxAngle;

                break; //only need to find the ground once

            }

        }

        //Helper.DrawRay(ray, totalRayLen, hits.Length > 0, groundHit, Color.magenta, Color.green);

        isJumpable = _isJumpable;

        return ret;
    }
}
