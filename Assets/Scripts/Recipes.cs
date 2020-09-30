using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public enum RecipeEnum
    {
        NONE,
        MIRROR_CELESTINE,
        RAINBOW_DEWDROP,
        SAKURA_BLOSSOMS,
        RAINBOW_REFRACTOR,
        GOLDEN_APPLE,
        LOFTY_LEMON,
        TEA_LEAF,
        APPLEBLOSSOM_TEA,
    }
    
    public RecipeEnum[] getItemsInRecipe(RecipeEnum recipe)
    {
        RecipeEnum[] items = new RecipeEnum[3];
        RecipeEnum item1 = Recipes.RecipeEnum.NONE;
        RecipeEnum item2 = Recipes.RecipeEnum.NONE;
        RecipeEnum item3 = Recipes.RecipeEnum.NONE;
        if (recipe == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
        {
            item1 = Recipes.RecipeEnum.MIRROR_CELESTINE;
            item2 = Recipes.RecipeEnum.RAINBOW_DEWDROP;
            item3 = Recipes.RecipeEnum.SAKURA_BLOSSOMS;
            Debug.Log("in rainbow refractor");
            Debug.Log(item1);
            Debug.Log(item2);
            Debug.Log(item3);
        }
        if (recipe == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
        {
            item1 = Recipes.RecipeEnum.GOLDEN_APPLE;
            item2 = Recipes.RecipeEnum.LOFTY_LEMON;
            item3 = Recipes.RecipeEnum.TEA_LEAF;
        }
        items[0] = item1;
        items[1] = item2;
        items[2] = item3;
        return items;
    }

    public bool CheckRecipe(RecipeEnum recipe, Item slot1, Item slot2, Item slot3)
    {
        RecipeEnum[] items = getItemsInRecipe(recipe);

        Debug.Log(items[0]);
        Debug.Log(slot1);
        Debug.Log(items[1]);
        Debug.Log(slot2);
        Debug.Log(items[2]);
        Debug.Log(slot3);

        if (items[0] == slot1.type && items[1] == slot2.type && items[2] == slot3.type)
        {
            return true;
        }
        return false;
    }
}
