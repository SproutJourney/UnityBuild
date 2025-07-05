using UnityEngine;
using TMPro;

public class ModuleItem : MonoBehaviour
{
    public TMP_Text chapterNumberText;
    public TMP_Text chapterNameText;

    private ModuleChapter moduleData;

    public void Initialize(ModuleChapter chapter, System.Action<ModuleChapter> onSelected)
    {
        moduleData = chapter;
        chapterNumberText.text = $"Chapter {moduleData.ModuleNumber}";
        chapterNameText.text = moduleData.ModuleName;
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => onSelected(moduleData));
    }

    private void Start()
    {
        if (chapterNumberText == null || chapterNameText == null)
        {
            Debug.LogError("Text references are not assigned in the ModuleItem prefab.");
        }
    }
}
