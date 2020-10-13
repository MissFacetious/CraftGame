using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public bool title = false;
    public bool village = false;
    public EventSystem eventSystem;
    public GameObject firstButton;
    public GameObject backButton;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI timer;

    private bool countdown;
    private float timeLeft;

    private int currentCount = 0;
    private int outOf = 40;

    // Start is called before the first frame update
    void Start()
    {
        if (title)
        {
            showMenu();
        }
        else if (village)
        {

        }
        else
        {
            startClock();
        }
    }

    public void showMenu()
    {
        GetComponent<Animator>().SetBool("menu", true);
        GetComponent<Animator>().SetBool("credits", false);
        eventSystem.SetSelectedGameObject(firstButton);
    }

    public void startGame()
    {
        GetComponent<Animator>().SetBool("menu", false);
        SceneManager.LoadScene("VillageScene", LoadSceneMode.Single);
    }

    public void showCredits()
    {
        GetComponent<Animator>().SetBool("credits", true);
        GetComponent<Animator>().SetBool("menu", false);
        eventSystem.SetSelectedGameObject(backButton); 
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
        if (timer)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.CeilToInt(timeLeft % 60);
            timer.text = minutes + ":" + seconds;
        }
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
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    void Update()
    {
        if (!title && countdown)
        {
            timeLeft -= Time.deltaTime;
            showCountdown();
        }
        if (!title && Keyboard.current.iKey.wasPressedThisFrame)
        {
            //ShowInventoryPanel();
        }
        if (!title && Keyboard.current.rKey.wasPressedThisFrame)
        {
            //ShowRecipesPanel();
        }
        if (!title && Keyboard.current.mKey.wasPressedThisFrame)
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
