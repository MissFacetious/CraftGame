using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Recipes.RecipeEnum type;
    public string title;
    public Sprite image;
    
    public TextMeshProUGUI count;
    public TextMeshProUGUI displayText;
    public Image displayImage;
    public bool available;
    public Item originalRef;

    // Start is called before the first frame update
    void Start()
    {
        available = true;
        if (displayText == null)
        {
            Debug.Log("map your text");
        }
        if (displayImage == null)
        {
            Debug.Log("map your image");
        }
    }

    public void setItem(Recipes.RecipeEnum myType)
    {
        type = myType;
        if (type == Recipes.RecipeEnum.MIRROR_CELESTINE) {
            title = "Mirror Celestine";
            image = Resources.Load<Sprite>("Icons/gem");
        }
        else if (type == Recipes.RecipeEnum.RAINBOW_DEWDROP) {
            title = "Rainbow Dewdrop";
            image = Resources.Load<Sprite>("Icons/water-drop");
        }
        else if (type == Recipes.RecipeEnum.SAKURA_BLOSSOMS) {
            title = "Sakura Blossoms";
            image = Resources.Load<Sprite>("Icons/cherry-blossom");
        }
        else if (type == Recipes.RecipeEnum.LOFTY_LEMON) {
            title = "Lofty Lemon";
            image = Resources.Load<Sprite>("Icons/lemon");
        }
        else if (type == Recipes.RecipeEnum.TEA_LEAF) {
            title = "Tea Leaf";
            image = Resources.Load<Sprite>("Icons/green-tea");
        }
        else if (type == Recipes.RecipeEnum.GOLDEN_APPLE) {
            title = "Golden Apple";
            image = Resources.Load<Sprite>("Icons/apple");
        }
        else if (type == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
        {
            title = "Rainbow Refractor";
            image = Resources.Load<Sprite>("Icons/diaphragm");
        }
        else if (type == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
        {
            title = "Appleblossom Tea";
            image = Resources.Load<Sprite>("Icons/hot-cup");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (displayText != null)
        {
            displayText.text = title;
        }
        if (displayImage != null)
        {
            displayImage.sprite = image;
        }
    }
}
