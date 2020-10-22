using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    public Recipes.RecipeEnum type;
    public string title;
    public Sprite image;

    public TextMeshProUGUI count;
    public TextMeshProUGUI displayText;
    public Image displayImage;
    public Recipe originalRef;

    // Start is called before the first frame update
    void Start()
    {
        if (displayText == null)
        {
            Debug.Log("map your text");
        }
        if (displayImage == null)
        {
            Debug.Log("map your image");
        }
    }

    public void setRecipe(Recipes.RecipeEnum myType)
    {
        type = myType;
        if (type == Recipes.RecipeEnum.RAINBOW_REFRACTOR)
        {
            title = "Rainbow Refractor";
            image = Resources.Load<Sprite>("Icons/diaphragm");
        }
       // else if (type == Recipes.RecipeEnum.APPLEBLOSSOM_TEA)
       // {
       //     title = "Apple Blossom Tea";
       //     image = Resources.Load<Sprite>("Icons/hot-cup");
       // }
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
