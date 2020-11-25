﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingPoint : MonoBehaviour
{
    public MenuActions menu;

    [Header("Gathering Panel")]
    // under the gathering panel
    public GameObject gatheringPanel;
    public Item item1;
    public Item item2;
    public Item item3;
    public Button ready;
    public Button end;
    public Item topItem1;
    public Item topItem2;
    public Item topItem3;
    public TextMeshProUGUI gatherInstructions;
    public Recipes recipes;
    public RecipeManager recipeManager;
    public InventoryManager inventoryManager;
    public GameObject yes;
    public GameObject no;

    [Header("Gathering Recipe")]
    public Recipes.RecipeEnum gatheringRecipe;

    private int outOf1 = 0;
    private int outOf2 = 0;
    private int outOf3 = 0;
    private Recipes.RecipeEnum type1;
    private Recipes.RecipeEnum type2;
    private Recipes.RecipeEnum type3;

    private AudioSource[] audio;

    // Start is called before the first frame update
    void Start()
    {
        if (topItem1 && topItem2 && topItem3)
        {
            topItem1.gameObject.SetActive(false);
            topItem2.gameObject.SetActive(false);
            topItem3.gameObject.SetActive(false);
        }
        audio = GetComponents<AudioSource>();
    }

    public void OnInteract()
    {
        // show intro menu
        ready.gameObject.SetActive(true);
        end.gameObject.SetActive(false);
        gatherInstructions.text = "Let's Get Ready to Gather";
        menu.GetComponent<Animator>().SetBool("gathering", true);
        if (menu.eventSystem != null)
        {
            menu.eventSystem.SetSelectedGameObject(ready.gameObject);
        }
        //ShowRequirements();
    }

    public void ShowRequirements()
    {
        // get the current quest/recipe to make
        if (recipeManager == null && GameObject.FindGameObjectWithTag("RecipeManager") &&
            GameObject.FindGameObjectWithTag("Recipes") && GameObject.FindGameObjectWithTag("InventoryManager"))
        {
            recipeManager = GameObject.FindGameObjectWithTag("RecipeManager").GetComponent<RecipeManager>();
            recipes = GameObject.FindGameObjectWithTag("Recipes").GetComponent<Recipes>();
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }
        if (recipeManager != null && item1 && item2 && item3 && topItem1 && topItem2 && topItem3)
            {
            topItem1.gameObject.SetActive(true);
            topItem2.gameObject.SetActive(true);
            topItem3.gameObject.SetActive(true);

            Recipes.RecipeTypeCount[] items = recipes.getItemsInRecipe(gatheringRecipe);

            if (item1.GetComponent<Item>().type == Recipes.RecipeEnum.NONE)
            {
                type1 = items[0].type;
                item1.setItem(type1, false);
                outOf1 = items[0].count;
                item1.count = outOf1;
                item1.displayCount.text = outOf1.ToString();
            }
            if (item2.GetComponent<Item>().type == Recipes.RecipeEnum.NONE)
            {
                type2 = items[1].type;
                item2.setItem(type2, false);
                outOf2 = items[1].count;
                item2.count = outOf2;
                item2.displayCount.text = outOf2.ToString();
            }
            if (item3.GetComponent<Item>().type == Recipes.RecipeEnum.NONE)
            {
                type3 = items[2].type;
                item3.setItem(type3, false);
                outOf3 = items[2].count;
                item3.count = outOf3;
                item3.displayCount.text = outOf3.ToString();
            }
            if (topItem1.GetComponent<Item>().type == Recipes.RecipeEnum.NONE) topItem1.setItem(items[0].type, false);
            if (topItem2.GetComponent<Item>().type == Recipes.RecipeEnum.NONE) topItem2.setItem(items[1].type, false);
            if (topItem3.GetComponent<Item>().type == Recipes.RecipeEnum.NONE) topItem3.setItem(items[2].type, false);
        }
        if (menu.eventSystem != null)
        {
            menu.eventSystem.SetSelectedGameObject(ready.gameObject);
        }
    }

    public void Proceed()
    {
        menu.GetComponent<Animator>().SetBool("gathering", false);
        // remove collider from this so that player can pass.
        Destroy(GetComponent<Collider>());

        menu.startClock();
    }

    public void EndPanel()
    {
        menu.GetComponent<Animator>().SetBool("menu", false);
        ready.gameObject.SetActive(false);
        end.gameObject.SetActive(true);
        gatherInstructions.text = "All Done Gathering";
        if (menu.eventSystem != null)
        {
            menu.eventSystem.SetSelectedGameObject(end.gameObject);
        }

        item1.count = 0;
        item2.count = 0;
        item3.count = 0;
        item1.displayCount.text = inventoryManager.getCount(type1) + " / " + outOf1;
        item2.displayCount.text = inventoryManager.getCount(type2) + " / " + outOf2;
        item3.displayCount.text = inventoryManager.getCount(type3) + " / " + outOf3;

        // check the inventory if they succeeded
        if (inventoryManager.getCount(type1) >= outOf1 &&
            inventoryManager.getCount(type2) >= outOf2 &&
            inventoryManager.getCount(type3) >= outOf3) {
            yes.SetActive(true);
            no.SetActive(false);
            if (audio != null && audio.Length > 0)
            {
                audio[0].Play();
            }
        }
        else {
            yes.SetActive(false);
            no.SetActive(true);
            if (audio != null && audio.Length > 1)
            {
                audio[1].Play();
            }
        }
        menu.GetComponent<Animator>().SetBool("gathering", true);
    }

    public void Leave()
    {
        menu.GetComponent<Animator>().SetBool("gathering", false);
        if (yes.activeInHierarchy)
        {
            menu.End(true);
        }
        else
        {
            menu.End(false);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (gatheringPanel && gatheringPanel.activeInHierarchy)
        {
            ShowRequirements();
        }
        if (inventoryManager && !inventoryManager.inventoryPanel.activeInHierarchy)
        {
            topItem1.displayCount.text = inventoryManager.getCount(type1) + " / " + outOf1;
            topItem2.displayCount.text = inventoryManager.getCount(type2) + " / " + outOf2;
            topItem3.displayCount.text = inventoryManager.getCount(type3) + " / " + outOf3;
        }
    }
}
