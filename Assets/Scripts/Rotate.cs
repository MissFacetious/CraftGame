using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    public bool clockwise = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.back;
        if (clockwise) direction = Vector3.forward;
        transform.Rotate(direction, speed * Time.deltaTime);
    }
}
