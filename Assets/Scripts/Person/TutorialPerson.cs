using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Type
{
    QUESTION,
    REPORT
}
public class TutorialPerson : Person
{
    [System.Serializable]
    public struct TutorialStep
    {
        public Type textBoxType;

        [TextArea(3,6)]
        public string textBox_text;

        public string textBox_confirm;
        public UnityEvent confirmAction;
        public string textBox_decline;
        public UnityEvent declineAction;
    }

    public TutorialStep[] steps;
    private int curStep;

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
                ShowTutorial();
                chatButton.SetSprite(closeSprite);
            }else{
                TextBoxManager.i.HideTextBox();
                chatButton.SetSprite(defaultSprite);
            } 
        };
    }

    public void ShowTutorial()
    {
        TextBoxManager.i.HideTextBox();

        if(curStep < steps.Length)
        {
            switch (steps[curStep].textBoxType)
            {
                case Type.QUESTION:
                    TextBoxManager.i.ShowQuestion(steps[curStep].textBox_text, steps[curStep].textBox_confirm, steps[curStep].textBox_decline, 
                    ()=>{ //Confirm
                        steps[curStep].confirmAction?.Invoke();
                        curStep++;
                        ShowTutorial();
                    }, ()=>{  //Decline
                        steps[curStep].declineAction?.Invoke();
                        curStep--;
                        ShowTutorial();
                    });
                break;
                case Type.REPORT:
                    TextBoxManager.i.SetReportText(steps[curStep].textBox_text, steps[curStep].textBox_confirm);
                    TextBoxManager.i.ShowReport(()=>{
                        steps[curStep].confirmAction?.Invoke();
                        curStep++;
                        ShowTutorial();
                    });
                break;
            }
        }else{
            QueueManager.i.RemoveFromQueue(0);
        }
    }



    public void GiveEmptyPaper()
    {
        PaperManager.i.AddPaper();
    }

    public void StartHiringEvent()
    {
        HiringManager.i.StartHiring();
    }

    public void PrepareTutorialTwo()
    {
        TutorialManager.i.SpawnTwo();
    }

    public void EndTutorial()
    {
        QueueManager.i.RemoveFromQueue(0);
        TutorialManager.i.EndTutorial();
    }


    public void GiveContractPaper()
    {
        PaperManager.i.AddContractPaper("Você entendeu todos os procedimentos do seu cargo? (Refazer o tutorial?) \nMarque com caneta por favor", 
        ()=>{
            TutorialManager.i.EndTutorial();
        }, ()=>{
            TutorialManager.i.ResetTutorial();
        });
    }
}
