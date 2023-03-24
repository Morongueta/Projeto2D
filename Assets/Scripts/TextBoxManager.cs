using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxManager : MonoBehaviour
{
    [SerializeField]private GameObject textBoxHUD;
    [Space]
    [SerializeField]private GameObject interviewObj;
    [SerializeField]private GameObject questionObj;
    [SerializeField]private GameObject reportObj;

    public static TextBoxManager i;

    private void Awake()
    {
        i = this;
    }

    public void ShowInterview()
    {
        interviewObj.SetActive(true);
        textBoxHUD.SetActive(true);
    }
    public void ShowQuestion()
    {
        questionObj.SetActive(true);
        textBoxHUD.SetActive(true);
    }
    public void ShowReport()
    {
        reportObj.SetActive(true);
        textBoxHUD.SetActive(true);
    }


    public void HideTextBox()
    {
        interviewObj.SetActive(false);
        questionObj.SetActive(false);
        reportObj.SetActive(false);
        textBoxHUD.SetActive(false);
    }
}
