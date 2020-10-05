using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public Animator mix;
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
            // show success panel
            resultPanel.SetActive(true);
            craftPanel.SetActive(false);
        }
        else
        {
            // failure panel
        }
        // clear the last actions
        lastAction.Clear();
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
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShowInventoryPanel();
        }
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            ShowRecipesPanel();
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UndoPanel();
        }
    }
}
