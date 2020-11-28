using System;
using UnityEngine;
using UnityEngine.UI;

public class Recipes : MonoBehaviour
{
    // this is the file you can add recipe information to!
    // add one recipe enum
    // add a title and sprite
    // add a recipe with three items with their numeric count
    public Image image;

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
        GNOME_NET,
        VINES,
        SHINY_STONE,
        LAVENDER_SCENT,
        ORANGE_POWDER,
        PINK_CRYSTAL,
        FRESH_STREAM_WATER,
        TRANSFORMATIONAL_POTION
    }

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public class RecipeTypeCount
    {
        public Recipes.RecipeEnum type;
        public int count;
    }

    public Tuple<string, Image> getItem(Recipes.RecipeEnum type)
    {
        string title = "";
        if (type == Recipes.RecipeEnum.MIRROR_CELESTINE)
        {
            title = "Mirror Celestine";
            image.sprite = Resources.Load<Sprite>("Icons/diamond");
        }
        else if (type == Recipes.RecipeEnum.RAINBOW_DEWDROP)
        {
            title = "Dewdrop Prism";
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
            title = "Apple Blossom Tea";
            image.sprite = Resources.Load<Sprite>("Icons/hot-cup");
        }
        else if (type == Recipes.RecipeEnum.TRANSFORMATIONAL_POTION)
        {
            title = "Transformation Potion";
            image.sprite = Resources.Load<Sprite>("Icons/potion");
        }
        else if (type == Recipes.RecipeEnum.GNOME_NET)
        {
            title = "Gnome Gnet";
            image.sprite = Resources.Load<Sprite>("Icons/net");
        }
        else if (type == Recipes.RecipeEnum.ORANGE_POWDER)
        {
            title = "Orange Powder";
            image.sprite = Resources.Load<Sprite>("Icons/flower");
        }
        else if (type == Recipes.RecipeEnum.PINK_CRYSTAL)
        {
            title = "Pink Crystal";
            image.sprite = Resources.Load<Sprite>("Icons/gem");
        }
        else if (type == Recipes.RecipeEnum.FRESH_STREAM_WATER)
        {
            title = "Fresh Stream Water";
            image.sprite = Resources.Load<Sprite>("Icons/splash");
        }
        else if (type == Recipes.RecipeEnum.VINES)
        {
            title = "Vines";
            image.sprite = Resources.Load<Sprite>("Icons/vine");
        }
        else if (type == Recipes.RecipeEnum.SHINY_STONE)
        {
            title = "Shiny Stone";
            image.sprite = Resources.Load<Sprite>("Icons/stone");
        }
        else if (type == Recipes.RecipeEnum.LAVENDER_SCENT)
        {
            title = "Lavender Scent";
            image.sprite = Resources.Load<Sprite>("Icons/lotus");
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
            type2 = Recipes.RecipeEnum.SAKURA_BLOSSOMS;
            type3 = Recipes.RecipeEnum.RAINBOW_DEWDROP;
            count1 = 6;
            count2 = 12;
            count3 = 6;
        }
        if (recipe == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
        {
            type1 = Recipes.RecipeEnum.GOLDEN_APPLE;
            type2 = Recipes.RecipeEnum.TEA_LEAF;
            type3 = Recipes.RecipeEnum.LOFTY_LEMON;
            count1 = 5;
            count2 = 10;
            count3 = 5;
        }
        if (recipe == Recipes.RecipeEnum.TRANSFORMATIONAL_POTION)
        {
            type1 = Recipes.RecipeEnum.FRESH_STREAM_WATER;
            type2 = Recipes.RecipeEnum.ORANGE_POWDER;
            type3 = Recipes.RecipeEnum.PINK_CRYSTAL;
            count1 = 8;
            count2 = 8;
            count3 = 8;
        }
        if (recipe == Recipes.RecipeEnum.GNOME_NET)
        {
            type1 = Recipes.RecipeEnum.VINES;
            type2 = Recipes.RecipeEnum.SHINY_STONE;
            type3 = Recipes.RecipeEnum.LAVENDER_SCENT;
            count1 = 8;
            count2 = 8;
            count3 = 12;
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
