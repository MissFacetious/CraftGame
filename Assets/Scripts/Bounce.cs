using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private AudioSource[] audio;

    void Start()
    {
        audio = GetComponents<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        float original = other.rigidbody.velocity.y;
        other.rigidbody.AddForce(0, 499.9699999f, 0, ForceMode.Impulse);
        if (audio != null && audio.Length > 0)
        {
            if (!audio[0].isPlaying)
            {
                audio[0].Play();
            }
        }
    }
}
