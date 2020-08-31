using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public class Action
    {
        public ActionTaken actionTaken;
        public GameObject item;
        public GameObject itemSlot;
    }
    public Recipes recipes;

    [Header("UI Panels")]
    // panels to show at certain states
    public GameObject recipePanel;
    public GameObject craftPanel;
    public GameObject resultPanel;

    [Header("Recipe Panel")]
    // things in the recipe panel

    [Header("Crafting Panel")]
    // items in the mix panel
    public Item item;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject inventoryPanel;
    public GameObject inventoryContent;

    public TextMeshProUGUI capricornValueText;
    public TextMeshProUGUI capricornRequiredText;
    public TextMeshProUGUI aquariusValueText;
    public TextMeshProUGUI aquariusRequiredText;
    public TextMeshProUGUI piscesValueText;
    public TextMeshProUGUI piscesRequiredText;
    public TextMeshProUGUI ariesValueText;
    public TextMeshProUGUI ariesRequiredText;
    public TextMeshProUGUI taurusValueText;
    public TextMeshProUGUI taurusRequiredText;
    public TextMeshProUGUI geminiValueText;
    public TextMeshProUGUI geminiRequiredText;
    public TextMeshProUGUI cancerValueText;
    public TextMeshProUGUI cancerRequiredText;
    public TextMeshProUGUI leoValueText;
    public TextMeshProUGUI leoRequiredText;
    public TextMeshProUGUI virgoValueText;
    public TextMeshProUGUI virgoRequiredText;
    public TextMeshProUGUI libraValueText;
    public TextMeshProUGUI libraRequiredText;
    public TextMeshProUGUI scorpioValueText;
    public TextMeshProUGUI scorpioRequiredText;
    public TextMeshProUGUI sagittariusValueText;
    public TextMeshProUGUI sagittariusRequiredText;

    [Header("Results Panel")]
    // items in the result panel


    Stack<Action> lastAction = new Stack<Action>();

    // zodiac bonus



    public enum ActionTaken
    {
        OPEN_RECIPE,
        ADDED_ITEM,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Item myItem1 = Instantiate(item);
        myItem1.title = "Rainbow Stardust";
        myItem1.aquarius = 9;
        myItem1.capricorn = 3;
        myItem1.gameObject.transform.parent = gameObject.transform;
        Item myItem2 = Instantiate(item);
        myItem2.title = "Red Gemstone";
        myItem2.scorpio = 2;
        myItem2.capricorn = 3;
        myItem2.gameObject.transform.parent = gameObject.transform;
        Item myItem3 = Instantiate(item);
        myItem3.title = "Red Gemstone";
        myItem3.scorpio = 3;
        myItem3.capricorn = 3;
        myItem3.gameObject.transform.parent = gameObject.transform;
        Item myItem4 = Instantiate(item);
        myItem4.title = "Red Gemstone";
        myItem4.scorpio = 2;
        myItem4.capricorn = 4;
        myItem4.gameObject.transform.parent = gameObject.transform;
        Item myItem5 = Instantiate(item);
        myItem5.title = "Rainbow Stardust";
        myItem5.aquarius = 8;
        myItem5.capricorn = 3;
        myItem5.aquarius = 1;
        myItem5.gameObject.transform.parent = gameObject.transform;
        Item myItem6 = Instantiate(item);
        myItem6.title = "Orange Brush";
        myItem6.aquarius = 8;
        myItem6.capricorn = 3;
        myItem6.aquarius = 1;
        myItem6.gameObject.transform.parent = gameObject.transform;
        Item myItem7 = Instantiate(item);
        myItem7.title = "Rainbow Stardust";
        myItem7.scorpio = 8;
        myItem7.capricorn = 3;
        myItem7.gameObject.transform.parent = gameObject.transform;
        Item myItem8 = Instantiate(item);
        myItem8.title = "Orange Brush";
        myItem8.aquarius = 8;
        myItem8.capricorn = 3;
        myItem8.aquarius = 1;
        myItem8.gameObject.transform.parent = gameObject.transform;
        Item myItem9 = Instantiate(item);
        myItem9.title = "Glorious Arrow";
        myItem9.aquarius = 8;
        myItem9.capricorn = 3;
        myItem9.aquarius = 1;
        myItem9.gameObject.transform.parent = gameObject.transform;

        for (int i=0; i < 150; i++)
        {
            Item myItem = Instantiate(item);
            myItem.title = "Glorious Arrow";
            myItem.aquarius = 8;
            myItem.capricorn = 3;
            myItem.aquarius = 1;
            myItem.gameObject.transform.parent = gameObject.transform;
        }
    }

    public void OpenRecipe(string recipe)
    {
        recipePanel.SetActive(false);
        craftPanel.SetActive(true);
    }

    public void Open(GameObject obj)
    {
        // open up inventory in recipe mixer
        inventoryPanel.gameObject.SetActive(true);
        // redraw the inventory panel
        foreach (Transform child in inventoryContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        // first, we need to know how big the panel needs to be, so I need to calculate the rowCount
        int rowCount = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Item myItem = gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            if (myItem.available && (obj.GetComponent<Item>().title == "Free" ||
                myItem.title == obj.GetComponent<Item>().title))
            {
                rowCount++;
            }
        }

        int row = 0;
        int column = 0;
        inventoryContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(340f, Mathf.Ceil(rowCount / 5) * 105 + 120f));
        for (int i=0; i < gameObject.transform.childCount; i++) {
            Item myItem = gameObject.transform.GetChild(i).gameObject.GetComponent<Item>();
            if (myItem.available && (obj.GetComponent<Item>().title == "Free" ||
                myItem.title == obj.GetComponent<Item>().title))
            {
                //list.Add(myItem.gameObject);
                Item newItem = Instantiate(myItem);
                //myItem.available = true;
                newItem.originalRef = myItem;
                newItem.GetComponent<Button>().onClick.AddListener(
                    delegate {
                        Select(newItem.gameObject, obj);
                        UpdateValues();
                    });
                newItem.gameObject.transform.parent = inventoryContent.transform;
                // now move where it displays
                column++;
                if (column % 5 == 1)
                {
                    row++;
                    column = 1;
                }
                newItem.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                newItem.gameObject.transform.localPosition = new Vector2((column*105f)-50f, -(row * 105f)+45f);
            }
        }
    }

    public void Select(GameObject item, GameObject itemSlot)
    {
        if (item.GetComponent<Item>().available)
        {
            // show the item in the slot if not already there...
            // show the count of the item in the slot
            //itemSlot.GetComponent<Item>().image = item.GetComponent<Item>().image;
            string countStr = itemSlot.GetComponent<Item>().count.text;
            // update the mixing values
            int.TryParse(countStr, out int count);
            count++;
            itemSlot.GetComponent<Item>().count.text = count.ToString();

            // mark inventory item unavailable
            //item.GetComponent<Item>().available = false;
            item.GetComponent<Item>().originalRef.available = false;
            // move item into slot
            item.transform.parent = itemSlot.transform;
            // make the item disapear from the render
            item.transform.localPosition = new Vector2(-500f, 0f);

            // set the last action in your stack
            Action action = new Action();
            action.item = item;
            action.itemSlot = itemSlot;

            lastAction.Push(action);

            // close the inventory panel
            inventoryPanel.gameObject.SetActive(false);
        }
    }   

    public void Craft(string recipe)
    {
        UpdateValues();

        // make new item and put into inventory
        Item i1 = item1.GetComponent<Item>();
        Item i2 = item2.GetComponent<Item>();
        Item i3 = item3.GetComponent<Item>();

        int capricornValue = getCapricorn(i1) + getCapricorn(i2) + getCapricorn(i3);
        int aquariusValue = getAquarius(i1) + getAquarius(i2) + getAquarius(i3);
        int piscesValue = getPisces(i1) + getPisces(i2) + getPisces(i3);
        int ariesValue = getAries(i1) + getAries(i2) + getAries(i3);
        int taurusValue = getTaurus(i1) + getTaurus(i2) + getTaurus(i3);
        int geminiValue = getGemini(i1) + getGemini(i2) + getGemini(i3);
        int cancerValue = getCancer(i1) + getCancer(i2) + getCancer(i3);
        int leoValue = getLeo(i1) + getLeo(i2) + getLeo(i3);
        int virgoValue = getVirgo(i1) + getVirgo(i2) + getVirgo(i3);
        int libraValue = getLibra(i1) + getLibra(i2) + getLibra(i3);
        int scorpioValue = getScorpio(i1) + getScorpio(i2) + getScorpio(i3);
        int sagittariusValue = getSagittarius(i1) + getSagittarius(i2) + getSagittarius(i3);

        if (recipes.CheckRecipe(recipe, capricornValue, aquariusValue, piscesValue, ariesValue, taurusValue, geminiValue, 
            cancerValue, leoValue, virgoValue, libraValue, scorpioValue, sagittariusValue))
        {
            Item myItem = Instantiate(item);

            var afterSpace = true;
            string formattedTitle = "";
            for (int i=0; i < recipe.Length; i++)
            {
                if (recipe[i].ToString().Equals("_"))
                {
                    formattedTitle += " ";
                    afterSpace = true;
                }
                else if (afterSpace)
                {
                    formattedTitle += recipe[i].ToString().ToUpper();
                    afterSpace = false;
                }
                else
                {
                    formattedTitle += recipe[i].ToString().ToLower();
                    afterSpace = false;
                }
            }
            Debug.Log(formattedTitle);

            myItem.title = formattedTitle;
            myItem.capricorn = capricornValue;
            myItem.aquarius = aquariusValue;
            myItem.pisces = piscesValue;
            myItem.aries = ariesValue;
            myItem.taurus = taurusValue;
            myItem.gemini = geminiValue;
            myItem.cancer = cancerValue;
            myItem.leo = leoValue;
            myItem.virgo = virgoValue;
            myItem.libra = libraValue;
            myItem.scorpio = scorpioValue;
            myItem.sagittarius = sagittariusValue;

            myItem.gameObject.transform.parent = gameObject.transform;

            // move new object into success panel view
            myItem.gameObject.transform.parent = gameObject.transform;
            myItem.gameObject.transform.localPosition = Vector2.zero;

            // show success panel
            resultPanel.SetActive(true);
            craftPanel.SetActive(false);

        }
        else
        {
            // show failure panel
        }

        // clear the last actions
        //lastAction = new Stack<Action>();
        lastAction.Clear();
    }

    public void BackToRecipes()
    {
        if (resultPanel.activeInHierarchy)
        {
            // if this came from mixing
            // destroy the original inventory objects that were mixed
            for (int i1 = 0; i1 < item1.transform.childCount; i1++)
            {
                Transform t1 = item1.transform.GetChild(i1);
                Item itemChild1 = t1.gameObject.GetComponent<Item>();
                if (itemChild1 != null)
                {
                    Destroy(itemChild1.originalRef.gameObject);
                    Destroy(itemChild1.gameObject);
                }
            }
            for (int i2 = 0; i2 < item2.transform.childCount; i2++)
            {
                Transform t2 = item2.transform.GetChild(i2);
                Item itemChild2 = t2.gameObject.GetComponent<Item>();
                if (itemChild2 != null)
                {
                    Destroy(itemChild2.originalRef.gameObject);
                    Destroy(itemChild2.gameObject);
                }
            }
            for (int i3 = 0; i3 < item3.transform.childCount; i3++)
            {
                Transform t3 = item3.transform.GetChild(i3);
                Item itemChild3 = t3.gameObject.GetComponent<Item>();
                if (itemChild3 != null)
                {
                    Destroy(itemChild3.originalRef.gameObject);
                    Destroy(itemChild3.gameObject);
                }
            }
            item1.GetComponent<Item>().count.text = "";
            item2.GetComponent<Item>().count.text = "";
            item3.GetComponent<Item>().count.text = "";

            resultPanel.SetActive(false);
        }
        if (craftPanel.activeInHierarchy)
        {
            // if this came from pressing back
            // reset any values in Item1/Item2/Item3
            for (int i1 = 0; i1 < item1.transform.childCount; i1++)
            {
                Transform t1 = item1.transform.GetChild(i1);
                Item itemChild1 = t1.gameObject.GetComponent<Item>();
                if (itemChild1 != null)
                {
                    itemChild1.originalRef.GetComponent<Item>().available = true;
                }
            }
            for (int i2 = 0; i2 < item2.transform.childCount; i2++)
            {
                Transform t2 = item2.transform.GetChild(i2);
                Item itemChild2 = t2.gameObject.GetComponent<Item>();
                if (itemChild2 != null)
                {
                    itemChild2.originalRef.GetComponent<Item>().available = true;
                }
            }
            for (int i3 = 0; i3 < item3.transform.childCount; i3++)
            {
                Transform t3 = item3.transform.GetChild(i3);
                Item itemChild3 = t3.gameObject.GetComponent<Item>();
                if (itemChild3 != null)
                {
                    itemChild3.originalRef.GetComponent<Item>().available = true;
                }
            }
            item1.GetComponent<Item>().count.text = "";
            item2.GetComponent<Item>().count.text = "";
            item3.GetComponent<Item>().count.text = "";

            craftPanel.SetActive(false);
        }
        recipePanel.SetActive(true);
    }

    void setValues(Item item)
    {
        int capricornValue = 0;
        int aquariusValue = 0;
        int piscesValue = 0;
        int ariesValue = 0;
        int taurusValue = 0;
        int geminiValue = 0;
        int cancerValue = 0;
        int leoValue = 0;
        int virgoValue = 0;
        int libraValue = 0;
        int scorpioValue = 0;
        int sagittariusValue = 0;

        for (int i = 0; i < item.transform.childCount; i++)
        {
            Transform t = item.transform.GetChild(i);
            Item itemChild = t.gameObject.GetComponent<Item>();
            if (itemChild != null)
            {
                // we found an item
                capricornValue += itemChild.capricorn;
                aquariusValue += itemChild.aquarius;
                piscesValue += itemChild.pisces;
                ariesValue += itemChild.aries;
                taurusValue += itemChild.taurus;
                geminiValue += itemChild.gemini;
                cancerValue += itemChild.cancer;
                leoValue += itemChild.leo;
                virgoValue += itemChild.virgo;
                libraValue += itemChild.libra;
                scorpioValue += itemChild.scorpio;
                sagittariusValue += itemChild.sagittarius;
            }
            item.capricorn = capricornValue;
            item.aquarius = aquariusValue;
            item.pisces = piscesValue;
            item.aries = ariesValue;
            item.taurus = taurusValue;
            item.gemini = geminiValue;
            item.cancer = cancerValue;
            item.leo = leoValue;
            item.virgo = virgoValue;
            item.libra = libraValue;
            item.scorpio = scorpioValue;
            item.sagittarius = sagittariusValue;
        }
    }

    void UpdateValues()
    {
        Item i1 = item1.GetComponent<Item>();
        Item i2 = item2.GetComponent<Item>();
        Item i3 = item3.GetComponent<Item>();
        // loop through item's child and tally values
        setValues(i1);
        setValues(i2);
        setValues(i3);

        int capricornValue = getCapricorn(i1) + getCapricorn(i2) + getCapricorn(i3);
        int aquariusValue = getAquarius(i1) + getAquarius(i2) + getAquarius(i3);
        int piscesValue = getPisces(i1) + getPisces(i2) + getPisces(i3);
        int ariesValue = getAries(i1) + getAries(i2) + getAries(i3);
        int taurusValue = getTaurus(i1) + getTaurus(i2) + getTaurus(i3);
        int geminiValue = getGemini(i1) + getGemini(i2) + getGemini(i3);
        int cancerValue = getCancer(i1) + getCancer(i2) + getCancer(i3);
        int leoValue = getLeo(i1) + getLeo(i2) + getLeo(i3);
        int virgoValue = getVirgo(i1) + getVirgo(i2) + getVirgo(i3);
        int libraValue = getLibra(i1) + getLibra(i2) + getLibra(i3);
        int scorpioValue = getScorpio(i1) + getScorpio(i2) + getScorpio(i3);
        int sagittariusValue = getSagittarius(i1) + getSagittarius(i2) + getSagittarius(i3);

        capricornValueText.text = capricornValue.ToString();
        aquariusValueText.text = aquariusValue.ToString();
        piscesValueText.text = piscesValue.ToString();
        ariesValueText.text = ariesValue.ToString();
        taurusValueText.text = taurusValue.ToString();
        cancerValueText.text = cancerValue.ToString();
        geminiValueText.text = geminiValue.ToString();
        leoValueText.text = leoValue.ToString();
        virgoValueText.text = virgoValue.ToString();
        libraValueText.text = libraValue.ToString();
        scorpioValueText.text = scorpioValue.ToString();
        sagittariusValueText.text = sagittariusValue.ToString();
    }

    int getCapricorn(Item item)
    {
        return item.capricorn;
    }
    int getAquarius(Item item)
    {
        return item.aquarius;
    }
    int getPisces(Item item)
    {
        return item.pisces;
    }
    int getAries(Item item)
    {
        return item.aries;
    }
    int getTaurus(Item item)
    {
        return item.taurus;
    }
    int getGemini(Item item)
    {
        return item.gemini;
    }
    int getCancer(Item item)
    {
        return item.cancer;
    }
    int getLeo(Item item)
    {
        return item.leo;
    }
    int getVirgo(Item item)
    {
        return item.virgo;
    }
    int getLibra(Item item)
    {
        return item.libra;
    }
    int getScorpio(Item item)
    {
        return item.scorpio;
    }
    int getSagittarius(Item item)
    {
        return item.sagittarius;
    }
    
    public void Undo()
    {
        if (lastAction.Count > 0)
        {
            Action thisAction = lastAction.Pop();
            if (thisAction.actionTaken.Equals(ActionTaken.OPEN_RECIPE))
            {
                
            }
            if (thisAction.actionTaken.Equals(ActionTaken.ADDED_ITEM))
            {
                //thisAction.item
                //thisAction.itemSlot
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
