using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    void Awake()
    {
        string thisName = gameObject.name;
        GameObject[] objs = GameObject.FindGameObjectsWithTag(thisName);
        Debug.Log(objs.Length);
        if (objs.Length > 1)
        {
            // this is already loaded, destory!
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("CraftScene", LoadSceneMode.Single);
        }
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("ProceduralGatheringRev2", LoadSceneMode.Single);
        }
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("VillageScene", LoadSceneMode.Single);
        }
    }
}