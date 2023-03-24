using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewPerson : Person
{
    public override void SetupEvent()
    {
        inFrontEvent += () => {TextBoxManager.i.ShowInterview();};
        goingAwayEvent += () => {TextBoxManager.i.HideTextBox();};

        base.SetupEvent();
    }

    
}
