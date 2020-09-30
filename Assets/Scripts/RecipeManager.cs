using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public CraftingManager craftingManager;
    public GameObject recipeContent;
    public Recipe recipe;

    [Header("UI Panels")]
    // panels to show at certain states
    public GameObject recipePanel;
    public GameObject craftPanel;
    public GameObject resultPanel;
    public GameObject inventoryPanel;

    //[Header("Recipe Panel")]
    // things in the recipe panel


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
                        OpenRecipe();
                        craftingManager.OpenRecipe(newRecipe.type);
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

    public void OpenRecipe()
    {
        // remove any panels for crafting
        inventoryPanel.SetActive(false);
        recipePanel.SetActive(false);
        craftPanel.SetActive(true);
        resultPanel.SetActive(false);
        //  recipePanel.SetActive(false);
        //  craftPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
