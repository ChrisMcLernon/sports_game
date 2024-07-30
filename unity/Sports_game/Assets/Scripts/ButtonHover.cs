using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text buttonText;
    public Color hoverColor = Color.white;
    private Color originalColor;

    void Start()
    {
        // Get the TMP_Text component in the Button's child hierarchy
        buttonText = GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            // Store the original color of the button text
            originalColor = buttonText.color;
        }
    }

    // This method is called when the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            // Change the button text color to the hover color
            buttonText.color = hoverColor;
        }
    }

    // This method is called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            // Revert the button text color back to the original color
            buttonText.color = originalColor;
        }
    }

    void OnDisable()
    {
        if (buttonText != null)
        {
            // Revert the button text color back to the original color
            buttonText.color = originalColor;
        }
    }
}