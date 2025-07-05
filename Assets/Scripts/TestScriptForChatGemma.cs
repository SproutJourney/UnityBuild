using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions; // <-- Needed for tag cleaning

public class TestScriptForChatGemma : MonoBehaviour
{
    private const string API_URL = "http://localhost:5000/chat"; // Flask API URL

    public TMP_InputField userInputField;
    public Button sendButton;
    public GameObject botMessagePrefab;
    public GameObject userMessagePrefab;
    public Transform content;
    public ScrollRect scrollRect;
    public AudioSource audioSource; // Drag your AudioSource here in Inspector

    public RawImage aiImageComponent; // Drag your RawImage here in Inspector (580x300 or 29:15)
    private bool aiImageShown = false; // Only show image after first image loaded

    private string selectedOption;
    private string selectedModule;
    private string userName;

    private void Start()
    {
        selectedOption = PlayerPrefs.GetString("SelectedOption", "Not Available");
        selectedModule = PlayerPrefs.GetString("SelectedModule", "Not Available");
        userName = PlayerPrefs.GetString("UserName", "Guest");

        if (aiImageComponent != null)
            aiImageComponent.gameObject.SetActive(false);

        string initialPrompt = GetInitialPrompt();
        SendChatMessage(initialPrompt, true);
        sendButton.onClick.AddListener(SendMessageFromInputField);
        userInputField.onValueChanged.AddListener(delegate { audioSource.Stop(); });
    }

    private string GetInitialPrompt()
    {
        switch (selectedOption.ToLower())
        {
            case "story":
                return $"Hi, I am {userName}. I want to learn about {selectedModule}. Tell me a story to help me learn about it from mainchaptercontents.";
            case "learning":
                return $"Hi, I am {userName}. I want to learn about {selectedModule}. Tell me the mainchaptercontents.";
            case "thinking":
                return $"Hi, I am {userName}. I want you to test me about {selectedModule}. Ask me 3 multiple choice questions from context.";
            case "fun":
                return $"Hi, I am {userName}. I want to have fun while learning about {selectedModule}.";
            default:
                return $"Hi, I am {userName}. I want to learn about {selectedModule}. Tell me a story.";
        }
    }

    private void SendMessageFromInputField()
    {
        string userMessage = userInputField.text;
        if (!string.IsNullOrEmpty(userMessage))
        {
            InstantiateMessage(userMessagePrefab, userMessage);
            AutoScroll();
            sendButton.interactable = false;
            SendChatMessage(userMessage, false);
            userInputField.text = "";
        }
    }

    public void SendChatMessage(string message, bool isInitialPrompt)
    {
        StartCoroutine(PostChatMessage(message));
    }

    private IEnumerator PostChatMessage(string message)
    {
        sendButton.interactable = false;

        var payload = new Dictionary<string, object>
        {
            { "question", message },
            { "user_name", userName },
            { "selected_module", selectedModule }
        };
        string jsonData = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Request failed: " + request.error);
                sendButton.interactable = true;
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("Flask API Response: " + responseText);

                string botMessage = "No response.";
                string audioUrl = null;
                string imageUrl = null;

                try
                {
                    var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);
                    if (responseDict.ContainsKey("answer"))
                        botMessage = responseDict["answer"].ToString();
                    if (responseDict.ContainsKey("audio_url"))
                        audioUrl = responseDict["audio_url"]?.ToString();
                    if (responseDict.ContainsKey("image_url"))
                        imageUrl = responseDict["image_url"]?.ToString();
                }
                catch
                {
                    Debug.LogError("Could not parse response JSON");
                }

                // Start TTS audio playback immediately
                if (!string.IsNullOrEmpty(audioUrl))
                {
                    StartCoroutine(PlayTTS(audioUrl));
                }

                // Show image if received
                if (!string.IsNullOrEmpty(imageUrl) && aiImageComponent != null)
                {
                    StartCoroutine(LoadAndShowImage(imageUrl));
                }

                // Show bot message with typewriter effect (cleaning SSML tags for display)
                GameObject botMsgObj = Instantiate(botMessagePrefab, content);
                TextMeshProUGUI botText = botMsgObj.GetComponentInChildren<TextMeshProUGUI>();
                if (botText != null)
                {
                    string displayMessage = StripSSMLTags(botMessage); // <--- CLEANING HERE!
                    yield return StartCoroutine(TypewriterEffect(botText, displayMessage, 0.025f));
                }

                sendButton.interactable = true;
            }
        }
    }

    private IEnumerator PlayTTS(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Audio download error: " + uwr.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                if (clip != null)
                {
                    audioSource.Stop();
                    audioSource.clip = clip;
                    audioSource.Play();
                }
            }
        }
    }

    private IEnumerator LoadAndShowImage(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Image download error: " + uwr.error);
            }
            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                if (tex != null)
                {
                    aiImageComponent.texture = tex;
                    if (!aiImageShown)
                    {
                        aiImageComponent.gameObject.SetActive(true);
                        aiImageShown = true;
                    }
                }
            }
        }
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent, string fullText, float delay = 0.03f)
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            AutoScroll();
            yield return new WaitForSeconds(delay);
        }
    }

    private void AutoScroll()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
    }

    private void InstantiateMessage(GameObject prefab, string message)
    {
        GameObject messageObject = Instantiate(prefab, content);
        TextMeshProUGUI textComponent = messageObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = message;
        }
    }

    // SSML/HTML/XML tag stripper for clean UI display
    private string StripSSMLTags(string input)
    {
        // Remove <emphasis>...</emphasis> but keep the text inside
        string noEmphasis = Regex.Replace(input, "<emphasis.*?>(.*?)</emphasis>", "$1", RegexOptions.IgnoreCase);
        // Remove <break .../>
        string clean = Regex.Replace(noEmphasis, "<break.*?/>", "", RegexOptions.IgnoreCase);
        // Remove any other XML-like tags just in case
        clean = Regex.Replace(clean, "<.*?>", "");
        return clean;
    }
}
