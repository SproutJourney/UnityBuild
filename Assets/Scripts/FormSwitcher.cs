using UnityEngine;

public class FormSwitcher : MonoBehaviour
{
    public GameObject loginForm;
    public GameObject parentRegForm;
    public GameObject schoolRegForm;

    void Start()
    {
        ShowLoginForm();
    }

    public void ShowLoginForm()
    {
        loginForm.SetActive(true);
        parentRegForm.SetActive(false);
        schoolRegForm.SetActive(false);
    }

    public void ShowParentRegForm()
    {
        loginForm.SetActive(false);
        parentRegForm.SetActive(true);
        schoolRegForm.SetActive(false);
    }

    public void ShowSchoolRegForm()
    {
        loginForm.SetActive(false);
        parentRegForm.SetActive(false);
        schoolRegForm.SetActive(true);
    }
}
