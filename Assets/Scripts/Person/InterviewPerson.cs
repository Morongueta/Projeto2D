using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewPerson : Person
{
    private string[] startContribution = new string[] {"Na minha ultima empresa ", "No meu ultimo emprego ", "Bom, na empresa que eu trabalhei "};
    private string[] startAboutYou = new string[] {"Bem, ", "Acredito que, ", "Bom, "};

    private bool aboutYou = false, contribution = false, quality = false, family = false;
    public override void SetupEvent()
    {
        SetupChatButton();
        inFrontEvent += () => {
            chatButton.gameObject.SetActive(true);
            HiringManager.i.ResetInterviewMoney();
        };

        goingAwayEvent += () => {TextBoxManager.i.HideTextBox();};

        base.SetupEvent();
    }

    public override void SetupChatButton()
    {
        chatButton.OnClickAction += () => {

            if(!TextBoxManager.i.showingTextBox)
            {
                SetInterviewBox();
                chatButton.SetSprite(closeSprite);
            }else{
                TextBoxManager.i.HideTextBox();
                chatButton.SetSprite(defaultSprite);
            } 
        };
    }

    public void SetInterviewBox()
    {
        int money = HiringManager.i.GetInverviewMoney();

        TextBoxManager.i.ShowInterviewMoney(money.ToString());
        TextBoxManager.i.ShowInterview((money >= 1),(money >= 2 && !aboutYou),(money >= 1 && !family),(money >= 3 && !contribution),(money >= 5 && !quality));
        TextBoxManager.i.ShowInterview(()=>ShowName(),()=>ShowAboutYou(),()=>ShowFamily(),()=>ShowContribution(),() => ShowTraits());
    }

    public void ShowName()
    {
        HiringManager.i.SpendInterviewMoney(1);
        TextBoxManager.i.HideTextBox();

        TextBoxManager.i.SetReportText("Meu nome é " + info.c.personName);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

    }

    public void ShowAboutYou()
    {
        HiringManager.i.SpendInterviewMoney(2);
        TextBoxManager.i.HideTextBox();

        string aboutYouText = startAboutYou[Random.Range(0,startAboutYou.Length)];

        int hideInfoAmount = 3;

        int hideInfo = 0;
        
        for (int i = 0; i < info.GetAllTraits().Length; i++)
        {
            //aboutYouText += "\n-";
            if(hideInfo < hideInfoAmount)
            {
                if((Random.value * 100f) <= 12f)
                {
                    hideInfo++;
                    
                }else{
                    if((Random.value * 100f) <= 25f) //lie
                    {
                        Trait falseTrait = InformationDatabase.i.GetRandomTraits(1,(Random.value * 100f < 30f) ? TraitType.NEGATIVE : TraitType.POSITIVE, info.GetAllTraits())[0];
                        aboutYouText += falseTrait.traitDetail[Random.Range(0,falseTrait.traitDetail.Length)];
                    }else
                    {
                        aboutYouText += info.GetAllTraits()[i].traitDetail[Random.Range(0,info.GetAllTraits()[i].traitDetail.Length)];
                    }
                    if(i < info.GetAllTraits().Length - 1)aboutYouText += ",";
                }
            }

        }

        TextBoxManager.i.SetReportText(aboutYouText);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

        aboutYou = true;
    }

    public void ShowContribution()
    {
        HiringManager.i.SpendInterviewMoney(3);
        TextBoxManager.i.HideTextBox();

        string contributionText = startContribution[Random.Range(0,startContribution.Length)] + "eu fiquei por " + info.c.contributionTime + " meses.\n";
        
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

        contribution = true;
    }

    public void ShowFamily()
    {
        HiringManager.i.SpendInterviewMoney(1);
        TextBoxManager.i.HideTextBox();

        TextBoxManager.i.SetReportText(info.c.hasFamily ? ("Sim, " + "Eu tenho " + info.c.sonsQtd.ToString() + (info.c.sonsQtd == 1 ? " Filho." : " Filhos.")) : "Não");
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});
        
        family = true;
    }


    public void ShowTraits()
    {
        HiringManager.i.SpendInterviewMoney(5);
        TextBoxManager.i.HideTextBox();

        string traitsText = "";
        traitsText += "<color=#FFFFFF>Positivos:\n<color=#00C119>";

        for (int i = 0; i < info.c.positiveTraits.Length; i++)
        {
            if(i == info.c.positiveTraits.Length - 1)traitsText += " e ";
            traitsText += info.c.positiveTraits[i].name;
            if(i < info.c.positiveTraits.Length - 2) traitsText += ", ";
        }

        traitsText += "\n";
        traitsText += "<color=#FFFFFF>Negativos:\n<color=#EA1A23>";

        for (int i = 0; i < info.c.negativeTraits.Length; i++)
        {
            if(i == info.c.negativeTraits.Length - 1)traitsText += " e ";
            traitsText += info.c.negativeTraits[i].name;
            if(i < info.c.negativeTraits.Length - 2) traitsText += ", ";
        }

        traitsText += "\n";
        traitsText += "\n";

        TextBoxManager.i.SetReportText(traitsText);
        TextBoxManager.i.ShowReport(()=>{TextBoxManager.i.HideTextBox(); SetInterviewBox();});

        quality = true;
    }

    
}
