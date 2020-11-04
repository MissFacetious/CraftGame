using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Fungus;

public class RecipeManager : MonoBehaviour
{
    [Header("Recipe Panel")]
    // items in the recipes panel
    public GameObject recipeContent;
    public PanelManager panelManager;
    public Recipe recipe;
    public Recipes recipes;
    public Recipes.RecipeEnum currentRecipe;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    public EventSystem eventSystem;

    bool alreadyInThere(Recipes.RecipeEnum type)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Recipe myRecipe = gameObject.transform.GetChild(i).gameObject.GetComponent<Recipe>();
            // is this a recipe we should be showing
            if (myRecipe.type == type)
            {
                return true;
            }
        }
        return false;
    }

    void createRecipesInPanel()
    {
        ArrayList rs = new ArrayList();

        // have conditionals here based on the fugus variables if we have processed enough to have these recipes

        Flowchart flowchart = eventSystem.GetComponentInChildren<Flowchart>();
        if (flowchart != null)
        {
            if (flowchart.GetStringVariable("hoshi_state") == "GATHERING_SUCCEEDED") {
                rs.Add(Recipes.RecipeEnum.RAINBOW_REFRACTOR);
            }
            if (flowchart.GetStringVariable("hawking_state") == "GATHERING_SUCCEEDED") {
                rs.Add(Recipes.RecipeEnum.APPLEBLOSSOM_TEA);
            }
            if (flowchart.GetStringVariable("ivy_state") == "GATHERING_SUCCEEDED") {
                rs.Add(Recipes.RecipeEnum.TRANSFORMATIONAL_POTION);
            }
            if (flowchart.GetStringVariable("greene_state") == "GATHERING_SUCCEEDED") {
                rs.Add(Recipes.RecipeEnum.GNOME_NET);
            }
        }  

        for (int i = 0; i < rs.Count; i++) {
            if (!alreadyInThere((Recipes.RecipeEnum)rs[i]))
            {
                Recipe myRecipe = Instantiate(recipe);
                myRecipe.setRecipe((Recipes.RecipeEnum)rs[i]);
                myRecipe.gameObject.transform.parent = gameObject.transform;
            }
        }
    }

    public void ShowRecipes()
    {
        // show recipes that are accessible at this point of the game
        createRecipesInPanel();

        // first, we need to know how big the panel needs to be, so I need to calculate the rowCount
        int rowCount = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Recipe myRecipe = gameObject.transform.GetChild(i).gameObject.GetComponent<Recipe>();
            rowCount++;
        }

        int row = 0;
        int column = 0;
        recipeContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil(rowCount / 3) * 255 + 250f));
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Recipe myRecipe = gameObject.transform.GetChild(i).gameObject.GetComponent<Recipe>();
            Recipe newRecipe = Instantiate(myRecipe);
            newRecipe.setRecipe(myRecipe.type);
            newRecipe.originalRef = myRecipe;
            newRecipe.GetComponent<Button>().onClick.AddListener(
                    delegate {
                        currentRecipe = myRecipe.type;
                        panelManager.OpenRecipePanel(myRecipe.type);
                    });
            newRecipe.gameObject.transform.parent = recipeContent.transform;
            // now move where it displays
            column++;
            if (column % 3 == 1)
            {
                row++;
                column = 1;
            }
            newRecipe.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            newRecipe.gameObject.transform.localPosition = new Vector2((column * 255f) - 125f, -(row * 255f) + 120f);
        }
    }

    public void ShowRecipeItems()
    {
        Recipes.RecipeTypeCount[] items = recipes.getItemsInRecipe(currentRecipe);
        item1.GetComponent<Item>().setItem(items[0].type, false);
        item2.GetComponent<Item>().setItem(items[1].type, false);
        item3.GetComponent<Item>().setItem(items[2].type, false);
    }

    public void setCurrentRecipe(Recipes.RecipeEnum type)
    {
        currentRecipe = type;
    }

    public Recipes.RecipeEnum getCurrentRecipe()
    {
        return currentRecipe;
    }

    void getEventSystem()
    {
        if (GameObject.FindGameObjectWithTag("EventSystem") != null)
        {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        }
        else
        {
            Debug.Log("event system is not hooked up.");
        }
    }


    public void Update()
    {
        if (eventSystem == null)
        {
            getEventSystem();
        }
    }
}
