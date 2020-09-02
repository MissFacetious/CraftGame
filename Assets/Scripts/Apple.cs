using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(new Vector3(0f, 5f, 0f));
        rb.AddForce(Vector3.up * 100f);
        //GetComponent<Rigidbody>().AddForce(transform.forward);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.inertiaTensorRotation = new Quaternion(0.01f, 0.01f, 0.01f, 1f);
        rb.AddTorque(-rb.angularVelocity);
    }
}
