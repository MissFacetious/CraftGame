using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingPoint : MonoBehaviour
{
    public MenuActions menu;
    public Item item1;
    public Item item2;
    public Item item3;
    public Button ready;
    public Button end;
    public Item topItem1;
    public Item topItem2;
    public Item topItem3;
    public Recipes recipes;
    public RecipeManager recipeManager;

    private int outOf1 = 0;
    private int outOf2 = 0;
    private int outOf3 = 0;

    // Start is called before the first frame update
    void Start()
    {
        // get the current quest/recipe to make
        if (recipeManager == null) {
            recipeManager = GameObject.FindGameObjectWithTag("RecipeManager").GetComponent<RecipeManager>();
            recipes = GameObject.FindGameObjectWithTag("Recipes").GetComponent<Recipes>();
        }
        Recipes.RecipeEnum currentRecipe = recipeManager.getCurrentRecipe();
        // just use this until we set the proper actual one for the quest
        Recipes.RecipeTypeCount[] items = recipes.getItemsInRecipe(Recipes.RecipeEnum.RAINBOW_REFRACTOR);

        Debug.Log(items[0].type);
        Debug.Log(items[1].type);
        Debug.Log(items[2].type);
        
        item1.setItem(items[0].type, false);
        item2.setItem(items[1].type, false);
        item3.setItem(items[2].type, false);
        item1.count = items[0].count;
        item2.count = items[1].count;
        item3.count = items[2].count;

        topItem1.setItem(items[0].type, false);
        topItem2.setItem(items[1].type, false);
        topItem3.setItem(items[2].type, false);
        outOf1 = items[0].count;
        outOf2 = items[1].count;
        outOf3 = items[2].count;

        // set this in the menu as well on the top panel
    }

    public void OnInteract()
    {
        Debug.Log(name + "'s interaction method");
        // show intro menu
        ready.gameObject.SetActive(true);
        end.gameObject.SetActive(false);
        menu.GetComponent<Animator>().SetBool("gathering", true);
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
        menu.GetComponent<Animator>().SetBool("gathering", true);
    }

    public void Leave()
    {
        menu.GetComponent<Animator>().SetBool("gathering", false);
        menu.End();
    }

    // Update is called once per frame
    void Update()
    {
        topItem1.displayCount.text = " / " + outOf1;
        topItem2.displayCount.text = " / " + outOf2;
        topItem3.displayCount.text = " / " + outOf3;
    }
}
