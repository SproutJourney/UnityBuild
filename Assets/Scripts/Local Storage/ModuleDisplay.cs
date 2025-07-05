using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class ModuleDisplay : MonoBehaviour
{
    public GameObject modulePrefab;
    public Transform content;
    public TMP_Text centralDisplayText;

    private Dictionary<string, List<ModuleChapter>> ageGroupData = new Dictionary<string, List<ModuleChapter>>()
    {
        {
            "6-7", new List<ModuleChapter>
            {
                new ModuleChapter("1", "Caring for Our Body"),
                new ModuleChapter("2", "Staying Clean"),
                new ModuleChapter("3", "The Power of Balanced Meals"),
                new ModuleChapter("4", "Helping at Home"),
                new ModuleChapter("5", "Being a Good Neighbour"),
                new ModuleChapter("6", "Caring for Animals"),
                new ModuleChapter("7", "Indoor and Outdoor Games"),
                new ModuleChapter("8", "Understanding Kindness"),
                new ModuleChapter("9", "Knowing Right from Wrong")
            }
        },
        {
            "8-9", new List<ModuleChapter>
            {
                new ModuleChapter("1", "Keeping a Healthy Mind"),
                new ModuleChapter("2", "Caring for Our Body"),
                new ModuleChapter("3", "Drinking Water and Limiting Junk Food"),
                new ModuleChapter("4", "Why We Need Healthy Food"),
                new ModuleChapter("5", "What is Climate?"),
                new ModuleChapter("6", "How Weather Affects Us"),
                new ModuleChapter("7", "Using Technology Safely"),
                new ModuleChapter("8", "Friendship and Trust"),
                new ModuleChapter("9", "Courage and Decision-Making"),
                new ModuleChapter("10", "Understanding Responsibility")
            }
        },
        {
            "10-12", new List<ModuleChapter>
            {
                new ModuleChapter("1", "How Occupations Support Livelihoods"),
                new ModuleChapter("2", "The Role of Trade in the Community"),
                new ModuleChapter("3", "What is Transport?"),
                new ModuleChapter("4", "What is Communication?"),
                new ModuleChapter("5", "Reading Maps"),
                new ModuleChapter("6", "Understanding Different Terrains"),
                new ModuleChapter("7", "How Things Move"),
                new ModuleChapter("8", "Learning About Light and Shadows"),
                new ModuleChapter("9", "What Are Simple Machines?"),
                new ModuleChapter("10", "How Simple Machines Help Us"),
                new ModuleChapter("11", "Managing Ethical Dilemmas"),
                new ModuleChapter("12", "Understanding Duty and Dharma")
            }
        }
    };

    void Start()
    {
        DisplayModules();
    }

    private void DisplayModules()
    {
        if (PlayerPrefs.HasKey("UserData"))
        {
            string json = PlayerPrefs.GetString("UserData");
            UserDatabase userDatabase = JsonUtility.FromJson<UserDatabase>(json);

            string currentUsername = PlayerPrefs.GetString("LoggedInUser", string.Empty);

            UserData currentUser = userDatabase.users.FirstOrDefault(user => user.username == currentUsername);

            if (currentUser != null)
            {
                int age;
                if (int.TryParse(PlayerPrefs.GetString("Age", "0"), out age))
                {
                    string ageGroup = GetAgeGroup(age);

                    if (ageGroupData.ContainsKey(ageGroup))
                    {
                        ClearContent();
                        List<ModuleChapter> modules = ageGroupData[ageGroup];
                        foreach (ModuleChapter module in modules)
                        {
                            GameObject moduleObject = Instantiate(modulePrefab, content);
                            TMP_Text[] texts = moduleObject.GetComponentsInChildren<TMP_Text>();
                            TMP_Dropdown dropdown = moduleObject.GetComponentInChildren<TMP_Dropdown>();

                            if (texts.Length >= 2)
                            {
                                texts[0].text = $"Chapter {module.ModuleNumber}";
                                texts[1].text = module.ModuleName;

                                if (dropdown != null)
                                {
                                    dropdown.onValueChanged.AddListener(delegate {
                                        UpdateCentralDisplayText(dropdown, module);
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Age group not found in data.");
                    }
                }
                else
                {
                    Debug.LogWarning("Age is not a valid integer or missing.");
                }
            }
            else
            {
                Debug.LogWarning("Current user not found in UserDatabase.");
            }
        }
        else
        {
            Debug.LogWarning("No user data found in PlayerPrefs.");
        }
    }

    private void UpdateCentralDisplayText(TMP_Dropdown dropdown, ModuleChapter module)
    {
        string selectedOption = dropdown.options[dropdown.value].text;
        centralDisplayText.text = $"Selected Option: {selectedOption}\nModule: Chapter {module.ModuleNumber}, {module.ModuleName}";

        PlayerPrefs.SetString("SelectedOption", selectedOption);
        PlayerPrefs.SetString("SelectedModule", $"{module.ModuleNumber}: {module.ModuleName}");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Chat Screen"); 
    }

    private string GetAgeGroup(int age)
    {
        if (age == 6 || age == 7) return "6-7";
        if (age == 8 || age == 9) return "8-9";
        if (age >= 10 && age <= 12) return "10-12";
        return "6-7";
    }

    private void ClearContent()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}

[System.Serializable]
public class ModuleChapter
{
    public string ModuleNumber;
    public string ModuleName;

    public ModuleChapter(string moduleNumber, string moduleName)
    {
        ModuleNumber = moduleNumber;
        ModuleName = moduleName;
    }
}
