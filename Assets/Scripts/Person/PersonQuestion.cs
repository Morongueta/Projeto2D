using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersonQuestion : Person
{
    public string question;
    public string confirm;
    public string decline;


    public Action confirmAction;
    public Action declineAction;


    public override void SetupEvent()
    {
        inFrontEvent += () => {ShowQuestion();};
        goingAwayEvent += () => {TextBoxManager.i.HideTextBox();};

        base.SetupEvent();
    }

    public void ShowQuestion()
    {
        TextBoxManager.i.ShowQuestion(question,confirm,decline,()=>
        {
            //Confirm Action
            confirmAction?.Invoke();
            QueueManager.i.RemoveFromQueue(0);
        },
        ()=>
        {
            //DeclineAction
            declineAction?.Invoke();
            QueueManager.i.RemoveFromQueue(0);
        }
        );
    }
}
