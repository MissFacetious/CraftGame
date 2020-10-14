using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Rigidbody rb;
    [Range(0f, 100f)]
    public float rotateSpeed = 35f;
    private float rotationAngle = 0f;


    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.up * 100f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found!");
        }
    }

    void OnEnable()
    {
        Debug.Log($"{name} is enabled.");
        // when object pooling, use OnEnable instead of Awake/Start
    }

    public void Collect()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotationAngle += rotateSpeed * Time.deltaTime;
        rb.MoveRotation(Quaternion.Euler(0f, rotationAngle, 0f));
    }
}
