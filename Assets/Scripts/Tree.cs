using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tree : MonoBehaviour
{
    public bool showButton;
    public GameObject Box;
    public GameObject Sphere;

    [SerializeField]
    private bool hasItemsToDrop;

    // Start is called before the first frame update
    void Start()
    {
        showButton = false;
        hasItemsToDrop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (showButton)
        {
            Box.SetActive(true);
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                //Destroy(GetComponent<Collider>());
                //showButton = false;
                //// create a shower of other items
                //for (int i = 0; i < 10; i++) {
                //    GameObject branch = Instantiate(Sphere, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity) as GameObject;
                //}
            }
        }
        else
        {
            Box.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    public void Interaction()
    {
        ShowerItems();
    }

    private void ShowerItems()
    {
        if (hasItemsToDrop)
        {
            //showButton = false;
            //for (int i = 0; i < objectPool.Count; i++)
            //{
            //    GameObject obj = objectPool.
            //}

            hasItemsToDrop = false;
        }
    }
}
