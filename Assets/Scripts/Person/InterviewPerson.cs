using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewPerson : Person
{
    private string[] startContribution = new string[] {"Na minha ultima empresa ", "No meu ultimo emprego ", "Bom, na empresa que eu trabalhei "};
    public override void SetupEvent()
    {
        inFrontEvent += () => {HiringManager.i.ResetInterviewMoney();};
        inFrontEvent += () => {SetInterviewBox();};
        goingAwayEvent += () => {TextBoxManager.i.HideTextBox();};

        base.SetupEvent();
    }

    public void SetInterviewBox()
    {
        int money = HiringManager.i.GetInverviewMoney();

        TextBoxManager.i.ShowInterviewMoney(money.ToString());
        TextBoxManager.i.ShowInterview((money >= 1),(money >= 2),(money >= 1),(money >= 3),(money >= 5));
        TextBoxManager.i.ShowInterview(()=>ShowName(),()=>ShowAboutYou(),()=>ShowFamily(),()=>ShowContribution(),() => ShowTraits());
    }

    public void ShowName()
    {
        HiringManager.i.SpendInterviewMoney(1);
        TextBoxManager.i.HideTextBox();

        TextBoxManager.i.SetReportText("Meu nome é " + info.personName);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }

    public void ShowAboutYou()
    {
        HiringManager.i.SpendInterviewMoney(3);
        TextBoxManager.i.HideTextBox();

        string aboutYouText = "";
        
        for (int i = 0; i < info.GetAllTraits().Length; i++)
        {
            aboutYouText += "\n-";
            if((Random.value * 100f) <= 25f) //lie
            {
                Trait falseTrait = InformationDatabase.i.GetRandomTraits(1,(Random.value * 100f < 30f) ? TraitType.NEGATIVE : TraitType.POSITIVE, info.GetAllTraits())[0];
                aboutYouText += falseTrait.traitDetail[Random.Range(0,falseTrait.traitDetail.Length)];
            }else
            {
                aboutYouText += info.GetAllTraits()[i].traitDetail[Random.Range(0,info.GetAllTraits()[i].traitDetail.Length)];
            }
            aboutYouText += ".";
        }

        TextBoxManager.i.SetReportText(aboutYouText);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }

    public void ShowContribution()
    {
        HiringManager.i.SpendInterviewMoney(2);
        TextBoxManager.i.HideTextBox();

        string contributionText = startContribution[Random.Range(0,startContribution.Length)] + "eu fiquei por " + info.contributionTime + " meses.\n";
        
        int highest = -100;
        int index = 0;
        for (int i = 0; i < info.GetAllTraits().Length; i++)
        {
            if(Mathf.Abs(info.GetAllTraits()[i].contributionTime) > highest)
            {
                highest = Mathf.Abs(info.GetAllTraits()[i].contributionTime);
                index = i;
            } 
        }
        Trait selected = info.GetAllTraits()[index];
        contributionText += selected.contributionExplanation[Random.Range(0, selected.contributionExplanation.Length)];
        
        TextBoxManager.i.SetReportText(contributionText);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }

    public void ShowFamily()
    {
        HiringManager.i.SpendInterviewMoney(2);
        TextBoxManager.i.HideTextBox();

        TextBoxManager.i.SetReportText(info.hasFamily ? ("Sim, " + "Eu tenho " + info.sonsQtd.ToString() + (info.sonsQtd == 1 ? " Filho." : " Filhos.")) : "Não");
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }


    public void ShowTraits()
    {
        HiringManager.i.SpendInterviewMoney(5);
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
