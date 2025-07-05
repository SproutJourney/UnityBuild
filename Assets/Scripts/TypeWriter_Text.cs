using System.Collections;
using UnityEngine;
using TMPro;

public class TypeWriter_Text : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpProText;
    private string writer;
    [SerializeField] private Coroutine coroutine;

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;

    [Space(10)][SerializeField] private bool startOnEnable = true;
    [SerializeField] private float underscoreFlashTime = 0.5f;

    void Awake()
    {
        if (tmpProText == null)
        {
            tmpProText = GetComponent<TMP_Text>();
        }
    }

    void Start()
    {
        if (tmpProText != null)
        {
            tmpProText.text = "";
        }
    }

    private void OnEnable()
    {
        if (startOnEnable) StartTypewriter();
    }

    public void StartTypewriterEffect(string text)
    {
        writer = text;
        StartTypewriter();
    }

    private void StartTypewriter()
    {
        StopAllCoroutines();
        if (tmpProText != null)
        {
            tmpProText.text = "";
            StartCoroutine(TypeWriterTMP());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TypeWriterTMP()
    {
        tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (tmpProText.text.Length > 0)
            {
                tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
            }
            tmpProText.text += c;
            tmpProText.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
        }

        while (true)
        {
            tmpProText.text = writer + " _";
            yield return new WaitForSeconds(underscoreFlashTime);
            tmpProText.text = writer;
            yield return new WaitForSeconds(underscoreFlashTime);
        }
    }
}
