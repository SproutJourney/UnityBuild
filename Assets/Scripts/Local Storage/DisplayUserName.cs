using UnityEngine;
using TMPro;

public class DisplayUserName : MonoBehaviour
{
    public TMP_Text userNameText;
    public TypeWriter_Text typeWriter;

    void Start()
    {
        DisplayUserNameInUI();
    }

    private void DisplayUserNameInUI()
    {
        string userName = PlayerPrefs.GetString("UserName", "Guest");
        string welcomeText = $"Welcome  {userName}";

        if (typeWriter != null && userNameText != null)
        {
            typeWriter.StartTypewriterEffect(welcomeText);
        }
        else
        {
            Debug.LogWarning("TypeWriter_Text or UserNameText is not assigned.");
        }
    }
}
