using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    
    public string title;
    public Sprite image;
    public TextMeshProUGUI count;
    public TextMeshProUGUI display;
    public bool available;
    public Item originalRef;

    public int capricorn;
    public int aquarius;
    public int pisces;
    public int aries;
    public int taurus;
    public int gemini;
    public int cancer;
    public int leo;
    public int virgo;
    public int libra;
    public int scorpio;
    public int sagittarius;



    // Start is called before the first frame update
    void Start()
    {
        available = true;   
    }

    // Update is called once per frame
    void Update()
    {
        display.text = title;

    }
}
