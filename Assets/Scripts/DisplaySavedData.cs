using UnityEngine;
using TMPro;

public class DisplaySavedData : MonoBehaviour
{
    public TMP_Text savedDataText;

    void Start()
    {
        string selectedOption = PlayerPrefs.GetString("SelectedOption", "Not Available");
        string selectedModule = PlayerPrefs.GetString("SelectedModule", "Not Available");

        savedDataText.text = $"Selected Option: {selectedOption}\nSelected Module: {selectedModule}";
    }
}
