using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    
    [SerializeField] private Image[] captionImage;         // CaptionImage that holds the text child
    [SerializeField] private float typingSpeed;            // The speed of typing in seconds
    [SerializeField] private bool keepPastCaptions;        // Keep all captions on screen 
    private TextMeshProUGUI textComponent;                 // Text child of the Caption Image
    private Coroutine typingCoroutine;                     // Reference to the typing coroutine
    private string textToType;                             // The text to be typed
    private int captionIndex;
    public static bool textTrigger;
                                                                // Current caption being typed starting at zero

    private void Update()
    {
        UserInput();
    }

    private void UserInput()
    {
        // Start typing by pressing any key.
        if (textTrigger && typingCoroutine == null)
        {
            if (captionIndex < captionImage.Length)
            {
                typingCoroutine = StartCoroutine(TypeText());
            }
            else
            {
                SetCaptionInactive();
            }
        }
    }

    private void SetCaptionInactive()
    {
        if (keepPastCaptions && captionIndex != 0)
        {
            captionImage[captionIndex -1].gameObject.SetActive(false);
        }
    }

    // Types caption into the image canvas in the scene
    IEnumerator TypeText()
    {
        SetCaptionInactive();
        captionImage[captionIndex].gameObject.SetActive(true);
        textComponent = captionImage[captionIndex].GetComponentInChildren<TextMeshProUGUI>();
        textToType = textComponent.text;
        textComponent.text = "";               // Clear existing text
        float typingSpeedThisCharacter = typingSpeed;
        // Loop through each character in the textToType

        for (int i = 0; i < textToType.Length; i++)
        {
            textComponent.text += textToType[i];  // Add the next character to the textComponent
            if (textToType[i].Equals(",")) typingSpeedThisCharacter *= 1.5f;
            if (textToType[i].Equals(".")) typingSpeedThisCharacter *= 2.0f;
            yield return new WaitForSeconds(typingSpeedThisCharacter);  // Wait for typingSpeed seconds before typing the next character
        }

        // Move to next caption
        captionIndex++;
        typingCoroutine = null;

    }
}
