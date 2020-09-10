using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    public Transform cameraTransform;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
    }
}
