using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public enum RecipeEnum
    {
        MIRROR_CELESTINE,
        RAINBOW_DEWDROP,
        SAKURA_BLOSSOMS,
        RAINBOW_REFRACTOR,
        GOLDEN_APPLE,
        LOFTY_LEMON,
        TEA_LEAF,
        APPLEBLOSSOM_TEA,
        SUPER_DUPER_CRYSTAL,
        ITEM1,
        ITEM2,
        ITEM3
    }
    
    public bool CheckRecipe(RecipeEnum recipe, 
        int capricornValue, int aquariusValue, int piscesValue,
        int ariesValue, int taurusValue, int geminiValue,
        int cancerValue, int leoValue, int virgoValue, 
        int libraValue, int scorpioValue, int sagittariusValue)
    {
        if (recipe.Equals(Recipes.RecipeEnum.SUPER_DUPER_CRYSTAL.ToString()))
        {
            if (capricornValue >= 10 && aquariusValue >= 10)
            {
                Debug.Log("mixing up super duper crystal");
                return true;
            }
            return false;
        }
        if (recipe == Recipes.RecipeEnum.ITEM1)
        {
            Debug.Log("mixing up item1");
            return true;
        }
        if (recipe == Recipes.RecipeEnum.ITEM2)
        {
            Debug.Log("mixing up item1");
            return true;
        }
        if (recipe == Recipes.RecipeEnum.ITEM3)
        {
            Debug.Log("mixing up item1");
            return true;
        }
        if (recipe == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
        {
            Debug.Log("mixing up rainbow refractor");
            return true;
        }
        if (recipe == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
        {
            Debug.Log("mixing up appleblossom tea");
            return true;
        }
        return false;
    }
}
