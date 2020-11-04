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
        summer,
        spring,
        autumn,
        greene,
        craft,
        outro,
        credits
    }

    public scene sceneName;
    public EventSystem eventSystem;
    public PanelManager panelManager;
    public GameObject firstButton;
    public GameObject backButton;
    public GameObject craftRecipesButton;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI locationName;
    public StartingPoint startingPoint;

    private bool countdown;
    private float timeLeft;

    private int currentCount = 0;
    private int outOf = 40;
    private bool selectAgain = true;

    // Start is called before the first frame update
    void Start()
    {
        getEventSystem();
        getPanelManager();
          
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (sceneName == scene.title)
        {
            showMenu();
        }
        else if (sceneName == scene.village)
        {
            showTitle("The Village");
        }
        else if (sceneName == scene.summer)
        {
            showTitle("Summer Shores");
        }
        else if (sceneName == scene.spring)
        {
            showTitle("The Spring Hills");
        }
        else if (sceneName == scene.autumn)
        {
            showTitle("Autumn Woodlands");
        }
        else if (sceneName == scene.greene)
        {
            showTitle("Greene Gardens");
        }
        else if (sceneName == scene.craft)
        {
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
        timeLeft = 3 * 60; // currently 3 minutes for demo, can be 10 minutes for final
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
            int seconds = Mathf.FloorToInt(timeLeft % 60);
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
        GetComponent<Animator>().SetBool("menu", true);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(craftRecipesButton);
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

    public void MenuEnd()
    {
        stopClock();
        startingPoint.EndPanel();
    }

    public void End(bool success)
    {
        GetComponent<Animator>().SetBool("menu", false);
        
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
            if (sceneName == scene.summer || 
                sceneName == scene.spring || 
                sceneName == scene.autumn || 
                sceneName == scene.greene) {
                // set variable - complete by default.
                Flowchart flowchart = eventSystem.GetComponentInChildren<Flowchart>();
                if(flowchart != null)
                {
                    if (sceneName == scene.spring)
                    {
                        if (success)
                        {
                            flowchart.SetStringVariable("hoshi_state", "GATHERING_SUCCEEDED");
                        }
                        else
                        {
                            flowchart.SetStringVariable("hoshi_state", "GATHERING_FAILED");
                        }
                    }
                    if (sceneName == scene.autumn)
                    {
                        if (success)
                        {
                            flowchart.SetStringVariable("hawking_state", "GATHERING_SUCCEEDED");
                        }
                        else
                        {
                            flowchart.SetStringVariable("hawking_state", "GATHERING_FAILED");
                        }
                    }
                    if (sceneName == scene.summer)
                    {
                        if (success)
                        {
                            flowchart.SetStringVariable("ivy_state", "GATHERING_SUCCEEDED");
                        }
                        else
                        {
                            flowchart.SetStringVariable("ivy_state", "GATHERING_FAILED");
                        }
                    }
                    if (sceneName == scene.greene)
                    {
                        if (success)
                        {
                            flowchart.SetStringVariable("greene_state", "GATHERING_SUCCEEDED");
                        }
                        else
                        {
                            flowchart.SetStringVariable("greene_state", "GATHERING_FAILED");
                        }
                    }
                    
                }
                else
                {
                    Debug.Log("Whoopsie daisy. Flowchart not found!");
                }
            }
        }

        SceneManager.LoadScene("VillageScene", LoadSceneMode.Single);
    }

    public void End()
    {
        GetComponent<Animator>().SetBool("menu", false);
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
        if (GameObject.FindGameObjectWithTag("Selection") != null && selectAgain)
        {
            GameObject firstSelection = GameObject.FindGameObjectWithTag("Selection");
            // a fungus yes/no selection has popped up, first selection is tagged
            eventSystem.SetSelectedGameObject(firstSelection);
            selectAgain = false;
        }
        else if (GameObject.FindGameObjectWithTag("Selection") == null)
        {
            selectAgain = true;
        }
        if ((sceneName == scene.summer ||
             sceneName == scene.spring ||
             sceneName == scene.autumn ||
             sceneName == scene.greene) && countdown)
        {
            if (timeLeft <= 1)
            {
                MenuEnd();
            } else
            {
                timeLeft -= Time.deltaTime;
                showCountdown();
            }
        }
        if ((sceneName == scene.village ||
             sceneName == scene.summer ||
             sceneName == scene.spring ||
             sceneName == scene.autumn ||
             sceneName == scene.greene || 
             sceneName == scene.craft) && Keyboard.current.iKey.wasPressedThisFrame)
        {
            Inventory();
        }
        if (sceneName == scene.craft && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Recipes();
        }
        if ((sceneName == scene.village ||
             sceneName == scene.summer ||
             sceneName == scene.spring ||
             sceneName == scene.autumn ||
             sceneName == scene.greene ||
             sceneName == scene.craft) && Keyboard.current.mKey.wasPressedThisFrame)
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
