using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersonReport : Person
{
    public string reportText;
    public string reportConfirm;

    public Action confirmAction;


    public override void SetupEvent()
    {
        SetupChatButton();
        inFrontEvent += () => {chatButton.gameObject.SetActive(true);};
        goingAwayEvent += () => {TextBoxManager.i.HideTextBox();};

        base.SetupEvent();
    }

    public override void SetupChatButton()
    {
        chatButton.OnClickAction += () => {
            
            if(!TextBoxManager.i.showingTextBox)
            {
                ShowQuestion();
                chatButton.SetSprite(closeSprite);
            }else{
                TextBoxManager.i.HideTextBox();
                chatButton.SetSprite(defaultSprite);
            } 
        };
    }

    public void ShowQuestion()
    {
        TextBoxManager.i.SetReportText(reportText, reportConfirm);
        TextBoxManager.i.ShowReport(()=>
        {
            //Confirm Action
            confirmAction?.Invoke();
            QueueManager.i.RemoveFromQueue(0);
        });
    }
}
