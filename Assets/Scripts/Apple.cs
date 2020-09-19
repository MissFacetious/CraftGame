using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 100f);
    }

    public void Collect(GameObject playerObject)
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<Collider>());

        transform.parent = player.transform;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        // kick off collection animation
        Debug.Log("kick off animation");
        GetComponent<Animator>().SetTrigger("collect");

        Destroy(gameObject, 2.0f); // the 2 seconds is the length of the animation
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.inertiaTensorRotation = new Quaternion(0.01f, 0.01f, 0.01f, 1f);
            rb.AddTorque(-rb.angularVelocity);
        }
    }
}
