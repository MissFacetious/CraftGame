﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

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
    public bool bundle = false;
    public GameObject displayBundleImage;
    public bool inGathering;

    // Start is called before the first frame update
    void Awake()
    {
        bundle = false;
        if (displayText == null)
        {
            Debug.Log("map your text");
        }
        if (displayImage == null)
        {
            Debug.Log("map your image");
        }
        if (displayCount == null)
        {
        //    Debug.Log("map your text");
        }
        if (recipes == null && GameObject.FindGameObjectWithTag("Recipes"))
        {
            recipes = GameObject.FindGameObjectWithTag("Recipes").GetComponent<Recipes>();
        }
    }

    public void setItem(Recipes.RecipeEnum myType, bool myBundle)
    {
        Tuple<string, Image> item = recipes.getItem(myType);
        title = item.Item1;
        image = item.Item2.sprite;
        type = myType;
        bundle = myBundle;
    }

    public bool inBundle()
    {
        return bundle;
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
        if (displayCount != null && !inGathering) {
            if (count > 0)
            {
                displayCount.text = count.ToString();
            }
            else
            {
                displayCount.text = "";
            }
        }
        if (bundle && displayBundleImage != null)
        {
            displayBundleImage.gameObject.SetActive(true);
        }
        else if (displayBundleImage != null)
        {
            displayBundleImage.gameObject.SetActive(false);
        }    
    }
}
