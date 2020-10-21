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
    public Recipes recipes;
    public RecipeManager recipeManager;

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

        // set this in the menu as well on the top panel
    }

    public void OnInteract()
    {
        Debug.Log(name + "'s interaction method");
        // show intro menu
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
        
    }
}
