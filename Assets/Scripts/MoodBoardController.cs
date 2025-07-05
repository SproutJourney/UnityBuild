using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoodboardController : MonoBehaviour
{
    public GameObject moodboard; // Assign the moodboard GameObject in inspector
    private bool isVisible = false;

    void Start()
    {
        moodboard.SetActive(false);
    }

    public void ToggleMoodboard()
    {
        isVisible = !isVisible;
        moodboard.SetActive(isVisible);
    }

    // Call this from a Button component inside the moodboard
    public void CloseMoodboard()
    {
        moodboard.SetActive(false);
        isVisible = false;
    }
}
