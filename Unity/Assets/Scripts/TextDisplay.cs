using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextDisplay : MonoBehaviour
{
    public TMP_Text textMeshPro; // Reference to a TextMeshPro Text element in the scene
    private string message = ""; // The message to display
    private float typingSpeed = 40f; // Characters per second typing speed
    private float delay;
    private Coroutine typingCoroutine;
    private Queue<string> messageQueue = new Queue<string>(); // Queue to hold messages
    private bool isTyping = false; // Flag to track if a message is currently being typed

    private void Start()
    {
        // Check if the TextMeshPro Text element is assigned
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro Text element is not assigned. Please assign it in the Inspector.");
        }
    }

    // Public method to update the message
    public void UpdateMessage(string newMessage, bool wait,float delayBeforeNextMessage)
    {
        if (isTyping || typingCoroutine != null)
        {
            if (!wait)
            {
                StopCoroutine(typingCoroutine);
                isTyping = false;
            }
            else
            {
                // If waiting is requested, add the message to the queue
                messageQueue.Enqueue(newMessage);
                return;
            }
        }

        delay = delayBeforeNextMessage;
        // Start a new typing animation coroutine with the updated message
        typingCoroutine = StartCoroutine(TypeMessage(newMessage));
    }

    IEnumerator TypeMessage(string newMessage)
    {
        isTyping = true; // Set the flag to indicate that a message is being typed
        textMeshPro.text = ""; // Clear the text initially

        foreach (char letter in newMessage)
        {
            textMeshPro.text += letter; // Add one character at a time
            yield return new WaitForSeconds(1 / typingSpeed);
        }

        isTyping = false; // Reset the flag when typing is done

        // Check if there are more messages in the queue and process them after a delay
        if (messageQueue.Count > 0)
        {
            yield return new WaitForSeconds(delay);
            string nextMessage = messageQueue.Dequeue();
            // Start typing the next message
            typingCoroutine = StartCoroutine(TypeMessage(nextMessage));
        }
    }
}
