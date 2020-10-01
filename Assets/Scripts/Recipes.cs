using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipes : MonoBehaviour
{
    public Sprite sprite;
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

    public class RecipeTypeCount
    {
        public Recipes.RecipeEnum type;
        public int count;
    }

    public Tuple<string, Image> getItem(Recipes.RecipeEnum type)
    {
        GameObject obj = new GameObject();
        Image image = obj.AddComponent<Image>();
        string title = "";
        if (type == Recipes.RecipeEnum.MIRROR_CELESTINE)
        {
            title = "Mirror Celestine";
            image.sprite = Resources.Load<Sprite>("Icons/gem");
        }
        else if (type == Recipes.RecipeEnum.RAINBOW_DEWDROP)
        {
            title = "Rainbow Dewdrop";
            image.sprite = Resources.Load<Sprite>("Icons/water-drop");
        }
        else if (type == Recipes.RecipeEnum.SAKURA_BLOSSOMS)
        {
            title = "Sakura Blossoms";
            image.sprite = Resources.Load<Sprite>("Icons/cherry-blossom");
        }
        else if (type == Recipes.RecipeEnum.LOFTY_LEMON)
        {
            title = "Lofty Lemon";
            image.sprite = Resources.Load<Sprite>("Icons/lemon");
        }
        else if (type == Recipes.RecipeEnum.TEA_LEAF)
        {
            title = "Tea Leaf";
            image.sprite = Resources.Load<Sprite>("Icons/green-tea");
        }
        else if (type == Recipes.RecipeEnum.GOLDEN_APPLE)
        {
            title = "Golden Apple";
            image.sprite = Resources.Load<Sprite>("Icons/apple");
        }
        else if (type == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
        {
            title = "Rainbow Refractor";
            image.sprite = Resources.Load<Sprite>("Icons/diaphragm");
        }
        else if (type == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
        {
            title = "Appleblossom Tea";
            image.sprite = Resources.Load<Sprite>("Icons/hot-cup");
        }
        return Tuple.Create(title, image);
    }

    public RecipeTypeCount[] getItemsInRecipe(RecipeEnum recipe)
    {
        RecipeTypeCount[] items = new RecipeTypeCount[3];
        RecipeTypeCount typeCount1 = new RecipeTypeCount();
        RecipeTypeCount typeCount2 = new RecipeTypeCount();
        RecipeTypeCount typeCount3 = new RecipeTypeCount();
        RecipeEnum type1 = Recipes.RecipeEnum.NONE;
        RecipeEnum type2 = Recipes.RecipeEnum.NONE;
        RecipeEnum type3 = Recipes.RecipeEnum.NONE;
        int count1 = 1;
        int count2 = 1;
        int count3 = 1;
        if (recipe == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
        {
            type1 = Recipes.RecipeEnum.MIRROR_CELESTINE;
            type2 = Recipes.RecipeEnum.RAINBOW_DEWDROP;
            type3 = Recipes.RecipeEnum.SAKURA_BLOSSOMS;
            count1 = 4;
            count2 = 8;
            count3 = 2;
        }
        if (recipe == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
        {
            type1 = Recipes.RecipeEnum.GOLDEN_APPLE;
            type2 = Recipes.RecipeEnum.LOFTY_LEMON;
            type3 = Recipes.RecipeEnum.TEA_LEAF;
            count1 = 10;
            count2 = 25;
            count3 = 2;
        }

        typeCount1.type = type1;
        typeCount2.type = type2;
        typeCount3.type = type3;
        typeCount1.count = count1;
        typeCount2.count = count2;
        typeCount3.count = count3;
        items[0] = typeCount1;
        items[1] = typeCount2;
        items[2] = typeCount3;
        return items;
    }

    public bool CheckRecipe(RecipeEnum recipe, Item slot1, Item slot2, Item slot3)
    {
        RecipeTypeCount[] items = getItemsInRecipe(recipe);

        Debug.Log(items[0].type);
        Debug.Log(slot1.type);
        Debug.Log(items[1].type);
        Debug.Log(slot2.type);
        Debug.Log(items[2].type);
        Debug.Log(slot3.type);

        Debug.Log(items[0].count);
        Debug.Log(slot1.count);
        Debug.Log(items[1].count);
        Debug.Log(slot2.count);
        Debug.Log(items[2].count);
        Debug.Log(slot3.count);

        if (items[0].type == slot1.type && items[1].type == slot2.type && items[2].type == slot3.type)
        {
            if (items[0].count <= slot1.count && items[1].count <= slot2.count && items[2].count <= slot3.count)
            {
                return true;
            }
        }
        return false;
    }
}
