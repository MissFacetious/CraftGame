using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcheroo : MonoBehaviour
{
    public GameObject currentItem;
    public GameObject newItem;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch()
    {
        Vector3 currentPosition = currentItem.transform.position;
        Vector3 newPosition = newItem.transform.position;
        Vector3 temp = Vector3.zero;
        temp = currentPosition;
        currentItem.transform.position = newPosition;
        newItem.transform.position = new Vector3(temp.x, temp.y + 0.6f, temp.z);
  
    }
}
