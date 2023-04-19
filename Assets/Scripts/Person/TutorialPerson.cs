using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public string textBox_text;

        public string textBox_confirm;
        public string textBox_decline;
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
        if(curStep < steps.Length)
        {
            switch (steps[curStep].textBoxType)
            {
                case Type.QUESTION:
                    TextBoxManager.i.ShowQuestion(steps[curStep].textBox_text, steps[curStep].textBox_confirm, steps[curStep].textBox_decline, null, null);
                break;
                case Type.REPORT:
                    TextBoxManager.i.SetReportText(steps[curStep].textBox_text, steps[curStep].textBox_confirm);
                    TextBoxManager.i.ShowReport(null);
                break;
            }
        }
    }
}
