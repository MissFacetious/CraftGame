using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuActions : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject firstButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Resume()
    {
        GetComponent<Animator>().SetBool("menu", false);
        eventSystem.SetSelectedGameObject(null);
        // also start input from the player controller
    }

    public void End()
    {
        GetComponent<Animator>().SetBool("menu", false);
        eventSystem.SetSelectedGameObject(null);

        // show ending UI

        // select default ending ui button
        //eventSystem.SetSelectedGameObject(null);

    }

    public void Quit()
    {
        GetComponent<Animator>().SetBool("menu", false);
        eventSystem.SetSelectedGameObject(null);
        // change scene back to main menu
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            //ShowInventoryPanel();
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            //ShowRecipesPanel();
        }
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            bool menu = GetComponent<Animator>().GetBool("menu");
            GetComponent<Animator>().SetBool("menu", !menu);
            if (menu)
            {
                eventSystem.SetSelectedGameObject(null);
                // also start input from the player controller
            }
            else
            {
                eventSystem.SetSelectedGameObject(firstButton);
                // also stop input from the player controller
            }
        }
    }
}
