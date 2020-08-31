using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public enum Recipe
    {
        SUPER_DUPER_CRYSTAL,
        ITEM1,
        ITEM2,
        ITEM3
    }
    
    public bool CheckRecipe(string recipe, 
        int capricornValue, int aquariusValue, int piscesValue,
        int ariesValue, int taurusValue, int geminiValue,
        int cancerValue, int leoValue, int virgoValue, 
        int libraValue, int scorpioValue, int sagittariusValue)
    {
        if (recipe.Equals(Recipes.Recipe.SUPER_DUPER_CRYSTAL.ToString()))
        {
            if (capricornValue >= 10 && aquariusValue >= 10)
            {
                Debug.Log("mixing up super duper crystal");
                return true;
            }
            return false;
        }
        if (recipe.Equals(Recipes.Recipe.ITEM1.ToString()))
        {
            Debug.Log("mixing up item1");
            return true;
        }
        if (recipe.Equals(Recipes.Recipe.ITEM2.ToString()))
        {

        }
        if (recipe.Equals(Recipes.Recipe.ITEM3.ToString()))
        {

        }
        return false;
    }
}
