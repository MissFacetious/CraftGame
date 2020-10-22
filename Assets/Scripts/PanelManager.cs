using Fungus;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public enum ActionTaken
    {
        OPEN_RECIPE,
        ADDED_ITEM,
        OPEN_ITEM,
    }

    public class Action
    {
        public ActionTaken actionTaken;
        public GameObject item;
        public GameObject itemSlot;
    }

    [Header("UI Panels")]
    // panels to show at certain states
    public GameObject inventoryPanel;
    public GameObject smallInventoryPanel;
    public GameObject recipePanel;
    public GameObject craftPanel;
    public GameObject resultPanel;

    [Header("Managers")]
    // managers to do the work
    public InventoryManager inventoryManager;
    public RecipeManager recipeManager;
    public CraftingManager craftingManager;

    [Header("Event System")]
    public EventSystem eventSystem;
    public GameObject filteredInventoryContent;
    public GameObject recipeContent;
    public GameObject inventoryContent;
    public GameObject slot1;
    public GameObject okayButton;

    [Header("Animator")]
    public Animator mix;

    public Flowchart flowchart;

    Stack<Action> lastAction = new Stack<Action>();

    public void ShowInventoryPanel()
    {
        craftingManager.HideFilteredInventory();
        // remove any panels for crafting
        recipePanel.SetActive(false);
        craftPanel.SetActive(false);
        resultPanel.SetActive(false);

        if (inventoryPanel.activeInHierarchy)
        {
            inventoryManager.HideInventory();
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
            inventoryManager.ShowInventory();
        }
        // move event system to first item in inventory
        if (inventoryContent.transform.childCount > 0)
        {   
            eventSystem.SetSelectedGameObject(inventoryContent.transform.GetChild(0).gameObject);
        }
    }

    public void ShowRecipesPanel()
    {
        craftingManager.HideFilteredInventory();
        inventoryManager.HideInventory();

        if (resultPanel.activeInHierarchy)
        {
            craftingManager.DestroyRecipePanel();
            resultPanel.SetActive(false);
        }
        else if (craftPanel.activeInHierarchy)
        {
            craftingManager.DestroyRecipePanel();
            lastAction.Clear();
            craftPanel.SetActive(false);
        }
        else if (recipePanel.activeInHierarchy)
        {
            recipePanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(false);
            recipePanel.SetActive(true);
            recipeManager.ShowRecipes();
        }
        // move event system to first item in recipes
        if (recipeContent.transform.childCount > 0)
        {
            eventSystem.SetSelectedGameObject(recipeContent.transform.GetChild(0).gameObject);
        }
    }

    public void OpenRecipePanel(Recipes.RecipeEnum type)
    {
        craftingManager.HideFilteredInventory();
        inventoryManager.HideInventory();

        inventoryPanel.SetActive(false);
        recipePanel.SetActive(false);
        craftPanel.SetActive(true);
        resultPanel.SetActive(false);

        // set the last action in your stack
        Action action = new Action();
        action.actionTaken = ActionTaken.OPEN_RECIPE;

        lastAction.Push(action);
        craftPanel.SetActive(true);

        // open type recipe
        recipeManager.setCurrentRecipe(type);
        recipeManager.ShowRecipeItems();

        // move event system to slot item1
        eventSystem.SetSelectedGameObject(slot1);
    }

    public void OpenItemPanel(GameObject obj)
    {
        inventoryManager.HideInventory();
        // open up inventory in recipe mixer
        craftingManager.HideFilteredInventory();
        smallInventoryPanel.gameObject.SetActive(true);

        craftingManager.ShowFilteredInventory(obj);
        // set the last action in your stack
        Action action = new Action();
        action.actionTaken = ActionTaken.OPEN_ITEM;

        lastAction.Push(action);

        // move event system to first item in filtered inventory
        if (filteredInventoryContent.transform.childCount > 0)
        {
            eventSystem.SetSelectedGameObject(filteredInventoryContent.transform.GetChild(0).gameObject);
        }
    }

    public void SelectItemPanel(GameObject item, GameObject itemSlot)
    {
        inventoryManager.HideInventory();
        craftingManager.Select(item, itemSlot);
        // set the last action in your stack
        Action action = new Action();
        action.item = item;
        action.itemSlot = itemSlot;
        action.actionTaken = ActionTaken.ADDED_ITEM;

        lastAction.Push(action);

        // close the inventory panel
        craftingManager.HideFilteredInventory();
        smallInventoryPanel.gameObject.SetActive(false);

        // move event system to slot it came from
        eventSystem.SetSelectedGameObject(itemSlot);
    }

    public void CraftPanel(Recipes.RecipeEnum type)
    {
        inventoryManager.HideInventory();
        craftingManager.HideFilteredInventory();
        // kick off mixing animation
        mix.SetTrigger("mix");
        
        bool success = craftingManager.Craft(type);
        if (success)
        {
            resultPanel.GetComponent<ResultsDisplay>().ShowSuccess();
            // tell the flowchart to set a var
            if (type == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
            {
                flowchart.SetBooleanVariable("hoshi_item_crafted", true);
            }
        }
        else
        {
            // failure panel
            resultPanel.GetComponent<ResultsDisplay>().ShowFailure();
        }
        // show results panel
        resultPanel.SetActive(true);
        resultPanel.GetComponent<ResultsDisplay>().setItem(type);
        craftPanel.SetActive(false);

        // clear the last actions
        lastAction.Clear();

        // move event system to okay button on results panel
        eventSystem.SetSelectedGameObject(okayButton);
    }

    public void UndoPanel()
    {
        if (lastAction.Count > 0)
        {
            Action thisAction = lastAction.Pop();

            if (thisAction.actionTaken.Equals(ActionTaken.OPEN_RECIPE))
            {
                craftPanel.SetActive(false);
                recipePanel.SetActive(true);
            }
            if (thisAction.actionTaken.Equals(ActionTaken.ADDED_ITEM))
            {

                for (int i = 0; i < thisAction.itemSlot.transform.childCount; i++)
                {
                    Transform t = thisAction.itemSlot.transform.GetChild(i);
                    Item itemChild = t.gameObject.GetComponent<Item>();

                    if (itemChild != null)
                    {
                        if (thisAction.item.GetComponent<Item>().Equals(itemChild))
                        {
                            //itemChild.originalRef.GetComponent<Item>().available = true;
                            Destroy(itemChild.gameObject);
                        }
                    }
                }
                // then open inventory with the last touched slot
                inventoryPanel.gameObject.SetActive(true);
                craftingManager.ShowFilteredInventory(thisAction.itemSlot);
            }
            if (thisAction.actionTaken.Equals(ActionTaken.OPEN_ITEM))
            {
                inventoryPanel.SetActive(false);
            }
        }
        //craftingManager.UpdateValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
