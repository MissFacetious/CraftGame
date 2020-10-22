using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class InventoryManager : MonoBehaviour, IComparer
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

        for (int i = 0; i < 10; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.MIRROR_CELESTINE, false);
            myItem.gameObject.transform.parent = gameObject.transform;
        }


        for (int i = 0; i < 10; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.SAKURA_BLOSSOMS, false);
            myItem.gameObject.transform.parent = gameObject.transform;
        }

        for (int i = 0; i < 10; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.RAINBOW_DEWDROP, false);
            myItem.gameObject.transform.parent = gameObject.transform;
        }
    }

    public Item CreateNewItem(Recipes.RecipeEnum type, bool bundle)
    {
        // make a new item and put into inventory!
        Item myItem = Instantiate(item);

        myItem.setItem(type, bundle);

        myItem.gameObject.transform.parent = gameObject.transform;
        myItem.gameObject.transform.localPosition = Vector2.zero;

        return myItem;
    }

    // TODO the sorter does nothing
    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
    }

    // TODO the sorter does nothing
    public GameObject[] Sort()
    {
        ArrayList array = new ArrayList();
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < ts.Length; i++)
        {
            array.Add(ts[i].gameObject);//.GetComponent<Item>();
        }
        //IComparer myComparer = new InventoryManager();
        GameObject[] gameObjects = (GameObject[])array.ToArray(typeof(GameObject));
        //Array.Sort(gameObjects, myComparer);

        return gameObjects;
    }

    public ArrayList Bundlize()
    {
        // find counts for each separate item type
        Dictionary<Recipes.RecipeEnum, int> inventoryMap = new Dictionary<Recipes.RecipeEnum, int>();
        ArrayList inventoryList = new ArrayList();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            int count = 1;
            int oldCount = 0;
            if (inventoryMap.ContainsKey(gameObject.transform.GetChild(i).gameObject.GetComponent<Item>().type))
            {
                inventoryMap.TryGetValue(gameObject.transform.GetChild(i).gameObject.GetComponent<Item>().type, out oldCount);
                count = oldCount + 1;
                if (gameObject.transform.GetChild(i).gameObject.GetComponent<Item>().inBundle())
                {
                    count = count + 10;
                }
                inventoryMap[gameObject.transform.GetChild(i).gameObject.GetComponent<Item>().type] = count;
            }
            else
            {
                inventoryMap.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<Item>().type, count);
            }
            inventoryList.Add(gameObject.transform.GetChild(i).gameObject.GetComponent<Item>());
        }

        Recipes.RecipeEnum[] AllTypes = new Recipes.RecipeEnum[inventoryMap.Keys.Count];
        ArrayList bundledInventoryList = new ArrayList();
        inventoryMap.Keys.CopyTo(AllTypes, 0);
        
        for (int i = 0; i < AllTypes.Length; i++)
        {
            if (AllTypes[i] != Recipes.RecipeEnum.NONE)
            {
                int count = 0;
                inventoryMap.TryGetValue(AllTypes[i], out count);
                // take this one type of item and bundle it based on bundles of 10
                // these are accurate numbers for bundle and remaider
                int bundles = Mathf.FloorToInt(count / 10);
                //int remainder = count % 10;

                // get this type of item from
                int amount = 0;
                for (int k = 0; k < inventoryList.Count; k++)
                {
                    Item myItem = (Item)inventoryList[k];
                    if (myItem.type == (Recipes.RecipeEnum)AllTypes[i])
                    {
                        if (myItem.inBundle())
                        {
                            bundledInventoryList.Add(myItem);
                            amount = amount + 10;
                        }
                        else if (bundles > 0 && amount < (10 * bundles) && amount % 10 == 0)
                        {
                            myItem.bundle = true;
                            myItem.count = 10;
                            bundledInventoryList.Add(myItem);
                        }
                        else if (bundles > 0 && amount < (10 * bundles))
                        {
                            Destroy(myItem.gameObject);
                        }
                        else
                        {
                            bundledInventoryList.Add(myItem);
                        }
                        amount++;
                    }
                }
            }
        }
        return bundledInventoryList;
    }

    public void ShowInventory()
    {
        // first, we need to know how big the panel needs to be, so I need to calculate the rowCount
        ArrayList inventoryList = Bundlize();
        int rowCount = inventoryList.Count;
        int row = 0;
        int column = 0;

        inventoryContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil(rowCount / 7) * 105 + 120f));
        for (int i = 0; i < inventoryList.Count; i++)
        {
            Item myItem = (Item)inventoryList[i];
            myItem.gameObject.transform.parent = inventoryContent.transform;
            // now move where it displays
            column++;
            if (column % 7 == 1)
            {
                row++;
                column = 1;
            }
            myItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            myItem.gameObject.transform.localPosition = new Vector2((column * 105f) - 50f, -(row * 105f) + 45f);
        }
    }

    public int getCount(Recipes.RecipeEnum type)
    {
        int count = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Item myItem = gameObject.transform.GetChild(i).GetComponent<Item>();            
            if (myItem.type == type)
            {
                count = count + myItem.count;
            }
        }
        return count;
    }

    public void HideInventory()
    {
        int size = inventoryContent.transform.childCount;
        for (int i=0; i < size; i++)
        {
            inventoryContent.transform.GetChild(0).parent = gameObject.transform;
        }
    }
}