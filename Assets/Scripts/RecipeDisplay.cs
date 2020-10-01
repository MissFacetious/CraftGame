using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{ 
    public Recipes recipes;
    public Image displayImage;
    public TextMeshProUGUI displayText;
    public Item item1;
    public Item item2;
    public Item item3;
    public Image image1;
    public Image image2;
    public Image image3;
    public string title;
    public Image image;
    public RecipeManager recipeManager;
    public Recipes.RecipeEnum type;
    public int item1count;
    public int item2count;
    public int item3count;
    public float item1percent;
    public float item2percent;
    public float item3percent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateRecipe()
    {
        type = recipeManager.getCurrentRecipe();

        Tuple<string, Image> item = recipes.getItem(type);
        Recipes.RecipeTypeCount[] recipeTypeCount = recipes.getItemsInRecipe(type);
        if (item != null)
        {
            title = item.Item1;
            image = item.Item2;
        }
        if (recipeTypeCount.Length > 0 && recipeTypeCount[0].count > 0)
        {
            item1percent = (0.333333f * item1.count / recipeTypeCount[0].count);
            image1.GetComponent<Image>().fillAmount = item1percent;
        }
        if (recipeTypeCount.Length > 1 && recipeTypeCount[1].count > 0)
        {
            item2percent = (0.333333f * item2.count / recipeTypeCount[1].count);
            image2.GetComponent<Image>().fillAmount = item2percent;
        }
        if (recipeTypeCount.Length > 2 && recipeTypeCount[2].count > 0)
        {
            item3percent = (0.333333f * item3.count / recipeTypeCount[2].count);
            image3.GetComponent<Image>().fillAmount = item3percent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRecipe();
        if (displayText != null)
        {
            displayText.text = title;
        }
        if (displayImage != null)
        {
            displayImage.sprite = image.sprite;
        }

        image1.GetComponent<Image>().fillAmount = item1percent;
        image2.GetComponent<Image>().fillAmount = item2percent;
        image3.GetComponent<Image>().fillAmount = item3percent;

    }
}
