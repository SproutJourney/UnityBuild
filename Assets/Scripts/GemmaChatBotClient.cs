using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class GemmaChatbotClient : MonoBehaviour
{
    public string openRouterApiKey = "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"; 
    public string openRouterUrl = "https://openrouter.ai/api/v1/chat/completions";
    public string model = "openai/gpt-4-1106-preview"; // GPT-4.1 model

    public List<string> chatHistory = new List<string>();

    void Start()
    {
        Debug.Log("Starting chat with the chatbot...");
        SendMessageToAPI("Hello, chatbot!");
    }

    public void SendMessageToAPI(string message)
    {
        Debug.Log($"Sending message to OpenRouter: {message}");
        StartCoroutine(SendChatMessage(message));
    }

    private IEnumerator SendChatMessage(string message)
    {
        // Build chat messages for OpenRouter format
        List<Dictionary<string, string>> messages = new List<Dictionary<string, string>>();
        for (int i = 0; i < chatHistory.Count; i += 2)
        {
            messages.Add(new Dictionary<string, string> { { "role", "user" }, { "content", chatHistory[i].Replace("User: ", "") } });
            if (i + 1 < chatHistory.Count)
            {
                messages.Add(new Dictionary<string, string> { { "role", "assistant" }, { "content", chatHistory[i + 1].Replace("Bot: ", "") } });
            }
        }
        messages.Add(new Dictionary<string, string> { { "role", "user" }, { "content", message } });

        // Payload JSON
        var payload = new JSONObject();
        payload["model"] = model;
        var jsonMessages = new JSONArray();
        foreach (var msg in messages)
        {
            var obj = new JSONObject();
            obj["role"] = msg["role"];
            obj["content"] = msg["content"];
            jsonMessages.Add(obj);
        }
        payload["messages"] = jsonMessages;

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(payload.ToString());

        using (UnityWebRequest www = new UnityWebRequest(openRouterUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + openRouterApiKey);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"OpenRouter Error: {www.error}");
            }
            else
            {
                JSONNode response = JSON.Parse(www.downloadHandler.text);
                string botResponse = response["choices"][0]["message"]["content"];
                chatHistory.Add($"User: {message}");
                chatHistory.Add($"Bot: {botResponse}");
                Debug.Log($"Bot response: {botResponse}");
            }
        }
    }
}
