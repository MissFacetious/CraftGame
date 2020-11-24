using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    private static DoNotDestroy _instance;

    public static DoNotDestroy Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DoNotDestroy>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
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