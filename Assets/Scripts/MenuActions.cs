using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public enum scene
    {
        title,
        intro,
        village,
        gathering,
        craft,
        outro,
        credits
    }

    public scene sceneName;
    public EventSystem eventSystem;
    public PanelManager panelManager;
    public GameObject firstButton;
    public GameObject backButton;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI locationName;

    private bool countdown;
    private float timeLeft;

    private int currentCount = 0;
    private int outOf = 40;

    // Start is called before the first frame update
    void Start()
    {
        getEventSystem();
        getPanelManager();
        if (sceneName == scene.title)
        {
            showMenu();
        }
        else if (sceneName == scene.village)
        {
            showTitle("The Village");
        }
        else if (sceneName == scene.gathering)
        {
            startClock();
            showTitle("The Spring Hills");
        }
        else if (sceneName == scene.craft)
        {
            Debug.Log("invoke Crafting()");
            Crafting();
        }
        else if (sceneName == scene.credits)
        {
            GetComponent<Animator>().SetTrigger("endCredits");
        }
    }


    void getEventSystem()
    {
        if (GameObject.FindGameObjectWithTag("EventSystem") != null)
        {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        }
        else
        {
            Debug.Log("event system is not hooked up.");
        }
    }

    void getPanelManager()
    {
        if (GameObject.FindGameObjectWithTag("PanelManager") != null)
        {
            panelManager = GameObject.FindGameObjectWithTag("PanelManager").GetComponent<PanelManager>();
        }
        else
        {
            Debug.Log("PanelManager is not hooked up.");
        }
    }

    public void showMenu()
    {
        GetComponent<Animator>().SetBool("menu", true);
        GetComponent<Animator>().SetBool("credits", false);
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(firstButton);
        }
    }

    public void startGame()
    {
        GetComponent<Animator>().SetBool("menu", false);
        SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
    }

    public void showCredits()
    {
        GetComponent<Animator>().SetBool("credits", true);
        GetComponent<Animator>().SetBool("menu", false);
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(backButton);
        }
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

    public void showTitle(string location)
    {
        if (locationName != null)
        {
            locationName.text = location;
            GetComponent<Animator>().SetTrigger("location");
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
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
        }
        // also start input from the player controller
    }

    public void Crafting() { 
        // on start of crafting, show the menu, and when we exit out of inventory/recipe
        //GetComponent<Animator>().SetBool("menu", true);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    public void Inventory()
    {
        if (sceneName == scene.craft)
        {
            if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("menu", false);
        }
        panelManager.ShowInventoryPanel();
    }

    public void Recipes()
    {
        if (sceneName == scene.craft) {
            Debug.Log(transform.GetChild(0).name);
            if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
         }
        else {
            GetComponent<Animator>().SetBool("menu", false);
        }
        panelManager.ShowRecipesPanel();
    }

    public void End()
    {
        GetComponent<Animator>().SetBool("menu", false);
        
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
            if (sceneName == scene.gathering)
            {
                // set variable - complete by default.
                Flowchart flowchart = eventSystem.GetComponentInChildren<Flowchart>();
                if(flowchart != null)
                {
                    flowchart.SetBooleanVariable("hoshi_gath_complete", true);
                    Debug.Log("Gathering complete:" + flowchart.GetBooleanVariable("hoshi_gath_complete"));
                }
                else
                {
                    Debug.Log("Whoopsie daisy. Flowchart not found!");
                }

              
            }
        }

        SceneManager.LoadScene("VillageScene", LoadSceneMode.Single);
    }

    public void Quit()
    {
        GetComponent<Animator>().SetBool("menu", false);
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
        }
        // change scene back to main menu
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CreditScene()
    {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
    }

    void Update()
    {
        if (eventSystem == null)
        {
            getEventSystem();
        }
        if (sceneName != scene.title && panelManager == null)
        {
            getPanelManager();
        }
        if (sceneName == scene.gathering && countdown)
        {
            timeLeft -= Time.deltaTime;
            showCountdown();
        }
        if ((sceneName == scene.village || sceneName == scene.gathering || sceneName == scene.craft) && Keyboard.current.iKey.wasPressedThisFrame)
        {
            Inventory();
        }
        if (sceneName == scene.craft && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Recipes();
        }
        if ((sceneName == scene.village || sceneName == scene.gathering || sceneName == scene.craft) && Keyboard.current.mKey.wasPressedThisFrame)
        {
            bool menu = GetComponent<Animator>().GetBool("menu");
            GetComponent<Animator>().SetBool("menu", !menu);
            if (menu && eventSystem != null)
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
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("CraftScene", LoadSceneMode.Single);
        }
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            CreditScene();
        }
    }
}
