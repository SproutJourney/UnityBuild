using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using TMPro;

public class ModNamesMan : MonoBehaviour
{
    public TMP_Text Mod_Display_Text;
    public TMP_Text Mode_Display_Text;
    void Start()
    {
        string selectedMode = PlayerPrefs.GetString("SelectedOption", "Not Available");
        string selectedModule = PlayerPrefs.GetString("SelectedModule", "Not Available");

        Mod_Display_Text.text = $"{selectedModule}";
        Mode_Display_Text.text = $"{selectedMode}";
    }

}
