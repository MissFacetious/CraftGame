using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuActions : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject firstButton;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI timer;

    private bool countdown;
    private float timeLeft;

    private int currentCount = 0;
    private int outOf = 40;

    // Start is called before the first frame update
    void Start()
    {
        startClock();
    }

    public void startClock()
    {

        countdown = true;
        timeLeft = 10 * 60; // 10 minutes
    }

    public void pauseClock()
    {
        countdown = false;
    }

    public void stopClock()
    {
        countdown = false;
        timeLeft = 0;
    }

    public void showCountdown()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.CeilToInt(timeLeft % 60);
        timer.text = minutes + ":" + seconds;
    }

    public void increaseCurrentCounter()
    {
        currentCount++;
        counter.text = currentCount + " / " + outOf;
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
        if (countdown)
        {
            timeLeft -= Time.deltaTime;
            showCountdown();
        }
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
