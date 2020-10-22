using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsDisplay : MonoBehaviour
{
    public Image displayImage;
    public TextMeshProUGUI displayText;
    public string title;
    public Image image;
    public Recipes recipes;
    public GameObject yes;
    public GameObject no;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setItem(Recipes.RecipeEnum type)
    {
        Tuple<string, Image> item = recipes.getItem(type);
        if (item != null)
        {
            title = item.Item1;
            image = item.Item2;
        }
    }

    public void ShowSuccess()
    {
        yes.SetActive(true);
        no.SetActive(false);
    }

    public void ShowFailure()
    {
        yes.SetActive(false);
        no.SetActive(true);
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
            displayImage.sprite = image.sprite;
        }
    }
}
