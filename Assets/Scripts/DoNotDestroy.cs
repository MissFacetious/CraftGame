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
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("VillageScene", LoadSceneMode.Single);
        }
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("1").wasPressedThisFrame)
        {
            SceneManager.LoadScene("SummerShoresScene", LoadSceneMode.Single);
        }
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("2").wasPressedThisFrame)
        {
            SceneManager.LoadScene("SpringHillsScene", LoadSceneMode.Single);
        }
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("3").wasPressedThisFrame)
        {
            SceneManager.LoadScene("AutumnWoodlandsScene", LoadSceneMode.Single);
        }
        if (Keyboard.current.FindKeyOnCurrentKeyboardLayout("4").wasPressedThisFrame)
        {
            SceneManager.LoadScene("GreeneGardensScene", LoadSceneMode.Single);
        }
    }
}