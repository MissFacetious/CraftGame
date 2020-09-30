using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
    
    public void Select(GameObject item, GameObject itemSlot)
    {
        mixButton.GetComponent<Button>().onClick.AddListener(
            delegate {
                panelManager.CraftPanel(recipeManager.getCurrentRecipe());
            });

        if (item.GetComponent<Item>().available)
        {
            // show the item in the slot if not already there...
            // show the count of the item in the slot
            //itemSlot.GetComponent<Item>().image = item.GetComponent<Item>().image;
            //string countStr = itemSlot.GetComponent<Item>().count.text;
            // update the mixing values
            //int.TryParse(countStr, out int count);
            int count = itemSlot.GetComponent<Item>().count;
            count++;
            itemSlot.GetComponent<Item>().count = count;
            //itemSlot.GetComponent<Item>().count.text = count.ToString();

            // mark inventory item unavailable
            //item.GetComponent<Item>().available = false;
            item.GetComponent<Item>().originalRef.available = false;
            // move item into slot
            item.transform.parent = itemSlot.transform;
            // make the item disapear from the render
            item.transform.localPosition = new Vector2(-500f, 0f);
        }
    }

    public bool Craft(Recipes.RecipeEnum type)
    {
        Debug.Log("craft");
        //UpdateValues();

        // make new item and put into inventory
        Item slot1 = item1.GetComponent<Item>();
        Item slot2 = item2.GetComponent<Item>();
        Item slot3 = item3.GetComponent<Item>();
        
        if (recipes.CheckRecipe(type, slot1, slot2, slot3))
        {
            Item myItem = Instantiate(item);
            
            myItem.setItem(type);
            
            myItem.gameObject.transform.parent = inventoryManager.gameObject.transform;

            // move new object into success panel view
            myItem.gameObject.transform.parent = inventoryManager.gameObject.transform;
            myItem.gameObject.transform.localPosition = Vector2.zero;
            Debug.Log("success!");
            return true;
        }
        return false;
    }

    public void DestroyRecipePanel()
    {
        // if this came from mixing
        // destroy the original inventory objects that were mixed
        for (int i1 = 0; i1 < item1.transform.childCount; i1++)
        {
            Transform t1 = item1.transform.GetChild(i1);
            Item itemChild1 = t1.gameObject.GetComponent<Item>();
            if (itemChild1 != null)
            {
                Destroy(itemChild1.originalRef.gameObject);
                Destroy(itemChild1.gameObject);
            }
        }
        for (int i2 = 0; i2 < item2.transform.childCount; i2++)
        {
            Transform t2 = item2.transform.GetChild(i2);
            Item itemChild2 = t2.gameObject.GetComponent<Item>();
            if (itemChild2 != null)
            {
                Destroy(itemChild2.originalRef.gameObject);
                Destroy(itemChild2.gameObject);
            }
        }
        for (int i3 = 0; i3 < item3.transform.childCount; i3++)
        {
            Transform t3 = item3.transform.GetChild(i3);
            Item itemChild3 = t3.gameObject.GetComponent<Item>();
            if (itemChild3 != null)
            {
                Destroy(itemChild3.originalRef.gameObject);
                Destroy(itemChild3.gameObject);
            }
        }
        item1.GetComponent<Item>().count = 0;
        item2.GetComponent<Item>().count = 0;
        item3.GetComponent<Item>().count = 0;
    }

    public void ShowFilteredInventory(GameObject obj)
    {
        // redraw the inventory panel
        foreach (Transform child in inventoryContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // first, we need to know how big the panel needs to be, so I need to calculate the rowCount
        int rowCount = 0;
        for (int i = 0; i < inventoryManager.transform.childCount; i++)
        {
            Item myItem = inventoryManager.gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            if (myItem.available && myItem.title == obj.GetComponent<Item>().title)
            {
                rowCount++;
            }
        }

        int row = 0;
        int column = 0;
        inventoryContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil(rowCount / 5) * 105 + 120f));
        for (int i = 0; i < inventoryManager.gameObject.transform.childCount; i++)
        {
            Item myItem = inventoryManager.gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            if (myItem.available && myItem.title == obj.GetComponent<Item>().title)
            {
                //list.Add(myItem.gameObject);
                Item newItem = Instantiate(myItem);
                newItem.setItem(myItem.type);
                //myItem.available = true;
                newItem.originalRef = myItem;
                newItem.GetComponent<Button>().onClick.AddListener(
                    delegate {
                        panelManager.SelectItemPanel(newItem.gameObject, obj);
                    });
                newItem.gameObject.transform.parent = inventoryContent.transform;
                // now move where it displays
                column++;
                if (column % 5 == 1)
                {
                    row++;
                    column = 1;
                }
                newItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                newItem.gameObject.transform.localPosition = new Vector2((column * 105f) - 50f, -(row * 105f) + 45f);
            }
        }
    }
}
