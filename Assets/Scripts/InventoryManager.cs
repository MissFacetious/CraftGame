using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public Recipes recipes;

    [Header("Inventory Panel")]
    // items in the mix panel
    public Item item;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject inventoryPanel;
    public GameObject inventoryContent;
    
    // Start is called before the first frame update
    void Start()
    {
        Item myItem1 = Instantiate(item);
        myItem1.setItem(Recipes.RecipeEnum.APPLEBLOSSOM_TEA);
        myItem1.gameObject.transform.parent = gameObject.transform;
        Item myItem2 = Instantiate(item);
        myItem2.setItem(Recipes.RecipeEnum.MIRROR_CELESTINE);

        myItem2.gameObject.transform.parent = gameObject.transform;
        Item myItem3 = Instantiate(item);
        myItem3.setItem(Recipes.RecipeEnum.APPLEBLOSSOM_TEA);
        
        myItem3.gameObject.transform.parent = gameObject.transform;
        Item myItem4 = Instantiate(item);
        myItem4.setItem(Recipes.RecipeEnum.TEA_LEAF);
        
        myItem4.gameObject.transform.parent = gameObject.transform;
        Item myItem5 = Instantiate(item);
        myItem5.setItem(Recipes.RecipeEnum.SAKURA_BLOSSOMS);

        myItem5.gameObject.transform.parent = gameObject.transform;
        Item myItem6 = Instantiate(item);
        myItem6.setItem(Recipes.RecipeEnum.SAKURA_BLOSSOMS);

        myItem6.gameObject.transform.parent = gameObject.transform;
        Item myItem7 = Instantiate(item);
        myItem7.setItem(Recipes.RecipeEnum.SAKURA_BLOSSOMS);

        myItem7.gameObject.transform.parent = gameObject.transform;
        Item myItem8 = Instantiate(item);
        myItem8.setItem(Recipes.RecipeEnum.RAINBOW_REFRACTOR);

        myItem8.gameObject.transform.parent = gameObject.transform;
        Item myItem9 = Instantiate(item);
        myItem9.setItem(Recipes.RecipeEnum.LOFTY_LEMON);
        
        myItem9.gameObject.transform.parent = gameObject.transform;

        for (int i=0; i < 50; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.RAINBOW_DEWDROP);
           
            myItem.gameObject.transform.parent = gameObject.transform;
        }
    }
    
    public void ShowInventory()
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
            rowCount++;
        }

        int row = 0;
        int column = 0;
        inventoryContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil(rowCount / 7) * 105 + 120f));
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Item myItem = gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            Item newItem = Instantiate(myItem);
            newItem.setItem(myItem.type);
            newItem.originalRef = myItem;

            newItem.gameObject.transform.parent = inventoryContent.transform;
            // now move where it displays
            column++;
            if (column % 7 == 1)
            {
                row++;
                column = 1;
            }
            newItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            newItem.gameObject.transform.localPosition = new Vector2((column * 105f) - 50f, -(row * 105f) + 45f);
        }
    }
}
