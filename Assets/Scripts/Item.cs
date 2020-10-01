using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Item : MonoBehaviour
{
    public Recipes.RecipeEnum type;
    public Recipes recipes;
    public string title;
    public Sprite image;
    public int count;
    public TextMeshProUGUI displayCount;
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
        Tuple<string, Image> item = recipes.getItem(myType);
        title = item.Item1;
        image = item.Item2.sprite;
        type = myType;
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
        if (displayCount != null && count > 0)
        {
            displayCount.text = count.ToString();
        }
    }
}
