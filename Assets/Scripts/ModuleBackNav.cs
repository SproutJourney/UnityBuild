using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleBackButton : MonoBehaviour
{
    public Button backButton;
    public string targetSceneName;

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClick);
        }
        else
        {
            Debug.LogWarning("Back button is not assigned.");
        }
    }

    private void OnBackButtonClick()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
