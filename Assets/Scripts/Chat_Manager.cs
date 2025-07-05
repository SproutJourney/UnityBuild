using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chat_Manager : MonoBehaviour
{
    public GameObject userMessagePrefab;  // Prefab for user message
    public GameObject botMessagePrefab;   // Prefab for bot message
    public Transform contentTransform;    // Content area of the scroll view
    public ScrollRect scrollRect;         // ScrollRect to control scrolling
    public TMP_InputField inputField;     // Input field for user input

    private List<string> chatHistory = new List<string>();

    // Call this method to display the user's message
    public void DisplayUserMessage(string message)
    {
        GameObject newMessage = Instantiate(userMessagePrefab, contentTransform);

        // Ensure the prefab's scale is correct
        newMessage.transform.localScale = Vector3.one;

        // Get the TextMeshPro component and set the message text
        TextMeshProUGUI messageText = newMessage.GetComponentInChildren<TextMeshProUGUI>();
        messageText.text = message;

        // Scroll to the bottom after adding a new message
        StartCoroutine(ScrollToBottom());
    }

    // Call this method to display the bot's response
    public void DisplayBotMessage(string message)
    {
        GameObject newMessage = Instantiate(botMessagePrefab, contentTransform);

        // Ensure the prefab's scale is correct
        newMessage.transform.localScale = Vector3.one;

        // Get the TextMeshPro component and set the message text
        TextMeshProUGUI messageText = newMessage.GetComponentInChildren<TextMeshProUGUI>();
        messageText.text = message;

        // Scroll to the bottom after adding a new message
        StartCoroutine(ScrollToBottom());
    }

    // Coroutine to ensure scroll view updates to the latest message
    private IEnumerator ScrollToBottom()
    {
        yield return null;  // Wait for a frame to ensure new content is added
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // Method to handle sending a message (called when clicking Send button)
    public void OnSendMessage()
    {
        string userMessage = inputField.text;
        if (!string.IsNullOrEmpty(userMessage))
        {
            DisplayUserMessage(userMessage);
            inputField.text = "";  // Clear the input field

            // Simulate bot response (replace this with actual chatbot API call)
            StartCoroutine(SimulateBotResponse(userMessage));
        }
    }

    // Simulates a bot response after 1 second (for testing purposes)
    private IEnumerator SimulateBotResponse(string userMessage)
    {
        yield return new WaitForSeconds(1f);
        DisplayBotMessage("Bot: I received your message!");
    }
}
