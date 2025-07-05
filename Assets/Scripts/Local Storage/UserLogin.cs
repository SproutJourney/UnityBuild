using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UserLogin : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TMP_Text errorText;

    private UserDatabase userDatabase = new UserDatabase();

    void Start()
    {
        LoadUserData();
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            DisplayError("Username and password cannot be empty.");
            return;
        }

        UserData loggedInUser = null;
        foreach (UserData user in userDatabase.users)
        {
            if (user.username == username && user.password == password)
            {
                loggedInUser = user;
                break;
            }
        }

        if (loggedInUser != null)
        {
            PlayerPrefs.SetString("LoggedInUser", username);
            PlayerPrefs.SetString("UserName", loggedInUser.name);
            PlayerPrefs.SetString("Age", loggedInUser.age.ToString());
            PlayerPrefs.Save();

            Debug.Log($"User logged in: {loggedInUser.name} Age: {loggedInUser.age}");

            SceneManager.LoadScene("Char Selection"); 
        }
        else
        {
            DisplayError("Invalid username or password.");
        }
    }

    private void DisplayError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
        }
        else
        {
            Debug.LogError(message);
        }
    }

    private void LoadUserData()
    {
        if (PlayerPrefs.HasKey("UserData"))
        {
            string json = PlayerPrefs.GetString("UserData");
            userDatabase = JsonUtility.FromJson<UserDatabase>(json);
            Debug.Log("Loaded user data: " + json);
        }
        else
        {
            Debug.Log("No existing user data found.");
        }
    }
}
