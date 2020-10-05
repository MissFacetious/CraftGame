﻿using System.Collections;
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
        Item myItem1 = Instantiate(item);
        myItem1.setItem(Recipes.RecipeEnum.APPLEBLOSSOM_TEA, false);
        myItem1.gameObject.transform.parent = gameObject.transform;
        Item myItem2 = Instantiate(item);
        myItem2.setItem(Recipes.RecipeEnum.MIRROR_CELESTINE, false);
        myItem2.gameObject.transform.parent = gameObject.transform;

        for (int i = 0; i < 15; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.APPLEBLOSSOM_TEA, false);
            myItem.gameObject.transform.parent = gameObject.transform;
        }

        Item myItem4 = Instantiate(item);
        myItem4.setItem(Recipes.RecipeEnum.TEA_LEAF, false);
        myItem4.gameObject.transform.parent = gameObject.transform;

        Item myItem5 = Instantiate(item);
        myItem5.setItem(Recipes.RecipeEnum.LOFTY_LEMON, false);
        myItem5.gameObject.transform.parent = gameObject.transform;

        for (int i = 0; i < 25; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.SAKURA_BLOSSOMS, false);
            myItem.gameObject.transform.parent = gameObject.transform;
        }

        Item myItem7 = Instantiate(item);
        myItem7.setItem(Recipes.RecipeEnum.TEA_LEAF, false);
        myItem7.gameObject.transform.parent = gameObject.transform;

        for (int i = 0; i < 30; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.RAINBOW_REFRACTOR, false);
            myItem.gameObject.transform.parent = gameObject.transform;
        }

        Item myItem9 = Instantiate(item);
        myItem9.setItem(Recipes.RecipeEnum.LOFTY_LEMON, false);
        myItem9.gameObject.transform.parent = gameObject.transform;

        Item myItem10 = Instantiate(item);
        myItem10.setItem(Recipes.RecipeEnum.GOLDEN_APPLE, false);
        myItem10.gameObject.transform.parent = gameObject.transform;

        Item myItem11 = Instantiate(item);
        myItem11.setItem(Recipes.RecipeEnum.GOLDEN_APPLE, false);
        myItem11.gameObject.transform.parent = gameObject.transform;

        for (int i = 0; i < 15; i++)
        {
            Item myItem = Instantiate(item);
            myItem.setItem(Recipes.RecipeEnum.RAINBOW_DEWDROP, false);

            myItem.gameObject.transform.parent = gameObject.transform;
        }
    }

    // Calls CaseInsensitiveComparer.Compare on the monster name string.
    int IComparer.Compare(System.Object x, System.Object y)
    {
        return ((new CaseInsensitiveComparer()).Compare(((GameObject)x).name, ((GameObject)y).name));
    }

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

        Recipes.RecipeEnum[] AllTypesTemp = new Recipes.RecipeEnum[inventoryMap.Keys.Count];
        inventoryMap.Keys.CopyTo(AllTypesTemp, 0);

        ArrayList AllTypes = new ArrayList();
        for (int i = 0; i < AllTypesTemp.Length; i++)
        {
            if (AllTypesTemp[i] != Recipes.RecipeEnum.NONE)
            {
                int count = 0;
                AllTypes.Add(AllTypesTemp[i]);
                inventoryMap.TryGetValue(AllTypesTemp[i], out count);

                // take this one type of item and bundle it based on bundles of 10
                // these are accurate numbers for bundle and remaider
                int bundles = Mathf.FloorToInt(count / 10);
                int remainder = count % 10;

                // grab 10 of that type, and destroy 9, bundle 1
                for (int j = 0; j < bundles; j++)
                {
                    // get this type of item from
                    int amount = 0;
                    for (int k = 0; k < inventoryList.Count; k++)
                    {
                        Item myItem = (Item)inventoryList[k];
                        if (!myItem.inBundle() && myItem.type == (Recipes.RecipeEnum)AllTypesTemp[i])
                        {
                            Debug.Log("found same type");
                            if (amount == 0)
                            {
                                myItem.bundle = true;
                                inventoryList[k] = myItem;
                                amount++;
                            }
                            else if (amount < 10)
                            {
                                inventoryList.RemoveAt(k);
                                Destroy(myItem.gameObject);
                                amount++;
                            }
                        }
                    }
                }
            }
        }
        return inventoryList;
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

    public void HideInventory()
    {
        int size = inventoryContent.transform.childCount;
        for (int i=0; i < size; i++)
        {
            inventoryContent.transform.GetChild(0).parent = gameObject.transform;
        }
    }
}