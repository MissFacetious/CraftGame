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
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = new Vector3(0f, 0.5f, 0f);

        Destroy(this);

        // Set inactive later if/when pooling?
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        rotationAngle += rotateSpeed * Time.deltaTime;
        rb.MoveRotation(Quaternion.Euler(0f, rotationAngle, 0f));
    }
}
