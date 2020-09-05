using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public Recipes recipes;

    [Header("UI Panels")]
    // panels to show at certain states
    public GameObject recipePanel;
    public GameObject craftPanel;
    public GameObject resultPanel;

    [Header("Recipe Panel")]
    // things in the recipe panel

    [Header("Crafting Panel")]
    // items in the mix panel
    public Item item;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject inventoryPanel;
    public GameObject inventoryContent;

    public enum ActionTaken
    {
        OPEN_RECIPE,
        ADDED_ITEM,
        OPEN_ITEM,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Item myItem1 = Instantiate(item);
        myItem1.title = "Rainbow Stardust";
        myItem1.aquarius = 9;
        myItem1.capricorn = 3;
        myItem1.gameObject.transform.parent = gameObject.transform;
        Item myItem2 = Instantiate(item);
        myItem2.title = "Red Gemstone";
        myItem2.scorpio = 2;
        myItem2.capricorn = 3;
        myItem2.gameObject.transform.parent = gameObject.transform;
        Item myItem3 = Instantiate(item);
        myItem3.title = "Red Gemstone";
        myItem3.scorpio = 3;
        myItem3.capricorn = 3;
        myItem3.gameObject.transform.parent = gameObject.transform;
        Item myItem4 = Instantiate(item);
        myItem4.title = "Red Gemstone";
        myItem4.scorpio = 2;
        myItem4.capricorn = 4;
        myItem4.gameObject.transform.parent = gameObject.transform;
        Item myItem5 = Instantiate(item);
        myItem5.title = "Rainbow Stardust";
        myItem5.aquarius = 8;
        myItem5.capricorn = 3;
        myItem5.aquarius = 1;
        myItem5.gameObject.transform.parent = gameObject.transform;
        Item myItem6 = Instantiate(item);
        myItem6.title = "Orange Brush";
        myItem6.aquarius = 8;
        myItem6.capricorn = 3;
        myItem6.aquarius = 1;
        myItem6.gameObject.transform.parent = gameObject.transform;
        Item myItem7 = Instantiate(item);
        myItem7.title = "Rainbow Stardust";
        myItem7.scorpio = 8;
        myItem7.capricorn = 3;
        myItem7.gameObject.transform.parent = gameObject.transform;
        Item myItem8 = Instantiate(item);
        myItem8.title = "Orange Brush";
        myItem8.aquarius = 8;
        myItem8.capricorn = 3;
        myItem8.aquarius = 1;
        myItem8.gameObject.transform.parent = gameObject.transform;
        Item myItem9 = Instantiate(item);
        myItem9.title = "Glorious Arrow";
        myItem9.aquarius = 8;
        myItem9.capricorn = 3;
        myItem9.aquarius = 1;
        myItem9.gameObject.transform.parent = gameObject.transform;

        for (int i=0; i < 150; i++)
        {
            Item myItem = Instantiate(item);
            myItem.title = "Glorious Arrow";
            myItem.aquarius = 8;
            myItem.capricorn = 3;
            myItem.aquarius = 1;
            myItem.gameObject.transform.parent = gameObject.transform;
        }
    }
    public void OpenRecipe(string recipe)
    {
        recipePanel.SetActive(false);
        craftPanel.SetActive(true);
    }
    
    public void OpenItem(GameObject obj)
    {
        // open up inventory in recipe mixer
        inventoryPanel.gameObject.SetActive(true);

        showFilteredInventory(obj);
    }

    public void Select(GameObject item, GameObject itemSlot)
    {
        if (item.GetComponent<Item>().available)
        {
            // show the item in the slot if not already there...
            // show the count of the item in the slot
            //itemSlot.GetComponent<Item>().image = item.GetComponent<Item>().image;
            string countStr = itemSlot.GetComponent<Item>().count.text;
            // update the mixing values
            int.TryParse(countStr, out int count);
            count++;
            itemSlot.GetComponent<Item>().count.text = count.ToString();

            // mark inventory item unavailable
            //item.GetComponent<Item>().available = false;
            item.GetComponent<Item>().originalRef.available = false;
            // move item into slot
            item.transform.parent = itemSlot.transform;
            // make the item disapear from the render
            item.transform.localPosition = new Vector2(-500f, 0f);

            // close the inventory panel
            inventoryPanel.gameObject.SetActive(false);
        }
    }

    void showFilteredInventory(GameObject obj)
    {
        // redraw the inventory panel
        foreach (Transform child in inventoryContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        // first, we need to know how big the panel needs to be, so I need to calculate the rowCount
        int rowCount = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Item myItem = gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            if (myItem.available && (obj.GetComponent<Item>().title == "Free" ||
                myItem.title == obj.GetComponent<Item>().title))
            {
                rowCount++;
            }
        }

        int row = 0;
        int column = 0;
        inventoryContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil(rowCount / 5) * 105 + 120f));
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Item myItem = gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            if (myItem.available && (obj.GetComponent<Item>().title == "Free" ||
                myItem.title == obj.GetComponent<Item>().title))
            {
                //list.Add(myItem.gameObject);
                Item newItem = Instantiate(myItem);
                //myItem.available = true;
                newItem.originalRef = myItem;
                newItem.GetComponent<Button>().onClick.AddListener(
                    delegate {
                        Select(newItem.gameObject, obj);

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

    // Update is called once per frame
    void Update()
    {

    }
}
