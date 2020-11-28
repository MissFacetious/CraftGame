using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Fungus;

public class CraftingManager : MonoBehaviour
{
    public Recipes recipes;
    public RecipeManager recipeManager;
    public InventoryManager inventoryManager;
    public PanelManager panelManager;
    public Button mixButton;

    [Header("Crafting Panel")]
    // items in the mix panel
    public Item item;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject inventoryPanel;
    public GameObject inventoryContent;
    //public Flowchart gameManagerFlowchart;

    public void Select(GameObject item, GameObject itemSlot)
    {
        mixButton.onClick.RemoveAllListeners();
        mixButton.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                panelManager.CraftPanel(recipeManager.getCurrentRecipe());
            });

        int count = itemSlot.GetComponent<Item>().count + item.GetComponent<Item>().count;
        itemSlot.GetComponent<Item>().count = count;

        // move item into slot
        item.transform.parent = itemSlot.transform;
        // make the item disapear from the render
        item.transform.localPosition = new Vector2(-500f, 0f);
    }

    public bool Craft(Recipes.RecipeEnum type)
    {
        // make new item and put into inventory
        Item slot1 = item1.GetComponent<Item>();
        Item slot2 = item2.GetComponent<Item>();
        Item slot3 = item3.GetComponent<Item>();
        bool success = false;

        if (recipes.CheckRecipe(type, slot1, slot2, slot3))
        {
            inventoryManager.CreateNewItem(type, false);
            
            success = true;
        }
        // loop through slot1, slot2, slot3 and destroy anything that is of Component<Item>()
        for (int i = 0; i < slot1.gameObject.transform.childCount; i++)
        {
            if (slot1.gameObject.transform.GetChild(i).GetComponent<Item>() != null)
            {
                Destroy(slot1.gameObject.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < slot2.gameObject.transform.childCount; i++)
        {
            if (slot2.gameObject.transform.GetChild(i).GetComponent<Item>() != null)
            {
                Destroy(slot2.gameObject.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < slot3.gameObject.transform.childCount; i++)
        {
            if (slot3.gameObject.transform.GetChild(i).GetComponent<Item>() != null)
            {
                Destroy(slot3.gameObject.transform.GetChild(i).gameObject);
            }
        }
        return success;
    }

    public void DestroyRecipePanel()
    {
        // if this came from mixing
        // move the original inventory objects that were mixed

        for (int i1 = 0; i1 < item1.transform.childCount; i1++)
        {
            Transform t1 = item1.transform.GetChild(i1);
            Item itemChild1 = t1.gameObject.GetComponent<Item>();
            if (itemChild1 != null)
            {
                // move back to where it belongs
                //Destroy(itemChild1.gameObject);
                itemChild1.transform.parent = inventoryManager.transform;
            }
        }
        for (int i2 = 0; i2 < item2.transform.childCount; i2++)
        {
            Transform t2 = item2.transform.GetChild(i2);
            Item itemChild2 = t2.gameObject.GetComponent<Item>();
            if (itemChild2 != null)
            {
                // move back to where it belongs
                itemChild2.transform.parent = inventoryManager.transform;
            }
        }
        for (int i3 = 0; i3 < item3.transform.childCount; i3++)
        {
            Transform t3 = item3.transform.GetChild(i3);
            Item itemChild3 = t3.gameObject.GetComponent<Item>();
            if (itemChild3 != null)
            {
                // move back to where it belongs
                itemChild3.transform.parent = inventoryManager.transform;
                //Destroy(itemChild3.gameObject);
            }
        }
        
        item1.GetComponent<Item>().count = 0;
        item2.GetComponent<Item>().count = 0;
        item3.GetComponent<Item>().count = 0;
    }

    public void ShowFilteredInventory(GameObject obj)
    {
        ArrayList inventoryList = inventoryManager.Bundlize();

        // first, we need to know how big the panel needs to be, so I need to calculate the rowCount
        int rowCount = 0;
        for (int i = 0; i < inventoryList.Count; i++)
        {
            Item myItem = (Item)inventoryList[i];
            //Debug.Log(myItem.inBundle());
            if (myItem.type == obj.GetComponent<Item>().type)
            {
                rowCount++;
            }
        }
        
        int row = 0;
        int column = 0;
        inventoryContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil((rowCount-1) / 5) * 105 + 120f));
        for (int i = 0; i < inventoryList.Count; i++)
        {
            Item myItem = (Item)inventoryList[i];
            if (myItem.type == obj.GetComponent<Item>().type)
            {
                myItem.GetComponent<Button>().onClick.RemoveAllListeners();
                myItem.GetComponent<Button>().onClick.AddListener(
                delegate {
                    panelManager.SelectItemPanel(myItem.gameObject, obj);
                });
                myItem.gameObject.transform.parent = inventoryContent.transform;
                // now move where it displays
                column++;
                if (column % 5 == 1)
                {
                    row++;
                    column = 1;
                }
                myItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                myItem.gameObject.transform.localPosition = new Vector2((column * 105f) - 50f, -(row * 105f) + 45f);
            }
        }
    }

    public void HideFilteredInventory()
    {
        int size = inventoryContent.transform.childCount;
        for (int i = 0; i < size; i++)
        {
            inventoryContent.transform.GetChild(0).parent = inventoryManager.transform;
        }
    }
}
