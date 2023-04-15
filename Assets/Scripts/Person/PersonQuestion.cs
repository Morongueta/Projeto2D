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
                Debug.Log("Ativado");
                ShowQuestion();
                chatButton.SetSprite(closeSprite);
            }else{
                Debug.Log("Desativado");
                TextBoxManager.i.HideTextBox();
                chatButton.SetSprite(defaultSprite);
            } 
        };
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
