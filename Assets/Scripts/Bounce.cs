using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        float original = other.rigidbody.velocity.y;
        other.rigidbody.AddForce(0, 499.9699999f, 0, ForceMode.Impulse); 
    }
}
