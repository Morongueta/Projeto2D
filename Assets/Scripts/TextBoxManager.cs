using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TextBoxManager : MonoBehaviour
{
    [SerializeField]private GameObject textBoxHUD;
    [Space]
    [SerializeField]private GameObject interviewObj;
    [Space]
    [SerializeField]private GameObject questionObj;
    [SerializeField]private GameObject reportObj;
    [SerializeField]private TextMeshPro reportText;

    public static TextBoxManager i;

    private void Awake()
    {
        i = this;
    }

    public void ShowInterview(params Action[] buttonActions)
    {
        interviewObj.SetActive(true);
        textBoxHUD.SetActive(true);

        WorldButton[] worldButtons = interviewObj.GetComponentsInChildren<WorldButton>();
        if(worldButtons.Length == 0) return;

        for (int i = 0; i < worldButtons.Length; i++)
        {
            if(buttonActions.Length <= i) return;

            worldButtons[i].OnClickAction += buttonActions[i];
        }
    }

    public void SetReportText(string text)
    {
        reportText.text = text;
    }
    public void ShowQuestion()
    {
        questionObj.SetActive(true);
        textBoxHUD.SetActive(true);
    }
    public void ShowReport(params Action[] buttonActions)
    {
        reportObj.SetActive(true);
        textBoxHUD.SetActive(true);

        WorldButton[] worldButtons = reportObj.GetComponentsInChildren<WorldButton>();
        if(worldButtons.Length == 0) return;

        for (int i = 0; i < worldButtons.Length; i++)
        {
            if(buttonActions.Length <= i) return;

            worldButtons[i].OnClickAction += buttonActions[i];
        }
    }


    public void HideTextBox()
    {
        interviewObj.SetActive(false);
        questionObj.SetActive(false);
        reportObj.SetActive(false);
        textBoxHUD.SetActive(false);
    }
}
