using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rb;
    public Camera cam;
    private float movementX;
    private float movementY;
    private Quaternion camRotation;
    public int branches = 0;
    public TextMeshProUGUI brachesCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camRotation = cam.transform.rotation;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
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
        rb.AddForce(movement * speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        brachesCount.GetComponent<TextMeshProUGUI>().text = "Apples Collected: " + branches;
    }
}
