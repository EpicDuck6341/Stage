using UnityEngine;
using TMPro;
using System.Collections;

public class TextDisplay : MonoBehaviour
{
    public TMP_Text textMeshPro; // Reference to a TextMeshPro Text element in the scene
    private string message = "Hello, TextMeshPro. This is a test to make a long message where i type all kinds of random stuff to see how long this thing can go on for without being unreadable xdd!"; // The message to display
    public float typingSpeed = 10.0f; // Characters per second typing speed

    private void Start()
    {
        // Check if the TextMeshPro Text element is assigned
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro Text element is not assigned. Please assign it in the Inspector.");
        }
        else
        {
            // Start the typing animation coroutine
            StartCoroutine(TypeMessage());
        }
    }

    IEnumerator TypeMessage()
    {
        textMeshPro.text = ""; // Clear the text initially

        foreach (char letter in message)
        {
            textMeshPro.text += letter; // Add one character at a time
            yield return new WaitForSeconds(1 / typingSpeed);
        }
    }
}