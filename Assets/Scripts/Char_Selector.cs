using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using TMPro;

public class Char_Selector : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> chrs = new List<Sprite>();
    public List<string> names = new List<string>();
    private int selectedChr = 0;
    public GameObject playerChar;
    public TMP_Text nameText;

    public float scaleSpeed = 2f;
    public float scaleAmount = 0.1f;

    private Vector3 originalScale;
    private bool scalingUp = true;

    private void Start()
    {
        originalScale = sr.transform.localScale;
        UpdateCharacter();
    }

    private void Update()
    {
        if (scalingUp)
        {
            sr.transform.localScale += Vector3.one * scaleAmount * Time.deltaTime * scaleSpeed;
            if (sr.transform.localScale.x >= originalScale.x + scaleAmount)
            {
                scalingUp = false;
            }
        }
        else
        {
            sr.transform.localScale -= Vector3.one * scaleAmount * Time.deltaTime * scaleSpeed;
            if (sr.transform.localScale.x <= originalScale.x)
            {
                scalingUp = true;
            }
        }
    }

    public void Next()
    {
        selectedChr = (selectedChr + 1) % chrs.Count;
        UpdateCharacter();
    }

    public void Back()
    {
        selectedChr = (selectedChr - 1 + chrs.Count) % chrs.Count;
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        sr.sprite = chrs[selectedChr];
        nameText.text = names[selectedChr];
    }

    public void Play()
    {
        PrefabUtility.SaveAsPrefabAsset(playerChar, "Assets/Prefabs/Selected_Char.prefab");
        SceneManager.LoadScene("Module Selection");
    }
}
