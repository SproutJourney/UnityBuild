using TMPro;
using UnityEngine;

public class UserListDisplay : MonoBehaviour
{
    public TMP_Text userListText;

    private UserDatabase userDatabase = new UserDatabase();

    void Start()
    {
        LoadUserData();
        DisplayUserData();
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

    private void DisplayUserData()
    {
        string userDataText = "Registered Users:\n";
        foreach (UserData user in userDatabase.users)
        {
            userDataText += $"Username: {user.username}, Email: {user.email}, Name: {user.name}, Age: {user.age}\n";
        }

        Debug.Log(userDataText);

        if (userListText != null)
        {
            userListText.text = userDataText;
        }
    }
}
