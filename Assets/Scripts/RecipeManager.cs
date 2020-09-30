using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        Recipe myRecipe1 = Instantiate(recipe);
        myRecipe1.setRecipe(Recipes.RecipeEnum.RAINBOW_REFRACTOR);
        myRecipe1.gameObject.transform.parent = gameObject.transform;
        Recipe myRecipe2 = Instantiate(recipe);
        myRecipe2.setRecipe(Recipes.RecipeEnum.APPLEBLOSSOM_TEA);
        myRecipe2.gameObject.transform.parent = gameObject.transform;
    }

    public void ShowRecipes()
    {
        Debug.Log("show recipes");
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
        // slot1 and slot2 ch;ange
        Debug.Log(currentRecipe);
        Recipes.RecipeTypeCount[] items = recipes.getItemsInRecipe(currentRecipe);
        Debug.Log(items[0].type);
        item1.GetComponent<Item>().setItem(items[0].type);
        item2.GetComponent<Item>().setItem(items[1].type);
        item3.GetComponent<Item>().setItem(items[2].type);
    }

    public void setCurrentRecipe(Recipes.RecipeEnum type)
    {
        currentRecipe = type;
    }

    public Recipes.RecipeEnum getCurrentRecipe()
    {
        return currentRecipe;
    }
}
