using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewPerson : Person
{
    public override void SetupEvent()
    {
        inFrontEvent += () => {SetInterviewBox();};
        goingAwayEvent += () => {TextBoxManager.i.HideTextBox();};

        base.SetupEvent();
    }

    public void SetInterviewBox()
    {
        TextBoxManager.i.ShowInterview(()=>ShowName(),null,()=>ShowFamilty(),null,() => ShowTraits());
    }

    public void ShowName()
    {
        TextBoxManager.i.HideTextBox();

        TextBoxManager.i.SetReportText("Meu nome é " + info.personName);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }

    public void ShowFamilty()
    {
        TextBoxManager.i.HideTextBox();

        TextBoxManager.i.SetReportText(info.hasFamily ? ("Sim, " + "Eu tenho " + info.sonsQtd.ToString() + (info.sonsQtd == 1 ? " Filho." : " Filhos.")) : "Não");
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }


    public void ShowTraits()
    {
        TextBoxManager.i.HideTextBox();

        string traitsText = "";
        traitsText += "<color=#FFFFFF>Positivos:\n<color=#00C119>";

        for (int i = 0; i < info.positiveTraits.Length; i++)
        {
            if(i == info.positiveTraits.Length - 1)traitsText += " e ";
            traitsText += info.positiveTraits[i].name;
            if(i < info.positiveTraits.Length - 2) traitsText += ", ";
        }

        traitsText += "\n";
        traitsText += "<color=#FFFFFF>Negativos:\n<color=#EA1A23>";

        for (int i = 0; i < info.negativeTraits.Length; i++)
        {
            if(i == info.negativeTraits.Length - 1)traitsText += " e ";
            traitsText += info.negativeTraits[i].name;
            if(i < info.negativeTraits.Length - 2) traitsText += ", ";
        }

        traitsText += "\n";
        traitsText += "\n";

        TextBoxManager.i.SetReportText(traitsText);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});
    }

    
}
