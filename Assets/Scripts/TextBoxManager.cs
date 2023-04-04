using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TextBoxManager : MonoBehaviour
{
    [SerializeField]private GameObject textBoxHUD;
    [Space]
    [SerializeField]private TextMeshPro interviewMoneyText;
    [SerializeField]private GameObject interviewObj;
    [SerializeField]private GameObject interviewQuestionObj;
    [Space]
    [SerializeField]private GameObject questionObj;
    [SerializeField]private GameObject reportObj;
    [SerializeField]private TextMeshPro reportText;

    [SerializeField]private WorldButton goAwayButton;

    public static TextBoxManager i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        goAwayButton.OnClickAction += () => {
            QueueManager.i.RemoveFromQueue(0);
        };
    }

    public void ShowInterview(params Action[] buttonActions)
    {
        interviewObj.SetActive(true);
        textBoxHUD.SetActive(true);

        WorldButton[] worldButtons = interviewQuestionObj.GetComponentsInChildren<WorldButton>();
        if(worldButtons.Length == 0) return;

        for (int i = 0; i < worldButtons.Length; i++)
        {
            if(buttonActions.Length <= i) return;
            worldButtons[i].OnClickAction -= worldButtons[i].OnClickAction;
            worldButtons[i].OnClickAction += buttonActions[i];
        }
    }

    public void ShowInterview(params bool[] activity)
    {
        interviewObj.SetActive(true);
        textBoxHUD.SetActive(true);

        WorldButton[] worldButtons = interviewQuestionObj.GetComponentsInChildren<WorldButton>();
        if(worldButtons.Length == 0) return;

        for (int i = 0; i < worldButtons.Length; i++)
        {
            if(activity.Length <= i) return;
            worldButtons[i].interactable = activity[i];
        }
    }

    public void ShowInterviewMoney(string text)
    {
        interviewMoneyText.text = text;
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

            worldButtons[i].OnClickAction -= worldButtons[i].OnClickAction;
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
