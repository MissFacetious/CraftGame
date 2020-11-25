using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public TextMeshProUGUI counter;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI locationName;
    public StartingPoint startingPoint;

    public GameObject selectIcon;
    public GameObject backIcon;
    public GameObject jumpIcon;
    public GameObject runIcon;
    public GameObject menuIcon;

    private bool countdown;
    private float timeLeft;

    private int currentCount = 0;
    private int outOf = 40;
    private bool selectAgain = true;
    private bool continueAgain = true;

    private Interactor interactor;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        getEventSystem();
        getPlayerInput();
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

    void getPlayerInput()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            interactor = playerInput.GetComponent<Interactor>();
            OnControlsChanged();
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

    public void OnControlsChanged()
    {
        PlayerController.currentControls.ToString();
        if (interactor != null)
        {
            // change sprites of the bottom menu
            if (selectIcon != null)
            {
                selectIcon.GetComponent<Image>().sprite = interactor.UpdateIconSprite(PlayerController.currentControls.ToString(), Interactor.buttons.okay, false);
            }
            if (backIcon != null)
            {
                backIcon.GetComponent<Image>().sprite = interactor.UpdateIconSprite(PlayerController.currentControls.ToString(), Interactor.buttons.cancel, false);
            }
            if (jumpIcon != null)
            {
                jumpIcon.GetComponent<Image>().sprite = interactor.UpdateIconSprite(PlayerController.currentControls.ToString(), Interactor.buttons.jump, false);
            }
            if (runIcon != null)
            {
                runIcon.GetComponent<Image>().sprite = interactor.UpdateIconSprite(PlayerController.currentControls.ToString(), Interactor.buttons.run, false);
            }
            if (menuIcon != null)
            {
                menuIcon.GetComponent<Image>().sprite = interactor.UpdateIconSprite(PlayerController.currentControls.ToString(), Interactor.buttons.menu, false);
            }
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

    public void addTime(float newTime)
    {
        //only add time to the counter if we're above one second remaining
        //this is to hold off any weirdness with ending gathering.
        if (GameObject.FindGameObjectWithTag("Dialog") != null ||
            GameObject.FindGameObjectWithTag("Selection") != null ||
            GameObject.FindGameObjectWithTag("Menu") != null)
        {
            // let the timer pause
        }
        else if (timeLeft >= 1.0f)
        {
            timeLeft = timeLeft + newTime;
        }
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
            string minutesStr = minutes.ToString();
            string secondsStr = seconds.ToString();
            if (seconds < 10)
            {
                secondsStr = "0" + secondsStr;
            }
            if (minutes < 10)
            {
                minutesStr = "0" + minutesStr;
            }
            timer.text = minutesStr + ":" + secondsStr;
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
        transform.Find("MainMenu").GetChild(0).gameObject.SetActive(true);
        transform.Find("BottomMenu").GetChild(0).gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(firstButton);
    }

    public void Inventory()
    {
        if (sceneName == scene.craft)
        {
            if (transform.Find("MainMenu").gameObject.activeInHierarchy)
            {
                panelManager.ShowInventoryPanel();
                transform.Find("MainMenu").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("MainMenu").gameObject.SetActive(true);
            }
        }
        else
        {
            panelManager.ShowInventoryPanel();
            GetComponent<Animator>().SetBool("menu", false);
        }
    }

    public void Recipes()
    {
        if (sceneName == scene.craft) {
            if (transform.Find("MainMenu").gameObject.activeInHierarchy)
            {
                panelManager.ShowRecipesPanel();
                transform.Find("MainMenu").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("MainMenu").gameObject.SetActive(true);
            }
         }
        else {
            panelManager.ShowRecipesPanel();
            GetComponent<Animator>().SetBool("menu", false);
        }
    }

    public void MenuEnd()
    {
        stopClock();
        startingPoint.EndPanel();
    }

    public void End(bool success)
    {
        GetComponent<Animator>().SetBool("menu", false);

        Debug.Log("sceneName:" + sceneName);

        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
            if (sceneName == scene.summer || 
                sceneName == scene.spring || 
                sceneName == scene.autumn || 
                sceneName == scene.greene) {
                // set variable - complete by default.
                Flowchart flowchart = eventSystem.GetComponentInChildren<Flowchart>();
         
                if (flowchart != null)
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

        eventSystem.SetSelectedGameObject(null);
        if (sceneName == scene.craft)
        {
            // set variable - complete by default.
            Flowchart flowchart = eventSystem.GetComponentInChildren<Flowchart>();
            //Debug.Log("sceneName:" + sceneName);
            //Debug.Log("flowchart:" + flowchart.GetName());
            if (flowchart != null)
            {
                //Debug.Log("sceneName:" + sceneName);
                if (sceneName == scene.craft)
                {
                    //Debug.Log("Checking the state of things upon exiting crafting.");
                    InventoryManager im = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
                    if (!im.CanCraftItemFromInventory(global::Recipes.RecipeEnum.RAINBOW_REFRACTOR) && flowchart.GetStringVariable("hoshi_state") == "GATHERING_SUCCEEDED")
                    {
                        //check hoshi state.  if hoshi state is gathering succeeded and we can't craft.
                        flowchart.SetStringVariable("hoshi_state", "GATHERING_AGAIN");
                        //Debug.Log("Set Hoshi back to Gathering_FAILED!");
                    }

                    if (!im.CanCraftItemFromInventory(global::Recipes.RecipeEnum.APPLEBLOSSOM_TEA) && flowchart.GetStringVariable("hawking_state") == "GATHERING_SUCCEEDED")
                    {
                        flowchart.SetStringVariable("hawking_state", "GATHERING_AGAIN");
                    }

                    if (!im.CanCraftItemFromInventory(global::Recipes.RecipeEnum.TRANSFORMATIONAL_POTION) && flowchart.GetStringVariable("ivy_state") == "GATHERING_SUCCEEDED")
                    {
                        flowchart.SetStringVariable("ivy_state", "GATHERING_AGAIN");
                    }

                    if (!im.CanCraftItemFromInventory(global::Recipes.RecipeEnum.GNOME_NET) && flowchart.GetStringVariable("greene_state") == "GATHERING_SUCCEEDED")
                    {
                        flowchart.SetStringVariable("greene_state", "GATHERING_AGAIN");
                    }

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

    public void OnCancel(InputValue inputValue)
    {
        Debug.Log("hit cancel");

        // if in inventory, to the menu
        if (GameObject.FindGameObjectWithTag("InventoryPanel") != null && GameObject.FindGameObjectWithTag("InventoryPanel").activeInHierarchy) { 
            Inventory();
            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(firstButton);
            }
        }
        // if in recipe, to the menu
        if (GameObject.FindGameObjectWithTag("RecipePanel") != null && GameObject.FindGameObjectWithTag("RecipePanel").activeInHierarchy)
        {
            Recipes();
            if (eventSystem != null)
            {
                eventSystem.SetSelectedGameObject(firstButton);
            }
        }
    }

    public void OnMenu(InputValue inputValue)
    {
        OnControlsChanged();
        if ((sceneName == scene.village ||
             sceneName == scene.summer ||
             sceneName == scene.spring ||
             sceneName == scene.autumn ||
             sceneName == scene.greene ||
             sceneName == scene.craft))
        {
            bool menu = GetComponent<Animator>().GetBool("menu");
            GetComponent<Animator>().SetBool("menu", !menu);
            if (eventSystem != null)
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
        if (GameObject.FindGameObjectWithTag("Continue") != null && continueAgain)
        {
            GameObject continueIcon = GameObject.FindGameObjectWithTag("Continue");
            // a fungus continue icon has popped up, set the sprite correctly
            continueIcon.GetComponent<Image>().sprite = interactor.UpdateIconSprite(playerInput.devices[0].name, Interactor.buttons.okay, true);
            continueAgain = false;
        }
        else if (GameObject.FindGameObjectWithTag("Continue") == null)
        {
            continueAgain = true;
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
