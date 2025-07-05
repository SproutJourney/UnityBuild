using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

[System.Serializable]
public class UserData
{
    public string username;
    public string email;
    public string password;
    public string name;
    public int age;
}

[System.Serializable]
public class UserDatabase
{
    public List<UserData> users = new List<UserData>();
}

public class UserRegistration : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField nameInput;
    public TMP_InputField ageInput;
    public Button registerButton;
    public TMP_Text errorText;

    private UserDatabase userDatabase = new UserDatabase();
    private float errorDisplayTime = 3f;

    private Color successColor;
    private Color errorColor;

    void Start()
    {
        LoadUserData();
        registerButton.onClick.AddListener(OnRegisterButtonClicked);

        // Parse the hex colors
        ColorUtility.TryParseHtmlString("#32B840", out successColor); // Green color
        ColorUtility.TryParseHtmlString("#B83238", out errorColor);   // Red color
    }

    public void OnRegisterButtonClicked()
    {
        string username = usernameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        string name = nameInput.text;
        string ageString = ageInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ageString))
        {
            DisplayMessage("All fields must be filled!", false);
            return;
        }

        if (!int.TryParse(ageString, out int age) || age < 6 || age > 120)
        {
            DisplayMessage("Age must be a valid number between 6 and 120.", false);
            return;
        }

        if (password.Length < 6)
        {
            DisplayMessage("Password must be at least 6 characters long!", false);
            return;
        }

        if (!IsValidEmail(email))
        {
            DisplayMessage("Invalid email format!", false);
            return;
        }

        if (password != confirmPassword)
        {
            DisplayMessage("Passwords do not match!", false);
            return;
        }

        foreach (UserData user in userDatabase.users)
        {
            if (user.username == username)
            {
                DisplayMessage("Username already exists!", false);
                return;
            }

            if (user.email == email)
            {
                DisplayMessage("Email already registered!", false);
                return;
            }
        }

        UserData newUser = new UserData
        {
            username = username,
            email = email,
            password = password,
            name = name,
            age = age
        };

        userDatabase.users.Add(newUser);
        SaveUserData();
        SaveCurrentUser(username);
        ClearInputFields();

        DisplayMessage("Registration Successful!", true);

        Debug.Log("User Registered Successfully!");
    }

    private void ClearInputFields()
    {
        usernameInput.text = "";
        emailInput.text = "";
        passwordInput.text = "";
        confirmPasswordInput.text = "";
        nameInput.text = "";
        ageInput.text = "";
    }

    private bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    private void SaveUserData()
    {
        string json = JsonUtility.ToJson(userDatabase, true);
        PlayerPrefs.SetString("UserData", json);
        PlayerPrefs.Save();
        Debug.Log("User data saved: " + json);
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

    private void SaveCurrentUser(string username)
    {
        PlayerPrefs.SetString("CurrentUser", username);
        PlayerPrefs.SetInt("UserAge", GetUserAge(username));
        PlayerPrefs.Save();
    }

    private int GetUserAge(string username)
    {
        UserData user = userDatabase.users.Find(u => u.username == username);
        return user != null ? user.age : 6;
    }

    private void DisplayMessage(string message, bool isSuccess)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.color = isSuccess ? successColor : errorColor;
            Invoke("ClearErrorText", errorDisplayTime);
        }
        else
        {
            Debug.LogError(message);
        }
    }

    private void ClearErrorText()
    {
        errorText.text = "";
    }
}
