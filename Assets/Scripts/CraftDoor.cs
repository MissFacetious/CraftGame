using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CraftDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnInteract()
    {
        Debug.Log(name + "'s interaction method");
        SceneManager.LoadScene("CraftScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
