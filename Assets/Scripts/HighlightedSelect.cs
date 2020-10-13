using TMPro;
using UnityEngine;

public class HighlightedSelect : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    private Color originalTextColor;
    private Color buttonHighlightColor = Color.white;

    void Start()
    {
        if (displayText == null)
        {
            displayText = transform.GetComponentInChildren<TextMeshProUGUI>();
        }
        if (displayText != null)
        {
            originalTextColor = displayText.color;
        }
    }

    public void OnSelect()
    {
        if (displayText != null)
        {
            displayText.color = buttonHighlightColor;
        }
    }

    public void OnDeselect()
    {
        if (displayText != null)
        {
            displayText.color = originalTextColor;
        }
    }
}
